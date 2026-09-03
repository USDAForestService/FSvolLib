import { existsSync } from "node:fs";
import path from "node:path";
import { pathToFileURL } from "node:url";

export interface FSvolLibModuleOptions {
  artifactDir?: string;
}

export interface VersionInfo {
  version: string;
}

export const VolumeCalculationType = {
  FVS: 0,
  FIA: 1,
  CRUISE: 2,
  VARIABLE_LOG_LENGTH: 3
} as const;

export type VolumeCalculationType = (typeof VolumeCalculationType)[keyof typeof VolumeCalculationType];

export interface VolumeCalculationOptions {
  fiaCode?: number;
  auxFlag?: number;
  region?: number;
  forest?: number;
  district?: number;
  primaryProduct?: number;
  secondaryProduct?: number;
  volumeCalculationOptions?: number;
  volumeEquationNumberOverride?: string;
  ecoRegion?: string;
  basalArea?: number;
  siteIndex?: number;
}

export interface TreeMeasurement {
  totalHeight?: number;
  referenceHeight?: number;
  merchHeightSaw?: number;
  merchHeightNonsaw?: number;
  merchHeightUnit?: number;
  heightToFirstLiveLimb?: number;
  heightToTopBroken?: number;
  isLive?: boolean;
  dbh?: number;
  drc?: number;
  referenceDiameter?: number;
  topBrokenDiameter?: number;
  formClass?: number;
  stumpHeightOverride?: number;
  minTopDibSawOverride?: number;
  minTopDibNonSawOverride?: number;
  cull?: number;
  decaycd?: number;
  crownRatio?: number;
  stems?: number;
}

export interface VolumeOutput {
  grossBoardFootPrimary: number;
  grossBoardFootSecondary: number;
  grossCubicFootPrimary: number;
  grossCubicFootSecondary: number;
  grossInternationalBoardFoot: number;
  totalCubicFoot: number;
  stumpCubicFoot: number;
  tipCubicFoot: number;
  cordMerchantable: number;
  numberOfLogs: number;
  errflag: number;
}

const DEFAULT_WASM_DIR = path.resolve(import.meta.dirname, "..", "wasm");

let modulePromise: Promise<any> | null = null;
let initializedArtifactDir: string | null = null;
let configuredArtifactDir: string | null = null;

function isRepoRoot(candidatePath: string): boolean {
  return existsSync(path.join(candidatePath, "build-wasm.ps1"));
}

export function resolveRepoRoot(startPath: string = process.cwd()): string {
  let current = path.resolve(startPath);

  while (true) {
    if (isRepoRoot(current)) {
      return current;
    }

    const parent = path.dirname(current);
    if (parent === current) {
      throw new Error(`Could not locate repo root from '${startPath}'.`);
    }

    current = parent;
  }
}

export function resolveWasmArtifactDirectory(candidatePath: string = DEFAULT_WASM_DIR): string {
  const resolved = path.resolve(candidatePath);

  if (existsSync(path.join(resolved, "FSvolLibInterop.js"))) {
    return resolved;
  }

  try {
    const repoFallback = path.join(resolveRepoRoot(resolved), "src", "FSvolLibJS", "wasm");
    if (existsSync(path.join(repoFallback, "FSvolLibInterop.js"))) {
      return repoFallback;
    }
  } catch {
    // ignore and use the requested directory so the caller sees a direct, actionable error below.
  }

  return resolved;
}

export function getWasmInteropEntryPath(artifactDir: string = DEFAULT_WASM_DIR): string {
  return path.join(resolveWasmArtifactDirectory(artifactDir), "FSvolLibInterop.js");
}

export function wasmInteropExists(artifactDir: string = DEFAULT_WASM_DIR): boolean {
  return existsSync(getWasmInteropEntryPath(artifactDir));
}

export function resetFSvolLibModule(): void {
  modulePromise = null;
  initializedArtifactDir = null;
  configuredArtifactDir = null;
}

export async function initFSvolLibModule(options: FSvolLibModuleOptions = {}): Promise<any> {
  const desiredArtifactDir = options.artifactDir ?? DEFAULT_WASM_DIR;
  const resolvedArtifactDir = resolveWasmArtifactDirectory(desiredArtifactDir);
  const entryPath = path.join(resolvedArtifactDir, "FSvolLibInterop.js");
  const wasmPath = path.join(resolvedArtifactDir, "FSvolLibInterop.wasm");

  if (modulePromise) {
    if (configuredArtifactDir && path.resolve(configuredArtifactDir) !== path.resolve(resolvedArtifactDir)) {
      throw new Error(
        `FSvolLib WASM module was already initialized from '${configuredArtifactDir}'. ` +
          `Re-use that directory or call resetFSvolLibModule() before initializing from '${resolvedArtifactDir}'.`
      );
    }

    return modulePromise;
  }

  if (!existsSync(entryPath) || !existsSync(wasmPath)) {
    throw new Error(
      `FSvolLib WASM artifacts were not found in '${resolvedArtifactDir}'. ` +
        `Expected '${entryPath}' and '${wasmPath}'. Run build-wasm.ps1 to stage the artifacts.`
    );
  }

  modulePromise = (async () => {
    try {
      const moduleUrl = pathToFileURL(entryPath).href;
      const { default: createModule } = await import(moduleUrl);
      const mod = await createModule({
        locateFile: (fileName: string) => {
          const candidate = path.join(resolvedArtifactDir, fileName);
          if (existsSync(candidate)) {
            return candidate;
          }

          return path.join(path.dirname(entryPath), fileName);
        }
      });

      if (!mod || typeof mod._GetVersion !== "function") {
        throw new Error("FSvolLib WASM module loaded but _GetVersion was not exported.");
      }

      initializedArtifactDir = resolvedArtifactDir;
      configuredArtifactDir = resolvedArtifactDir;
      return mod;
    } catch (error) {
      resetFSvolLibModule();
      throw new Error(
        `Failed to initialize FSvolLib WASM module from '${resolvedArtifactDir}': ${
          error instanceof Error ? error.message : String(error)
        }`
      );
    }
  })();

  return modulePromise;
}

export async function loadFSvolLibModule(options: FSvolLibModuleOptions = {}): Promise<any> {
  return initFSvolLibModule(options);
}

export async function getFSvolLibVersion(): Promise<string> {
  const mod = await initFSvolLibModule();
  const bufferLength = 256;
  const outPtr = mod._malloc(bufferLength);

  try {
    mod._GetVersion(outPtr, bufferLength);
    return mod.UTF8ToString(outPtr);
  } finally {
    if (typeof mod._free === "function") {
      mod._free(outPtr);
    }
  }
}

export async function getVersion(): Promise<string> {
  return getFSvolLibVersion();
}

export async function getVersionInfo(): Promise<VersionInfo> {
  return { version: await getVersion() };
}

function allocateString(mod: any, value: string): number {
  if (!value) {
    return 0;
  }

  const bytes = new TextEncoder().encode(value);
  const ptr = mod._malloc(bytes.length + 1);
  mod.HEAPU8.set(bytes, ptr);
  mod.HEAPU8[ptr + bytes.length] = 0;
  return ptr;
}

function readCString(mod: any, ptr: number): string {
  if (!ptr) {
    return "";
  }

  return mod.UTF8ToString(ptr);
}

export async function calculateVolume(options: VolumeCalculationOptions, tree: TreeMeasurement): Promise<VolumeOutput> {
  const mod = await initFSvolLibModule();
  const heapView = new DataView(mod.HEAPU8.buffer, mod.HEAPU8.byteOffset, mod.HEAPU8.byteLength);

  const optionsPtr = mod._malloc(128);
  const treePtr = mod._malloc(256);
  const outputPtrPtr = mod._malloc(4);
  const errorPtrPtr = mod._malloc(4);

  const volumeEquationOverridePtr = allocateString(mod, options.volumeEquationNumberOverride ?? "");
  const ecoRegionPtr = allocateString(mod, options.ecoRegion ?? "");

  try {
    heapView.setInt32(optionsPtr + 0, options.fiaCode ?? 0, true);
    heapView.setUint8(optionsPtr + 4, options.auxFlag ?? 0);
    heapView.setUint8(optionsPtr + 5, 0);
    heapView.setUint8(optionsPtr + 6, 0);
    heapView.setUint8(optionsPtr + 7, 0);
    heapView.setInt32(optionsPtr + 8, options.region ?? 0, true);
    heapView.setInt32(optionsPtr + 12, options.forest ?? 0, true);
    heapView.setInt32(optionsPtr + 16, options.district ?? 0, true);
    heapView.setInt32(optionsPtr + 20, options.primaryProduct ?? 0, true);
    heapView.setInt32(optionsPtr + 24, options.secondaryProduct ?? 0, true);
    heapView.setInt32(
      optionsPtr + 28,
      options.volumeCalculationOptions ?? VolumeCalculationType.FVS,
      true
    );
    heapView.setInt32(optionsPtr + 32, volumeEquationOverridePtr, true);
    heapView.setInt32(optionsPtr + 36, ecoRegionPtr, true);
    heapView.setInt32(optionsPtr + 40, options.basalArea ?? 0, true);
    heapView.setInt32(optionsPtr + 44, options.siteIndex ?? 0, true);

    heapView.setFloat64(treePtr + 0, tree.totalHeight ?? 0, true);
    heapView.setFloat64(treePtr + 8, tree.referenceHeight ?? 0, true);
    heapView.setFloat64(treePtr + 16, tree.merchHeightSaw ?? 0, true);
    heapView.setFloat64(treePtr + 24, tree.merchHeightNonsaw ?? 0, true);
    heapView.setInt32(treePtr + 32, tree.merchHeightUnit ?? 0, true);
    heapView.setFloat64(treePtr + 40, tree.heightToFirstLiveLimb ?? 0, true);
    heapView.setFloat64(treePtr + 48, tree.heightToTopBroken ?? 0, true);
    heapView.setUint8(treePtr + 56, tree.isLive === false ? 0 : 1);
    heapView.setUint8(treePtr + 57, 0);
    heapView.setUint8(treePtr + 58, 0);
    heapView.setUint8(treePtr + 59, 0);
    heapView.setUint8(treePtr + 60, 0);
    heapView.setUint8(treePtr + 61, 0);
    heapView.setUint8(treePtr + 62, 0);
    heapView.setUint8(treePtr + 63, 0);
    heapView.setFloat64(treePtr + 64, tree.dbh ?? 0, true);
    heapView.setFloat64(treePtr + 72, tree.drc ?? 0, true);
    heapView.setFloat64(treePtr + 80, tree.referenceDiameter ?? 0, true);
    heapView.setFloat64(treePtr + 88, tree.topBrokenDiameter ?? 0, true);
    heapView.setInt32(treePtr + 96, tree.formClass ?? 0, true);
    heapView.setFloat64(treePtr + 104, tree.stumpHeightOverride ?? 0, true);
    heapView.setFloat64(treePtr + 112, tree.minTopDibSawOverride ?? 0, true);
    heapView.setFloat64(treePtr + 120, tree.minTopDibNonSawOverride ?? 0, true);
    heapView.setFloat64(treePtr + 128, tree.cull ?? 0, true);
    heapView.setInt32(treePtr + 136, tree.decaycd ?? 0, true);
    heapView.setFloat64(treePtr + 144, tree.crownRatio ?? 0, true);
    heapView.setInt32(treePtr + 152, tree.stems ?? 0, true);

    heapView.setInt32(outputPtrPtr, 0, true);
    heapView.setInt32(errorPtrPtr, 0, true);

    const succeeded = Boolean(mod._CalculateVolume(optionsPtr, treePtr, outputPtrPtr, errorPtrPtr));
    const outputPtr = heapView.getInt32(outputPtrPtr, true);
    const errorPtr = heapView.getInt32(errorPtrPtr, true);

    if (!succeeded) {
      if (errorPtr !== 0) {
        const errorMessagePtr = heapView.getInt32(errorPtr + 4, true);
        const message = readCString(mod, errorMessagePtr);
        mod._free_error_info_c(errorPtr);
        throw new Error(message || "FSvolLib calculation failed.");
      }

      throw new Error("FSvolLib calculation failed.");
    }

    if (outputPtr === 0) {
      throw new Error("FSvolLib returned a null output pointer.");
    }

    const result: VolumeOutput = {
      grossBoardFootPrimary: heapView.getFloat64(outputPtr + 0, true),
      grossBoardFootSecondary: heapView.getFloat64(outputPtr + 8, true),
      grossCubicFootPrimary: heapView.getFloat64(outputPtr + 16, true),
      grossCubicFootSecondary: heapView.getFloat64(outputPtr + 24, true),
      grossInternationalBoardFoot: heapView.getFloat64(outputPtr + 32, true),
      totalCubicFoot: heapView.getFloat64(outputPtr + 40, true),
      stumpCubicFoot: heapView.getFloat64(outputPtr + 48, true),
      tipCubicFoot: heapView.getFloat64(outputPtr + 56, true),
      cordMerchantable: heapView.getFloat64(outputPtr + 64, true),
      numberOfLogs: 0,
      errflag: 0
    };

    if (typeof mod._free_tree_output_c === "function") {
      mod._free_tree_output_c(outputPtr);
    }

    return result;
  } finally {
    if (volumeEquationOverridePtr !== 0 && typeof mod._free === "function") {
      mod._free(volumeEquationOverridePtr);
    }

    if (ecoRegionPtr !== 0 && typeof mod._free === "function") {
      mod._free(ecoRegionPtr);
    }

    if (typeof mod._free === "function") {
      mod._free(optionsPtr);
      mod._free(treePtr);
      mod._free(outputPtrPtr);
      mod._free(errorPtrPtr);
    }
  }
}

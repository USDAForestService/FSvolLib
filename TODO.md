# FSvolLibJS Implementation Plan

## Goal

Build `FSvolLibJS` as the JavaScript/TypeScript glue layer between the compiled WASM module and consumer applications, with `FSvolLibJS.test` validating runtime behavior and API compatibility (not build-pipeline checks).

## Phase 0: Scope and Contract

- [x] Define the first supported API slice for JS consumers.
	Decision: expose `GetVersion` first (from `VolumeLibrary.interop.h`) to validate JS-to-WASM interop with a minimal, deterministic call.
- [x] Identify which native operations will be exposed in v1.
	Decision: include one end-to-end volume calculation path using validated fixtures from `src/FSvolLib.test/VolumeLibrary.test.cpp` (`TreeMeasurment` + `VolumeCalculationOptions`).
- [x] Decide runtime target(s).
	Decision: Node-only for v1.
- [x] Define error contract: deterministic JS errors come from the C interop layer, not from native C++ exceptions crossing the boundary.
	Decision: native C++ failures are converted into `ErrorInfo_C` by reference in `VolumeLibrary.interop.h` / `.cpp`, and JS wrappers translate those into structured JS errors or result objects.
- [x] Document v1 non-goals.
	Decision: v1 scope is only methods defined in `VolumeLibrary.interop.h` (no broader FSvolLib API parity).

## Phase 1: Native WASM Export Surface

- [x] Choose export strategy for WASM interop.
	Decision: use C ABI exports (`extern "C"`) first, then evaluate embind later if needed.
- [x] Enable C interface target in `emscripten-wasm` preset.
	Implemented: preset now sets `BUILD_INTEROP=ON`, and CMake emits a Node-focused interop JS/WASM target in wasm builds.
- [x] Create a minimal, stable C-compatible entrypoint for v1.
	Implemented via the public C ABI surface in `VolumeLibrary.interop.h`/`.cpp` and the generated Node-targeted JS/WASM module.
- [x] Ensure exported function signatures are versionable and ABI-safe.
	Implemented in practice by keeping a small C ABI surface and avoiding C++ exceptions crossing the boundary; follow-up: document explicit ABI-versioning policy and compatibility expectations.
- [x] Emit required WASM artifacts for JS consumption (`.wasm` plus Node loader/glue output).
	Verified by the successful `build-wasm.ps1` run generating the interop output artifacts.
- [x] Confirm `GetVersion` export works via a direct Node invocation before adding higher-level wrappers.
	Verified by the runtime test in `FSvolLibJS.test` against the generated interop module.

## Phase 2: FSvolLibJS Runtime Loader

- [x] Implement module loader that initializes WASM once and reuses the instance.
- [x] Support async initialization (`init()` / lazy-init guard).
- [x] Allow explicit artifact path override for app integration.
- [x] Implement Node-focused runtime assumptions for v1 (browser support deferred).
- [x] Add predictable initialization errors with actionable messages.

## Phase 3: TypeScript API Layer

- [ ] Define public TS interfaces for request/response models.
- [ ] Implement `getVersion()` wrapper mapped to the WASM export.
- [ ] Implement one typed volume-calculation wrapper method mapped to the selected v1 native path.
- [ ] Implement marshalling utilities for strings, numbers, and structured outputs.
- [ ] Map native `ErrorInfo_C` codes/messages to typed JS error objects without letting C++ exceptions cross the interop boundary.
- [ ] Keep API small and stable for first publish.

Status: Phase 3 has started with the public `getVersion()` API wrapper and loader surface being formalized.

## Phase 4: Packaging and Distribution

- [ ] Decide package layout for runtime + artifacts.
- [ ] Configure package exports for ESM (and CJS only if needed).
- [ ] Include `.d.ts` output and source maps as appropriate.
- [ ] Ensure WASM artifacts are included in package files.
- [ ] Add versioning policy for API and ABI changes.

## Phase 5: FSvolLibJS.test Runtime Tests

- [ ] Replace build-pipeline smoke test with runtime behavior tests.
- [ ] Add init test: WASM loader initializes successfully.
- [ ] Add `getVersion()` test: returns non-empty, stable version string.
- [ ] Add happy-path test: one known calculation returns expected values.
- [ ] Source expected happy-path values from `src/FSvolLib.test/VolumeLibrary.test.cpp` test vectors.
- [ ] Add validation test: bad inputs return expected JS errors based on the interop `ErrorInfo_C` contract.
- [ ] Add determinism test: repeated calls return stable results.
- [ ] Add contract tests for public TS shapes.

## Phase 6: Quality Gates

- [ ] Add typecheck and test scripts to run in CI.
- [ ] Add API snapshot or contract guard for breaking changes.
- [ ] Add regression tests for any fixed interop bugs.
- [ ] Add performance baseline test for a representative call.

## Phase 7: Documentation

- [ ] Write a quickstart for consuming `FSvolLibJS`.
- [ ] Document initialization lifecycle and resource cleanup.
- [ ] Document artifact location and override patterns.
- [ ] Document supported platforms/environments and known limits.

## Proposed Implementation Order

1. Implement/export `GetVersion` and verify direct Node call against WASM.
2. Build FSvolLibJS loader and ship `getVersion()` wrapper.
3. Replace FSvolLibJS.test pipeline smoke test with runtime init + `getVersion()` tests.
4. Implement one end-to-end calculation wrapper using validated native test vectors.
5. Expand remaining `VolumeLibrary.interop.h` methods incrementally with tests per method.

## Review Questions

- [x] Prefer C ABI exports first, or embind first?
	Answer: C ABI first.
- [x] Node-first only for v1, or Node + browser immediately?
	Answer: Node-first only for v1.
- [ ] Which single volume calculation path should be the first public JS method?
- [ ] Should v1 include structured logging hooks for debugging interop calls?


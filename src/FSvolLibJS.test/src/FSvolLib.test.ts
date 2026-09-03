import { describe, expect, it } from "vitest";

import {
  calculateVolume,
  getVersion,
  getFSvolLibVersion,
  initFSvolLibModule,
  loadFSvolLibModule,
  VolumeCalculationType
} from "@fsvollib/js";

describe("FSvolLib integration tests", () => {
  it("initializes once and exposes the GetVersion export", async () => {
    const mod = await initFSvolLibModule();
    expect(mod).toBeTruthy();
    expect(typeof mod._GetVersion).toBe("function");

    const version = await getFSvolLibVersion();
    expect(version).toEqual(expect.any(String));
    expect(version.length).toBeGreaterThan(0);
  }, 300000);

  it("reuses the initialized module instance for lazy init", async () => {
    const { initFSvolLibModule, loadFSvolLibModule } = await import("@fsvollib/js");

    const first = await initFSvolLibModule();
    const second = await loadFSvolLibModule();

    expect(first).toBe(second);
    expect(typeof first._GetVersion).toBe("function");
  }, 300000);

  it("exposes the public getVersion() wrapper without per-call artifact configuration", async () => {
    const version = await getVersion();
    expect(version).toEqual(expect.any(String));
    expect(version.length).toBeGreaterThan(0);
  }, 300000);

  it("calculates a valid volume result using the native C ABI path", async () => {
    const result = await calculateVolume({
      region: 2,
      forest: 1,
      fiaCode: 100,
      primaryProduct: 1,
      secondaryProduct: 2,
      volumeCalculationOptions: VolumeCalculationType.FIA,
      volumeEquationNumberOverride: "R03CHO0066"
    }, {
      dbh: 19.7,
      totalHeight: 76.0,
      referenceHeight: 0.0,
      referenceDiameter: 0.0,
      merchHeightSaw: 0.0,
      merchHeightNonsaw: 0.0,
      formClass: 80,
      heightToTopBroken: 0.0,
      topBrokenDiameter: 0.0,
      isLive: true
    });

    expect(result.grossCubicFootPrimary).toBeGreaterThan(0);
    expect(result.totalCubicFoot).toBeGreaterThan(0);
  }, 300000);
});

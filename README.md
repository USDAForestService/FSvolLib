# FSvolLib

A portable C++ static library for forest volume and biomass calculations.

---

## Development Environment Setup

### Prerequisites

| Tool | Minimum Version | Notes |
|------|----------------|-------|
| **Visual Studio 2022** | 17.x | Install the **Desktop development with C++** workload |
| CMake | 3.21 | Installed with VS **Desktop development with C++** workload |

---

### Visual Studio

1. Start Visual Studio
2. **Open the folder** — use *File → Open → Folder* and select this repository's folder. Visual Studio will automatically detect `CMakeLists.tst` and `CMakePresets.json` in `src/FSvolLib`.
2. **Select a preset** — use the toolbar dropdowns to pick a configure preset (e.g. *Windows Debug x64*) and corresponding build preset.
3. **Build** — *Build → Build All* (`Ctrl+Shift+B`).
4. **Run tests** — open *Test → Test Explorer* and click *Run All*. Tests are discovered via CTest/GTest.


The CMake presets are configured for the **Visual Studio 17 2022** generator.

> If Visual Studio does not pick up the presets automatically, go to *Project → CMake Settings* and confirm the `CMakePresets.json` path is correct.

---

### Building from the Command Line

```powershell
# From src/FSvolLib
cmake --preset win-debug-x64
cmake --build --preset win-debug-x64-build
ctest --preset win-debug-x64  # if a test preset is defined, otherwise:
cd out/build/win-debug-x64
ctest -C Debug
```

---

### Available Presets

| Preset name | Platform | Config | Generator |
|-------------|----------|--------|-----------|
| `win-debug-x64` | Windows x64 | Debug | Visual Studio 17 2022 |
| `win-release-x64` | Windows x64 | Release | Visual Studio 17 2022 |
| `emscripten-wasm` | WASM | Release | Ninja |
| `android-arm64` | Android ARM64 | Release | Ninja |

> The `emscripten-wasm` and `android-arm64` presets require additional toolchain setup. See [src/FSvolLib/README.md](src/FSvolLib/README.md) for details.

---

### Build Output

All build artifacts are written to `src/FSvolLib/out/build/<preset-name>/` and are excluded from source control via `.gitignore`.

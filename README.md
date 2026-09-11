# Forest Service Volume Estimation Library

The Forest Service Volume Estimation Library is a library written in C++ that provides various mathematical models for calculating estimated volumes and biomass weights for trees.  

---

## Development Environment Setup

The core of the volume library uses a CMake build system. 

### C++ development

C++ develop depends on several tools:
 - CMake 3.20+ - build orchestration
 - Ninja - compilation and linking
 - Emscription - for wasm builds
 - Android NDK - for Android builds

 Excluding Emscription, these can be acquired individually, or you can use Visual Studio to install them for you. This build scripts default to assuming that these tools are installed using Visual Studio. The script `bootstrap-build-env`.ps1 can be modified to change default paths for tools.  

---

#### Visual Studio


##### Setup Build Environment
Using the Visual Studio Installer, install the "Desktop development with C++" and "Mobile development with C++" workloads. 

> As of Visual Studio 2026, the **Mobile development with C++** will say Out of Support. Microsoft is retiring mobile C++ builds using MSBuild. However the only component we require is the **Android NDK** which will continue to be supported in Visual Studio. If using a Version of Visual Studio newer than 2026 the **Android NDK** component may be moved to a different workload.

##### Opening CMake Project

1. Start Visual Studio
2. **Open the folder** — use *File → Open → Folder* and select this repository's folder. Visual Studio will automatically detect the entrypoint `src/CMakeLists.txt` and `src/CMakePresets.json`.
2. **Select a preset** — use the toolbar dropdowns to pick a configure preset (e.g. *Windows Debug x64*) and corresponding build preset.
3. **Build** — *Build → Build All* (`Ctrl+Shift+B`).
4. **Run tests** — open *Test → Test Explorer* and click *Run All*. Tests are discovered via CTest/GTest.

> If Visual Studio does not pick up the presets automatically, go to *Project → CMake Settings* and confirm the `CMakePresets.json` path is correct.

---

### C# development
For C# development use solution located at `src/cs/FSvolLib.Interop.slnx`

---

### Building from the Command Line

```powershell
# From the repository root
.\build.ps1
.\test.ps1
```

If CMake/Ninja are installed with the Visual Studio C++ workload (but not on `PATH`), this repo includes wrappers that locate them with `vswhere`:

```powershell
.\CMake.cmd --version
.\Ninja.cmd --version
```

For WASM builds, use the bootstrap script to add Visual Studio CMake/Ninja and load your local emsdk environment:

```powershell
# Default emsdk env script path:
# C:\Repos\clones\emsdk\emsdk_env.ps1
.\build-wasm.ps1

# Or override emsdk env script path
.\build-wasm.ps1 -EmsdkEnvPath 'D:\tools\emsdk\emsdk_env.ps1'
```

The scripts default to the standardized preset aliases `win-dev` for debug and `win-ci-release` for release, and they stage the native interop DLL into the managed output path so `dotnet build` and `dotnet test` can load `FSvolLibInterop.dll` without manual copying.

---

### Available Presets

| Preset name | Platform | Config | Generator |
|-------------|----------|--------|-----------|
| `win-dev` | Windows x64 | Debug | Visual Studio 17 2022 |
| `win-ci-debug` | Windows x64 | Debug | Visual Studio 17 2022 |
| `win-ci-release` | Windows x64 | Release | Visual Studio 17 2022 |
| `win-debug-x64` | Windows x64 | Debug | Visual Studio 17 2022 |
| `win-release-x64` | Windows x64 | Release | Visual Studio 17 2022 |
| `emscripten-wasm` | WASM | Release | Ninja |
| `android-arm64` | Android ARM64 | Release | Ninja |

> The `emscripten-wasm` and `android-arm64` presets require additional toolchain setup. See [src/FSvolLib/README.md](src/FSvolLib/README.md) for details.

---

### Build Output

All build artifacts are written to `src/FSvolLib/out/build/<preset-name>/` and are excluded from source control via `.gitignore`.

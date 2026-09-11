param(
    [string]$EmsdkEnvPath = 'C:\Repos\clones\emsdk\emsdk_env.ps1'
)

$ErrorActionPreference = 'Stop'

$repoRoot = Split-Path -Parent $MyInvocation.MyCommand.Path
$srcRoot = Join-Path $repoRoot 'src'
$buildArtifactRoot = Join-Path $srcRoot 'out\build\emscripten-wasm\FSvolLib\FSvolLib.interop'
$packageArtifactRoot = Join-Path $repoRoot 'src\js\FSvolLibJS\wasm'

& (Join-Path $repoRoot 'bootstrap-build-env.ps1') -UseEmsdk -EmsdkEnvPath $EmsdkEnvPath

Push-Location $srcRoot
try {
    cmake --preset emscripten-wasm
    if ($LASTEXITCODE -ne 0) { throw 'cmake configure failed for emscripten-wasm preset' }

    cmake --build --preset emscripten-wasm-build
    if ($LASTEXITCODE -ne 0) { throw 'cmake build failed for emscripten-wasm preset' }
}
finally {
    Pop-Location
}

New-Item -ItemType Directory -Force -Path $packageArtifactRoot | Out-Null
Copy-Item (Join-Path $buildArtifactRoot 'FSvolLibInterop.js') (Join-Path $packageArtifactRoot 'FSvolLibInterop.js') -Force
Copy-Item (Join-Path $buildArtifactRoot 'FSvolLibInterop.wasm') (Join-Path $packageArtifactRoot 'FSvolLibInterop.wasm') -Force

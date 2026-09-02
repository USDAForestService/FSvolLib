param(
    [string]$EmsdkEnvPath = 'C:\Repos\clones\emsdk\emsdk_env.ps1'
)

$ErrorActionPreference = 'Stop'

$repoRoot = Split-Path -Parent $MyInvocation.MyCommand.Path
$srcRoot = Join-Path $repoRoot 'src'

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

param(
    [ValidateSet('Debug', 'Release')]
    [string]$Configuration = 'Release',

    [string]$NativePreset = 'android-arm64'
)

$ErrorActionPreference = 'Stop'

$repoRoot = Split-Path -Parent $MyInvocation.MyCommand.Path
$srcRoot = Join-Path $repoRoot 'src'

& (Join-Path $repoRoot 'bootstrap-build-env.ps1') -Quiet

if (-not $env:ANDROID_NDK) {
    throw 'ANDROID_NDK is not set. Run bootstrap-build-env.ps1 with a valid NDK path.'
}

Push-Location $srcRoot
try {
    cmake --preset $NativePreset -D CMAKE_BUILD_TYPE=$Configuration
    if ($LASTEXITCODE -ne 0) { throw "cmake configure failed for preset $NativePreset" }

    cmake --build --preset "$NativePreset-build"
    if ($LASTEXITCODE -ne 0) { throw "cmake build failed for preset $NativePreset" }
}
finally {
    Pop-Location
}
param(
    [ValidateSet('Debug', 'Release')]
    [string]$Configuration = 'Debug',

    [ValidateSet('win-dev', 'win-ci-debug', 'win-ci-release')]
    [string]$NativePreset
)

$ErrorActionPreference = 'Stop'

$repoRoot = Split-Path -Parent $MyInvocation.MyCommand.Path
$srcRoot = Join-Path $repoRoot 'src'

# Ensure CMake/Ninja from the local Visual Studio installation are on PATH.
& (Join-Path $repoRoot 'bootstrap-build-env.ps1') -Quiet

if (-not $NativePreset) {
    $NativePreset = if ($Configuration -eq 'Release') { 'win-ci-release' } else { 'win-dev' }
}

$nativeBuildDir = Join-Path $repoRoot "src\cpp\FSvolLib\out\build\$NativePreset"
$nativeInteropDir = Join-Path $nativeBuildDir "FSvolLib\FSvolLib.interop\$Configuration"

Push-Location $srcRoot
try {
    cmake --preset $NativePreset -D BUILD_INTEROP=ON
    if ($LASTEXITCODE -ne 0) { throw "cmake configure failed for preset $NativePreset" }

    cmake --build --preset "$NativePreset-build" --config $Configuration
    if ($LASTEXITCODE -ne 0) { throw "cmake build failed for preset $NativePreset" }
}
finally {
    Pop-Location
}

dotnet build (Join-Path $repoRoot 'src\cs\FSvolLib.Interop\FSvolLib.Interop.csproj') `
    -c $Configuration `
    /p:NativeInteropDir=$nativeInteropDir

dotnet build (Join-Path $repoRoot 'src\cs\FSvolLib.Interop.Test\FSvolLib.Interop.test.csproj') `
    -c $Configuration `
    /p:NativeInteropDir=$nativeInteropDir
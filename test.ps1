param(
    [ValidateSet('Debug', 'Release')]
    [string]$Configuration = 'Debug',

    [ValidateSet('win-dev', 'win-ci-debug', 'win-ci-release')]
    [string]$NativePreset
)

$ErrorActionPreference = 'Stop'

$repoRoot = Split-Path -Parent $MyInvocation.MyCommand.Path
$srcRoot = Join-Path $repoRoot 'src'

if (-not $NativePreset) {
    $NativePreset = if ($Configuration -eq 'Release') { 'win-ci-release' } else { 'win-dev' }
}

$nativeBuildDir = Join-Path $repoRoot "src\cpp\FSvolLib\out\build\$NativePreset"
$nativeInteropDir = Join-Path $nativeBuildDir "FSvolLib\FSvolLib.interop\$Configuration"

# call build script to build native and managed code
& (Join-Path $repoRoot 'build.ps1') -Configuration $Configuration -NativePreset $NativePreset
if ($LASTEXITCODE -ne 0) { throw "build.ps1 failed for preset $NativePreset" }

# run native tests
Push-Location $srcRoot
try {
    ctest --preset "$NativePreset-test" --output-on-failure
    if ($LASTEXITCODE -ne 0) { throw "ctest failed for preset $NativePreset" }
}
finally {
    Pop-Location
}

# run C# managed tests
dotnet test (Join-Path $repoRoot 'src\cs\FSvolLib.net.test\FSvolLib.net.test.csproj') `
    -c $Configuration `
    --no-build `
    /p:NativeInteropDir=$nativeInteropDir
if ($LASTEXITCODE -ne 0) { throw "dotnet test failed for preset $NativePreset" }
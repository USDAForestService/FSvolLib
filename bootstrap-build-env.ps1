param(
    [string]$EmsdkEnvPath = 'C:\Repos\clones\emsdk\emsdk_env.ps1',
    [string]$AndroidNdkPath = 'C:\Program Files (x86)\Android\AndroidNDK\android-ndk-r27c',
    [switch]$UseEmsdk,
    [switch]$Quiet
)

$ErrorActionPreference = 'Stop'

function Get-VsWherePath {
    $path = Join-Path ${env:ProgramFiles(x86)} 'Microsoft Visual Studio\Installer\vswhere.exe'
    if (-not (Test-Path $path)) {
        throw "vswhere.exe was not found at '$path'. Install Visual Studio Installer components."
    }

    return $path
}

function Find-VsToolPath {
    param(
        [Parameter(Mandatory)]
        [string]$RelativePath,
        [string]$RequiredComponent
    )

    $vswhere = Get-VsWherePath
    $args = @('-latest', '-prerelease', '-products', '*')
    if ($RequiredComponent) {
        $args += @('-requires', $RequiredComponent)
    }
    $args += @('-find', $RelativePath)

    $resolved = & $vswhere @args 2>$null | Select-Object -First 1
    if (-not $resolved) {
        throw "Unable to locate '$RelativePath' via vswhere."
    }

    return $resolved.Trim()
}

function Find-ToolOnPath {
    param([Parameter(Mandatory)][string]$CommandName)

    $command = Get-Command $CommandName -ErrorAction SilentlyContinue
    if (-not $command) {
        return $null
    }

    return $command.Source
}

function Add-ToPathIfMissing {
    param([Parameter(Mandatory)][string]$Directory)

    if (-not (Test-Path $Directory)) {
        return
    }

    $segments = ($env:PATH -split ';') | Where-Object { $_ -ne '' }
    if ($segments -notcontains $Directory) {
        $env:PATH = "$Directory;$env:PATH"
    }
}

function Resolve-AndroidNdkPath {
    param([string]$ConfiguredPath)

    if ($env:ANDROID_NDK -and (Test-Path $env:ANDROID_NDK)) {
        return $env:ANDROID_NDK
    }

    if ($env:ANDROID_NDK_ROOT -and (Test-Path $env:ANDROID_NDK_ROOT)) {
        return $env:ANDROID_NDK_ROOT
    }

    if ($ConfiguredPath -and (Test-Path $ConfiguredPath)) {
        return $ConfiguredPath
    }

    return $null
}

$cmakePath = Find-ToolOnPath -CommandName 'cmake'
if (-not $cmakePath) {
    $cmakePath = Find-VsToolPath -RelativePath 'Common7\IDE\CommonExtensions\Microsoft\CMake\CMake\bin\cmake.exe' -RequiredComponent 'Microsoft.VisualStudio.Component.VC.CMake.Project'
}

$ninjaPath = Find-ToolOnPath -CommandName 'ninja'
if (-not $ninjaPath) {
    $ninjaPath = Find-VsToolPath -RelativePath 'Common7\IDE\CommonExtensions\Microsoft\CMake\Ninja\ninja.exe' -RequiredComponent 'Microsoft.VisualStudio.Component.VC.CMake.Project'
}

Add-ToPathIfMissing -Directory (Split-Path -Parent $cmakePath)
Add-ToPathIfMissing -Directory (Split-Path -Parent $ninjaPath)

$env:FSVOLLIB_CMAKE = $cmakePath
$env:FSVOLLIB_NINJA = $ninjaPath

$androidNdkPath = Resolve-AndroidNdkPath -ConfiguredPath $AndroidNdkPath
if ($androidNdkPath) {
    $env:ANDROID_NDK = $androidNdkPath
    $env:ANDROID_NDK_ROOT = $androidNdkPath
}

if ($UseEmsdk) {
    if (-not (Test-Path $EmsdkEnvPath)) {
        throw "Emscripten env script not found at '$EmsdkEnvPath'."
    }

    . $EmsdkEnvPath

    if (-not $env:EMSDK) {
        $emsdkRoot = Split-Path -Parent $EmsdkEnvPath
        $env:EMSDK = $emsdkRoot
    }
}

if (-not $Quiet) {
    Write-Host "Configured tools:" -ForegroundColor Cyan
    Write-Host "  CMake : $cmakePath"
    Write-Host "  Ninja : $ninjaPath"
    if ($androidNdkPath) {
        Write-Host "  NDK   : $androidNdkPath"
    }
    if ($UseEmsdk) {
        Write-Host "  EMSDK : $env:EMSDK"
        Write-Host "  emcc  : $env:EM_CONFIG"
    }
}

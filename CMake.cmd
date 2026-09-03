@echo off
setlocal

set "VSWHERE=%ProgramFiles(x86)%\Microsoft Visual Studio\Installer\vswhere.exe"
if not exist "%VSWHERE%" (
  echo vswhere.exe not found at "%VSWHERE%"
  exit /b 1
)

set "CMakePath="
for /f "usebackq delims=" %%i in (`"%VSWHERE%" -latest -prerelease -products * -requires Microsoft.VisualStudio.Component.VC.CMake.Project -find Common7\IDE\CommonExtensions\Microsoft\CMake\CMake\bin\cmake.exe`) do (
  set "CMakePath=%%i"
)

if not defined CMakePath (
  echo CMake not found via vswhere
  exit /b 1
)

if not exist "%CMakePath%" (
  echo CMake path does not exist: "%CMakePath%"
  exit /b 1
)

"%CMakePath%" %*
exit /b %errorlevel%

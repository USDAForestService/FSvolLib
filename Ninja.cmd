@echo off
setlocal

set "VSWHERE=%ProgramFiles(x86)%\Microsoft Visual Studio\Installer\vswhere.exe"
if not exist "%VSWHERE%" (
  echo vswhere.exe not found at "%VSWHERE%"
  exit /b 1
)

set "NinjaPath="
for /f "usebackq delims=" %%i in (`"%VSWHERE%" -latest -prerelease -products * -find Common7\IDE\CommonExtensions\Microsoft\CMake\Ninja\ninja.exe`) do (
  set "NinjaPath=%%i"
)

if not defined NinjaPath (
  echo Ninja not found via vswhere
  exit /b 1
)

if not exist "%NinjaPath%" (
  echo Ninja path does not exist: "%NinjaPath%"
  exit /b 1
)

"%NinjaPath%" %*
exit /b %errorlevel%

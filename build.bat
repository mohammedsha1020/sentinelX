@echo off
echo ========================================
echo SentinelX - Build Script
echo ========================================
echo.

echo Checking prerequisites...
where msbuild >nul 2>nul
if %errorlevel% neq 0 (
    echo ERROR: MSBuild not found. Please install Visual Studio or Build Tools.
    pause
    exit /b 1
)

echo Building SentinelX...
msbuild SentinelX.sln /p:Configuration=Release /p:Platform="Any CPU"

if %errorlevel% equ 0 (
    echo.
    echo ========================================
    echo Build successful!
    echo ========================================
    echo.
    echo Executable location: SentinelX\bin\Release\SentinelX.exe
    echo.
    echo IMPORTANT: Run as Administrator for full functionality
    echo.
    echo Default login credentials:
    echo Username: admin
    echo Password: admin123
    echo.
    echo Press any key to open the executable location...
    pause >nul
    explorer SentinelX\bin\Release\
) else (
    echo.
    echo ========================================
    echo Build failed!
    echo ========================================
    echo.
    echo Please check:
    echo 1. Visual Studio is installed
    echo 2. .NET Framework 4.7.2+ is installed
    echo 3. NuGet packages are restored
    echo.
    pause
) 
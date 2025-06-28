# SentinelX PowerShell Build Script
param(
    [string]$Configuration = "Release",
    [string]$Platform = "Any CPU"
)

Write-Host "========================================" -ForegroundColor Green
Write-Host "SentinelX - PowerShell Build Script" -ForegroundColor Green
Write-Host "========================================" -ForegroundColor Green
Write-Host ""

# Check for Visual Studio installations
$vsPaths = @(
    "${env:ProgramFiles(x86)}\Microsoft Visual Studio\2019\Community\MSBuild\Current\Bin\MSBuild.exe",
    "${env:ProgramFiles(x86)}\Microsoft Visual Studio\2019\Professional\MSBuild\Current\Bin\MSBuild.exe",
    "${env:ProgramFiles(x86)}\Microsoft Visual Studio\2019\Enterprise\MSBuild\Current\Bin\MSBuild.exe",
    "${env:ProgramFiles}\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe",
    "${env:ProgramFiles}\Microsoft Visual Studio\2022\Professional\MSBuild\Current\Bin\MSBuild.exe",
    "${env:ProgramFiles}\Microsoft Visual Studio\2022\Enterprise\MSBuild\Current\Bin\MSBuild.exe"
)

$msbuildPath = $null
foreach ($path in $vsPaths) {
    if (Test-Path $path) {
        $msbuildPath = $path
        break
    }
}

if ($msbuildPath) {
    Write-Host "Found MSBuild at: $msbuildPath" -ForegroundColor Yellow
    Write-Host "Building SentinelX..." -ForegroundColor Yellow
    
    try {
        & $msbuildPath SentinelX.sln /p:Configuration=$Configuration /p:Platform=$Platform /v:minimal
        
        if ($LASTEXITCODE -eq 0) {
            Write-Host ""
            Write-Host "========================================" -ForegroundColor Green
            Write-Host "Build successful!" -ForegroundColor Green
            Write-Host "========================================" -ForegroundColor Green
            Write-Host ""
            Write-Host "Executable location: SentinelX\bin\$Configuration\SentinelX.exe" -ForegroundColor Cyan
            Write-Host ""
            Write-Host "IMPORTANT: Run as Administrator for full functionality" -ForegroundColor Red
            Write-Host ""
            Write-Host "Default login credentials:" -ForegroundColor Yellow
            Write-Host "Username: admin" -ForegroundColor White
            Write-Host "Password: admin123" -ForegroundColor White
            Write-Host ""
            
            $exePath = "SentinelX\bin\$Configuration\SentinelX.exe"
            if (Test-Path $exePath) {
                Write-Host "Opening executable location..." -ForegroundColor Green
                Start-Process "explorer.exe" -ArgumentList "/select,$exePath"
            }
        } else {
            Write-Host ""
            Write-Host "========================================" -ForegroundColor Red
            Write-Host "Build failed!" -ForegroundColor Red
            Write-Host "========================================" -ForegroundColor Red
            Write-Host ""
            Write-Host "Please check:" -ForegroundColor Yellow
            Write-Host "1. Visual Studio is installed" -ForegroundColor White
            Write-Host "2. .NET Framework 4.7.2+ is installed" -ForegroundColor White
            Write-Host "3. NuGet packages are restored" -ForegroundColor White
            Write-Host ""
        }
    }
    catch {
        Write-Host "Error during build: $($_.Exception.Message)" -ForegroundColor Red
    }
} else {
    Write-Host "========================================" -ForegroundColor Red
    Write-Host "MSBuild not found!" -ForegroundColor Red
    Write-Host "========================================" -ForegroundColor Red
    Write-Host ""
    Write-Host "Please install Visual Studio 2019 or later:" -ForegroundColor Yellow
    Write-Host "https://visualstudio.microsoft.com/downloads/" -ForegroundColor Cyan
    Write-Host ""
    Write-Host "Or install Visual Studio Build Tools:" -ForegroundColor Yellow
    Write-Host "https://visualstudio.microsoft.com/downloads/#build-tools-for-visual-studio-2022" -ForegroundColor Cyan
    Write-Host ""
    Write-Host "Make sure to install the '.NET desktop development' workload." -ForegroundColor Yellow
}

Write-Host ""
Write-Host "Press any key to continue..." -ForegroundColor Gray
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown") 
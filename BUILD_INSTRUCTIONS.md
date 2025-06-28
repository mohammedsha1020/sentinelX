# 🔨 SentinelX Build Instructions

## Prerequisites

### Required Software
1. **Visual Studio 2019 or later** (Community Edition is free)
   - Download from: https://visualstudio.microsoft.com/downloads/
   - Install with ".NET desktop development" workload

2. **Windows 10/11** with Administrator privileges

3. **Internet connection** for NuGet package downloads

## Step-by-Step Build Process

### 1. Open Visual Studio
- Launch Visual Studio
- Select "Open a project or solution"
- Navigate to your project folder and select `SentinelX.sln`

### 2. Restore NuGet Packages
- Right-click on the solution in Solution Explorer
- Select "Restore NuGet Packages"
- Wait for all packages to download

### 3. Build Configuration
- In Solution Explorer, right-click on "SentinelX" project
- Select "Properties"
- Ensure Target Framework is set to ".NET Framework 4.7.2"
- Set Configuration to "Release"
- Set Platform to "Any CPU"

### 4. Build the Solution
- Press `Ctrl+Shift+B` or go to Build → Build Solution
- Wait for build to complete
- Check Output window for any errors

### 5. Run as Administrator
- Right-click on "SentinelX" project in Solution Explorer
- Select "Properties"
- Go to "Debug" tab
- Check "Run as Administrator"
- Press F5 to run the application

## Expected Output

After successful build, you should find:
```
SentinelX\bin\Release\SentinelX.exe
```

## Troubleshooting

### Common Build Errors

**"Package restore failed"**
- Check internet connection
- Right-click solution → Restore NuGet Packages
- Clean solution and rebuild

**"Target framework not found"**
- Install .NET Framework 4.7.2 Developer Pack
- Download from: https://dotnet.microsoft.com/download/dotnet-framework/net472

**"SharpPcap not found"**
- Ensure NuGet packages are restored
- Check package references in .csproj file

**"Permission denied"**
- Run Visual Studio as Administrator
- Ensure Windows Firewall is enabled

### Manual Package Installation

If NuGet restore fails, manually install these packages:
1. Right-click project → Manage NuGet Packages
2. Search and install:
   - SharpPcap (6.2.6)
   - PacketDotNet (1.4.7)
   - System.Data.SQLite (1.0.118)
   - iTextSharp (5.5.13.3)
   - Newtonsoft.Json (13.0.3)

## Alternative Build Methods

### Using Command Line (if MSBuild is available)
```cmd
msbuild SentinelX.sln /p:Configuration=Release /p:Platform="Any CPU"
```

### Using Visual Studio Developer Command Prompt
1. Open "Developer Command Prompt for VS"
2. Navigate to project directory
3. Run: `msbuild SentinelX.sln /p:Configuration=Release`

## Post-Build Setup

### 1. Create Required Directories
Ensure these directories exist:
- `Data/` (for database and rules)
- `Resources/` (for alert sounds)
- `Reports/` (for generated reports)

### 2. Add Alert Sound
Replace `Resources/alert.wav` with an actual .wav file

### 3. Customize Rules
Edit rule files in `Data/rules/` directory

## Running the Application

### First Run
1. Right-click `SentinelX.exe` → "Run as administrator"
2. Login with: `admin` / `admin123`
3. Navigate through dashboard tabs

### Features to Test
- **Alerts Tab**: Click "Start Monitoring"
- **Network Traffic**: Click "Start Sniffing"
- **Firewall**: Try blocking an IP address
- **Proxy**: Configure proxy settings
- **Reports**: Generate a PDF report

## Security Notes

⚠️ **Important Security Considerations:**
- Run as Administrator for full functionality
- Use only on systems you own or have permission to monitor
- Be aware of local laws regarding network monitoring
- This tool is for educational and legitimate security purposes only

## Support

If you encounter build issues:
1. Check Visual Studio installation
2. Verify .NET Framework 4.7.2 is installed
3. Ensure all NuGet packages are restored
4. Run Visual Studio as Administrator
5. Check Windows Event Viewer for detailed error messages

---

**Happy Building! 🚀** 
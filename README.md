# 🔐 SentinelX - Windows Cybersecurity Monitoring System

A comprehensive SOC-style Windows desktop application built with C# WPF for real-time security monitoring, threat detection, and network analysis.

## 🎯 Features

### Core Security Features
- **Live Windows Event Log Monitoring** - Monitors Security and System logs in real-time
- **Rule-Based Threat Detection** - Customizable rules for detecting suspicious activities
- **Network Packet Sniffing** - Real-time network traffic analysis using SharpPcap
- **Automatic IP Blocking** - Blocks malicious IPs via Windows Firewall
- **OSINT IP Lookup** - Performs threat intelligence lookups on suspicious IPs
- **Anonymous Proxy Configuration** - Enables/disables proxy settings for anonymous browsing
- **Weekly Forensic Reports** - Generates PDF reports of security events

### GUI Features
- **Modern Dark Theme** - Professional cybersecurity-themed interface
- **Real-time Dashboard** - Live monitoring with multiple tabs
- **Alert System** - Visual and audio alerts for security events
- **Data Export** - Export alerts to CSV format
- **Settings Management** - Configurable monitoring options

## 🛠 Technology Stack

| Component | Technology |
|-----------|------------|
| Language | C# (.NET Framework 4.7.2) |
| GUI | WPF (Windows Presentation Foundation) |
| Packet Capture | SharpPcap + PacketDotNet |
| Database | SQLite |
| PDF Reports | iTextSharp |
| JSON Parsing | Newtonsoft.Json |

## 📦 Required NuGet Packages

```xml
<PackageReference Include="SharpPcap" Version="6.2.6" />
<PackageReference Include="PacketDotNet" Version="1.4.7" />
<PackageReference Include="System.Data.SQLite" Version="1.0.118" />
<PackageReference Include="iTextSharp" Version="5.5.13.3" />
<PackageReference Include="Newtonsoft.Json" Version="13.0.3" />
```

## 🚀 Build Instructions

### Prerequisites
- Visual Studio 2019 or later
- .NET Framework 4.7.2 or later
- Administrator privileges (for firewall and event log access)

### Build Steps
1. **Clone/Download** the project
2. **Open** `SentinelX.sln` in Visual Studio
3. **Restore NuGet packages** (right-click solution → Restore NuGet Packages)
4. **Build** the solution (Build → Build Solution)
5. **Run** as Administrator (right-click project → Properties → Debug → Start Options → Run as Administrator)

### Output
The compiled executable will be in:
```
SentinelX\bin\Release\SentinelX.exe
```

## 🔧 Configuration

### Default Login Credentials
- **Username**: `admin`
- **Password**: `admin123`

### Rule Files
Place custom rule files in `Data\rules/` directory:
```json
{
  "Name": "Custom Rule",
  "Keyword": "suspicious_keyword",
  "RegexPattern": ".*pattern.*",
  "Severity": "High",
  "Description": "Description of the rule"
}
```

### Alert Sound
Replace `Resources/alert.wav` with your preferred alert sound file.

## 📊 Usage Guide

### Starting the Application
1. Run `SentinelX.exe` as Administrator
2. Login with default credentials
3. Navigate through the dashboard tabs

### Monitoring Windows Logs
1. Go to **Alerts** tab
2. Click **"Start Monitoring"**
3. View real-time security events
4. Configure monitoring settings in **Settings** tab

### Network Traffic Analysis
1. Go to **Network Traffic** tab
2. Click **"Start Sniffing"**
3. View captured packets in real-time
4. Monitor for suspicious activities

### Firewall Management
1. Go to **Firewall** tab
2. Enter IP address to block/unblock
3. View list of blocked IPs
4. Manage firewall rules

### Proxy Configuration
1. Go to **Proxy Settings** tab
2. Enter proxy server and port
3. Click **"Enable Proxy"** or **"Disable Proxy"**

### Generating Reports
1. Click **"Generate Report"** in the header
2. PDF report will be saved in `Reports/` directory
3. Reports include all alerts and security events

## 🛡️ Security Features Explained

### Event Log Monitoring
- Monitors Windows Security and System event logs
- Detects failed login attempts (Event ID 4625)
- Identifies successful logins (Event ID 4624)
- Applies custom rules for threat detection

### Network Analysis
- Captures all network packets in promiscuous mode
- Detects port scanning activities
- Identifies connections to suspicious ports
- Monitors connection frequency patterns

### Firewall Integration
- Automatically blocks IPs based on alerts
- Uses Windows Firewall via `netsh` commands
- Maintains database of blocked IPs
- Supports manual IP blocking/unblocking

### Threat Intelligence
- Performs OSINT lookups on suspicious IPs
- Uses ip-api.com for geolocation and organization data
- Provides threat level assessment
- Integrates with alert system

## 📁 Project Structure

```
SentinelX/
├── Views/
│   ├── LoginWindow.xaml          # Login interface
│   └── Dashboard.xaml            # Main dashboard
├── Modules/
│   ├── LogMonitor.cs             # Event log monitoring
│   ├── FirewallManager.cs        # Firewall operations
│   ├── RuleEngine.cs             # Rule-based detection
│   ├── PacketSniffer.cs          # Network capture
│   ├── IPAnalyzer.cs             # OSINT lookups
│   ├── ProxyManager.cs           # Proxy configuration
│   └── ReportGenerator.cs        # PDF report generation
├── Models/
│   ├── Alert.cs                  # Alert data model
│   └── PacketInfo.cs             # Packet data model
├── Data/
│   ├── alerts.db                 # SQLite database
│   └── rules/                    # Rule files
├── Resources/
│   └── alert.wav                 # Alert sound
└── Reports/                      # Generated reports
```

## 🔍 Troubleshooting

### Common Issues

**"No network devices found"**
- Run as Administrator
- Install WinPcap or Npcap
- Check network adapter permissions

**"Database initialization failed"**
- Ensure write permissions to Data directory
- Check SQLite installation

**"Firewall operations fail"**
- Run as Administrator
- Ensure Windows Firewall is enabled
- Check netsh command availability

**"Event log access denied"**
- Run as Administrator
- Check Windows Event Log service
- Verify user permissions

### Performance Tips
- Limit packet capture to specific interfaces
- Adjust monitoring intervals in settings
- Use filters to reduce alert noise
- Regularly clean old alerts and packets

## 📝 License

This project is for educational and research purposes. Use responsibly and in accordance with local laws and regulations.

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Test thoroughly
5. Submit a pull request

## ⚠️ Disclaimer

This tool is designed for legitimate security monitoring and research purposes. Users are responsible for ensuring compliance with applicable laws and regulations. The authors are not responsible for any misuse of this software.

---

**Built with ❤️ for the cybersecurity community** 
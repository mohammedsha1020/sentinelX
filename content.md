

---

## 🔐 **Full Prompt for AI Assistant (SentinelX in C# WPF)**

---

> 💬 **Prompt for ChatGPT or any AI assistant:**

---

I want you to help me build a **complete Windows cybersecurity application** called **SentinelX**, using **C# (.NET Framework 4.7+) and WPF (Windows Presentation Foundation)**.

---

### 🎯 **Objective**:

Build a **SOC-style Windows desktop app** that:

* Monitors Windows event logs live (Security, System)
* Detects suspicious behavior via rule engine
* Sniffs network traffic in real-time
* Automatically blocks IPs via Windows Firewall
* Looks up suspicious IP info (OSINT)
* Allows anonymous proxy browsing (manual or Tor)
* Generates weekly forensic reports (PDF)
* Works 100% locally and compiles into a `.exe`

---

### ✅ **Project Stack**:

| Component       | Technology                    |
| --------------- | ----------------------------- |
| Language        | C# (.NET Framework 4.7+)      |
| GUI             | WPF                           |
| Packet Capture  | SharpPcap + PacketDotNet      |
| Logs Access     | System.Diagnostics            |
| Firewall Access | netsh via Process class       |
| Proxy Settings  | Windows Registry              |
| Database        | SQLite (`System.Data.SQLite`) |
| PDF Reports     | iTextSharp                    |
| Build System    | Visual Studio `.exe`          |

---

### 🧱 **Features to Build (with Implementation Details)**:

---

#### 1. **Login System**

* WPF LoginWindow\.xaml
* Optionally use password hashing (BCrypt/SHA256)
* On success → load Main Dashboard

---

#### 2. **Live Windows Event Log Monitor**

* Read `Security` and `System` logs
* Filter by `EventID` (e.g., 4625 = failed login)

```csharp
EventLog log = new EventLog("Security");
foreach (EventLogEntry entry in log.Entries)
{
    if (entry.InstanceId == 4625)
    {
        // Alert logic here
    }
}
```

---

#### 3. **Rule-Based Threat Engine**

* Load `.rule` files (INI/JSON format)
* Apply keyword/regex match on log messages
* If match → save alert to database

---

#### 4. **Firewall IP Blocking**

* Automatically block attacker IPs using `netsh`

```csharp
Process.Start("netsh", $"advfirewall firewall add rule name=\"BlockIP\" dir=in action=block remoteip={ip}");
```

* Maintain log of blocked IPs

---

#### 5. **Packet Sniffer**

* Use SharpPcap + PacketDotNet to sniff all packets

```csharp
CaptureDeviceList devices = CaptureDeviceList.Instance;
ICaptureDevice device = devices[0];
device.OnPacketArrival += new PacketArrivalEventHandler(OnPacket);
device.Open(DeviceMode.Promiscuous, 1000);
device.StartCapture();
```

* Parse and display IP, Port, Protocol in a real-time WPF DataGrid

---

#### 6. **OSINT IP Lookup**

* Query IP using `ip-api.com` or `abuseipdb.com`:

```csharp
var response = await client.GetStringAsync($"https://ip-api.com/json/{ip}");
```

* Show IP country, org, threat level

---

#### 7. **Proxy Configuration**

* Allow user to toggle proxy (manual or Tor)
* Update Windows Registry:

```csharp
Registry.SetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Internet Settings", "ProxyEnable", 1);
Registry.SetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Internet Settings", "ProxyServer", "127.0.0.1:9050");
```

---

#### 8. **Weekly Forensic Report**

* Generate PDF reports from logs using iTextSharp

```csharp
Document doc = new Document();
PdfWriter.GetInstance(doc, new FileStream("report.pdf", FileMode.Create));
doc.Open();
doc.Add(new Paragraph("SentinelX Weekly Report"));
doc.Close();
```

---

#### 9. **GUI Alert System**

* Show pop-ups or alerts inside Dashboard
* Use `System.Media.SoundPlayer` to play alert sounds

---

### 📁 **Project Structure**:

```plaintext
SentinelX/
├── Views/
│   ├── LoginWindow.xaml
│   └── Dashboard.xaml
├── Modules/
│   ├── LogMonitor.cs
│   ├── FirewallManager.cs
│   ├── RuleEngine.cs
│   ├── PacketSniffer.cs
│   ├── IPAnalyzer.cs
│   ├── ProxyManager.cs
│   └── ReportGenerator.cs
├── Models/
│   ├── Alert.cs
│   └── PacketInfo.cs
├── Data/
│   ├── alerts.db
│   └── rules/
│       ├── ssh.rule
│       └── portscan.rule
├── Resources/
│   └── alert.wav
├── App.xaml
├── MainWindow.xaml.cs
└── SentinelX.sln
```

---

### 📦 **NuGet Packages to Install**:

```plaintext
SharpPcap
PacketDotNet
System.Data.SQLite
iTextSharp
Newtonsoft.Json
```

---

### 🛠 **Build Instructions**:

1. Open `SentinelX.sln` in **Visual Studio**
2. Set project to `.NET Framework 4.7.2`
3. Build in **Release Mode**
4. Output: `.exe` file in `bin\Release`

---

Please generate all required files, classes, and GUI in a structured way following the folder layout, and provide complete C# code for each module with comments. You can break it into multiple parts if needed.

---

Let me know if you want help generating the [**GUI templates**](f), [**Log Monitor & Firewall Code**](f), or [**Sniffer Engine**](f) now! add in content file and start building
using System;
using System.Collections.Generic;
using System.Linq;
using PacketDotNet;
using SharpPcap;
using SentinelX.Models;

namespace SentinelX.Modules
{
    public class PacketSniffer
    {
        public event EventHandler<PacketInfo> PacketCaptured;
        public event EventHandler<string> NetworkScanDetected;
        private ICaptureDevice device;
        private bool sniffing = false;
        private Dictionary<string, int> connectionCounts = new Dictionary<string, int>();
        private Dictionary<string, DateTime> lastConnections = new Dictionary<string, DateTime>();
        private Dictionary<string, HashSet<int>> scanPorts = new Dictionary<string, HashSet<int>>();
        private Dictionary<string, DateTime> scanTimestamps = new Dictionary<string, DateTime>();
        private int selectedDeviceIndex = 0;

        public List<string> GetDeviceNames()
        {
            return CaptureDeviceList.Instance.Select((d, i) => $"[{i}] {d.Description}").ToList();
        }

        public void SetDeviceIndex(int index)
        {
            selectedDeviceIndex = index;
        }

        public void StartSniffing()
        {
            if (sniffing) return;
            sniffing = true;
            var devices = CaptureDeviceList.Instance;
            if (devices.Count < 1)
                throw new Exception("No network devices found. Please install Npcap/WinPcap and run as Administrator.");
            if (selectedDeviceIndex < 0 || selectedDeviceIndex >= devices.Count)
                selectedDeviceIndex = 0;
            device = devices[selectedDeviceIndex];
            device.OnPacketArrival += Device_OnPacketArrival;
            device.Open();
            device.StartCapture();
        }

        public void StopSniffing()
        {
            if (!sniffing) return;
            sniffing = false;
            if (device != null)
            {
                device.StopCapture();
                device.Close();
                device.OnPacketArrival -= Device_OnPacketArrival;
            }
        }

        private void Device_OnPacketArrival(object sender, PacketCapture e)
        {
            try
            {
                var rawPacket = e.GetPacket();
                var packet = Packet.ParsePacket(rawPacket.LinkLayerType, rawPacket.Data);
                var ipPacket = packet.Extract<IPPacket>();
                if (ipPacket != null)
                {
                    var tcp = packet.Extract<TcpPacket>();
                    var udp = packet.Extract<UdpPacket>();
                    var info = new PacketInfo
                    {
                        SourceIP = ipPacket.SourceAddress.ToString(),
                        DestinationIP = ipPacket.DestinationAddress.ToString(),
                        Protocol = ipPacket.Protocol.ToString(),
                        PacketSize = ipPacket.TotalLength
                    };
                    if (tcp != null)
                    {
                        info.SourcePort = tcp.SourcePort;
                        info.DestinationPort = tcp.DestinationPort;
                        info.Flags = GetTcpFlags(tcp);
                    }
                    else if (udp != null)
                    {
                        info.SourcePort = udp.SourcePort;
                        info.DestinationPort = udp.DestinationPort;
                    }

                    // Check for suspicious activities
                    CheckForSuspiciousActivity(info);
                    PacketCaptured?.Invoke(this, info);
                }
            }
            catch { }
        }

        private string GetTcpFlags(TcpPacket tcp)
        {
            var flags = new List<string>();
            if (tcp.Synchronize) flags.Add("SYN");
            if (tcp.Acknowledgment) flags.Add("ACK");
            if (tcp.Finished) flags.Add("FIN");
            if (tcp.Push) flags.Add("PSH");
            if (tcp.Reset) flags.Add("RST");
            if (tcp.Urgent) flags.Add("URG");
            return string.Join(",", flags);
        }

        private void CheckForSuspiciousActivity(PacketInfo info)
        {
            // Check for port scanning
            string key = $"{info.SourceIP}_{info.DestinationIP}";
            if (connectionCounts.ContainsKey(key))
            {
                connectionCounts[key]++;
                if (connectionCounts[key] > 10 && (DateTime.Now - lastConnections[key]).TotalMinutes < 1)
                {
                    info.IsSuspicious = true;
                    info.SuspicionReason = "Potential port scanning detected";
                }
            }
            else
            {
                connectionCounts[key] = 1;
                lastConnections[key] = DateTime.Now;
            }

            // Port scan detection: >10 unique destination ports from same source IP in 5 seconds
            if (!scanPorts.ContainsKey(info.SourceIP))
            {
                scanPorts[info.SourceIP] = new HashSet<int>();
                scanTimestamps[info.SourceIP] = DateTime.Now;
            }
            scanPorts[info.SourceIP].Add(info.DestinationPort);
            if ((DateTime.Now - scanTimestamps[info.SourceIP]).TotalSeconds > 5)
            {
                scanPorts[info.SourceIP].Clear();
                scanTimestamps[info.SourceIP] = DateTime.Now;
            }
            if (scanPorts[info.SourceIP].Count > 10)
            {
                NetworkScanDetected?.Invoke(this, $"Network scan detected from {info.SourceIP} (more than 10 ports in 5 seconds)");
                scanPorts[info.SourceIP].Clear();
                scanTimestamps[info.SourceIP] = DateTime.Now;
            }

            // Check for suspicious ports
            var suspiciousPorts = new[] { 22, 23, 3389, 445, 135, 139 };
            if (suspiciousPorts.Contains(info.DestinationPort) || suspiciousPorts.Contains(info.SourcePort))
            {
                info.IsSuspicious = true;
                info.SuspicionReason = $"Connection to suspicious port: {info.DestinationPort}";
            }

            // Clean up old entries
            var oldKeys = lastConnections.Where(kvp => (DateTime.Now - kvp.Value).TotalMinutes > 5).Select(kvp => kvp.Key).ToList();
            foreach (var oldKey in oldKeys)
            {
                lastConnections.Remove(oldKey);
                connectionCounts.Remove(oldKey);
            }
        }
    }
} 
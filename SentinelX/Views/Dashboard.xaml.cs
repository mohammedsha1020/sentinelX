using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using SentinelX.Models;
using SentinelX.Modules;
using LiveCharts;
using LiveCharts.Wpf;
using System.Windows.Media;
using Microsoft.Win32;
using System.Windows.Controls;
using System.Windows.Data;
using SharpPcap;
using System.Collections.Generic;

namespace SentinelX.Views
{
    public partial class Dashboard : Window
    {
        private PacketSniffer packetSniffer;
        
        public SeriesCollection AlertSeries { get; set; }
        public SeriesCollection TrafficSeries { get; set; }
        public ObservableCollection<PacketInfo> NetworkPackets { get; set; }
        public ICollectionView FilteredNetworkPackets { get; set; }
        private bool isSniffing = false;
        private ObservableCollection<Alert> Alerts { get; set; } = new ObservableCollection<Alert>();
        private DispatcherTimer trafficTimer;
        private Queue<int> packetsPerSecond = new Queue<int>();
        private Queue<long> bytesPerSecond = new Queue<long>();
        private int currentPacketCount = 0;
        private long currentByteCount = 0;
        private const int TrafficGraphWindow = 30; // seconds
        private UserControl alertsPage;
        private UIElement dashboardContent;

        public Dashboard()
        {
            InitializeComponent();
            DataContext = this;
            // Sample data for AlertSeries
            AlertSeries = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Packets/sec",
                    Values = new ChartValues<int>(),
                    PointGeometry = null
                }
            };
            // Sample data for TrafficSeries
            TrafficSeries = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Bytes/sec",
                    Values = new ChartValues<long>(),
                    PointGeometry = null
                }
            };

            for (int i = 0; i < TrafficGraphWindow; i++)
            {
                TrafficSeries[0].Values.Add(0L);
                AlertSeries[0].Values.Add(0);
            }

            NetworkPackets = new ObservableCollection<PacketInfo>();
            FilteredNetworkPackets = CollectionViewSource.GetDefaultView(NetworkPackets);
            packetSniffer = new PacketSniffer();
            packetSniffer.PacketCaptured += OnPacketCaptured;
            packetSniffer.StartSniffing();
            packetSniffer.NetworkScanDetected += OnNetworkScanDetected;
            TrafficFilterBox.TextChanged += TrafficFilterBox_TextChanged;
            dashboardContent = (UIElement)MainContentArea.Content;
            trafficTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            trafficTimer.Tick += TrafficTimer_Tick;
            trafficTimer.Start();
            alertsPage = new AlertsPage();
            ((AlertsPage)alertsPage).AlertsGrid.ItemsSource = Alerts;
        }
        
        protected override void OnClosing(CancelEventArgs e)
        {
            // Clean up any resources if needed for the new dashboard
            base.OnClosing(e);
        }

        private void OnPacketCaptured(object sender, PacketInfo packet)
        {
            Dispatcher.Invoke(() =>
            {
                NetworkPackets.Insert(0, packet);
                if (NetworkPackets.Count > 500) NetworkPackets.RemoveAt(NetworkPackets.Count - 1);
                currentPacketCount++;
                currentByteCount += packet.PacketSize;
                if (packet.IsSuspicious)
                {
                    Alerts.Insert(0, new Alert { Timestamp = packet.Timestamp, Type = "Suspicious", Message = packet.SuspicionReason, SourceIP = packet.SourceIP, Severity = "High" });
                }
            });
        }

        private void TrafficFilterBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string filter = TrafficFilterBox.Text.Trim().ToLower();
            if (string.IsNullOrEmpty(filter))
            {
                FilteredNetworkPackets.Filter = null;
            }
            else
            {
                FilteredNetworkPackets.Filter = obj =>
                {
                    var pkt = obj as PacketInfo;
                    return pkt != null && (
                        pkt.SourceIP.ToLower().Contains(filter) ||
                        pkt.DestinationIP.ToLower().Contains(filter) ||
                        pkt.Protocol.ToLower().Contains(filter) ||
                        pkt.SourcePort.ToString().Contains(filter) ||
                        pkt.DestinationPort.ToString().Contains(filter)
                    );
                };
            }
            FilteredNetworkPackets.Refresh();
        }

        private void CopyTrafficRow_Click(object sender, RoutedEventArgs e)
        {
            if (NetworkTrafficGrid.SelectedItem is PacketInfo pkt)
            {
                Clipboard.SetText($"{pkt.Timestamp},{pkt.SourceIP},{pkt.SourcePort},{pkt.DestinationIP},{pkt.DestinationPort},{pkt.Protocol},{pkt.PacketSize},{pkt.Flags},{pkt.IsSuspicious},{pkt.SuspicionReason}");
            }
        }

        private void ExportTraffic_Click(object sender, RoutedEventArgs e)
        {
            var saveDialog = new SaveFileDialog
            {
                Filter = "CSV files (*.csv)|*.csv",
                FileName = $"network_traffic_{System.DateTime.Now:yyyyMMdd_HHmmss}.csv"
            };
            if (saveDialog.ShowDialog() == true)
            {
                using (var writer = new StreamWriter(saveDialog.FileName))
                {
                    writer.WriteLine("Timestamp,SourceIP,SourcePort,DestinationIP,DestinationPort,Protocol,PacketSize,Flags,Suspicious,Reason");
                    foreach (var pkt in NetworkPackets)
                    {
                        writer.WriteLine($"{pkt.Timestamp},{pkt.SourceIP},{pkt.SourcePort},{pkt.DestinationIP},{pkt.DestinationPort},{pkt.Protocol},{pkt.PacketSize},{pkt.Flags},{pkt.IsSuspicious},{pkt.SuspicionReason}");
                    }
                }
                MessageBox.Show("Network traffic exported successfully.", "Export", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void ExportTrafficRow_Click(object sender, RoutedEventArgs e)
        {
            // Optional: Implement per-row export if needed, or leave as a stub to resolve the error
        }

        private void OnNetworkScanDetected(object sender, string message)
        {
            Dispatcher.Invoke(() =>
            {
                MessageBox.Show(message, "Network Scan Alert", MessageBoxButton.OK, MessageBoxImage.Warning);
                Alerts.Insert(0, new Alert { Timestamp = DateTime.Now, Type = "Scan", Message = message, SourceIP = "", Severity = "Critical" });
            });
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Populate network interfaces
            var devices = CaptureDeviceList.Instance;
            InterfaceComboBox.Items.Clear();
            foreach (var dev in devices)
            {
                InterfaceComboBox.Items.Add(dev.Description);
            }
            if (devices.Count > 0)
            {
                InterfaceComboBox.SelectedIndex = 0;
            }
            else
            {
                MessageBox.Show("No network interfaces found. Please install Npcap and run as Administrator.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void InterfaceComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (InterfaceComboBox.SelectedIndex >= 0)
            {
                if (isSniffing)
                {
                    packetSniffer.StopSniffing();
                }
                packetSniffer.SetDeviceIndex(InterfaceComboBox.SelectedIndex);
                packetSniffer.StartSniffing();
                isSniffing = true;
            }
        }

        private void TrafficTimer_Tick(object sender, EventArgs e)
        {
            // Remove the oldest point
            TrafficSeries[0].Values.RemoveAt(0);
            AlertSeries[0].Values.RemoveAt(0);

            // Add the new point
            TrafficSeries[0].Values.Add(currentByteCount);
            AlertSeries[0].Values.Add(currentPacketCount);

            currentPacketCount = 0;
            currentByteCount = 0;
        }

        private void btnAlerts_Click(object sender, RoutedEventArgs e)
        {
            MainContentArea.Content = alertsPage;
        }
        
        private void btnDashboard_Click(object sender, RoutedEventArgs e)
        {
            MainContentArea.Content = dashboardContent;
        }

        public void BackToDashboard()
        {
            MainContentArea.Content = dashboardContent;
        }

        private void btnReports_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Reports page not yet implemented.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Settings page not yet implemented.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
} 
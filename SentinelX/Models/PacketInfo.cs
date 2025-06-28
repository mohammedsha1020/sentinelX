using System;

namespace SentinelX.Models
{
    public class PacketInfo
    {
        public DateTime Timestamp { get; set; }
        public string SourceIP { get; set; }
        public string DestinationIP { get; set; }
        public int SourcePort { get; set; }
        public int DestinationPort { get; set; }
        public string Protocol { get; set; }
        public int PacketSize { get; set; }
        public string Flags { get; set; }
        public bool IsSuspicious { get; set; }
        public string SuspicionReason { get; set; }

        public PacketInfo()
        {
            Timestamp = DateTime.Now;
            IsSuspicious = false;
        }
    }
} 
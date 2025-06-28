using System;

namespace SentinelX.Models
{
    public class Alert
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public string Severity { get; set; }
        public string Source { get; set; }
        public string Message { get; set; }
        public string IPAddress { get; set; }
        public int? EventId { get; set; }
        public bool Processed { get; set; }
        public string Type { get; set; }
        public string SourceIP { get; set; }
        
        public Alert()
        {
            Timestamp = DateTime.Now;
            Processed = false;
        }
        
        public Alert(string severity, string source, string message, string ipAddress = null, int? eventId = null)
        {
            Timestamp = DateTime.Now;
            Severity = severity;
            Source = source;
            Message = message;
            IPAddress = ipAddress;
            EventId = eventId;
            Processed = false;
        }
        
        public override string ToString()
        {
            return $"[{Timestamp:yyyy-MM-dd HH:mm:ss}] {Severity} - {Source}: {Message}";
        }
    }
    
    public enum AlertSeverity
    {
        Low,
        Medium,
        High,
        Critical
    }
} 
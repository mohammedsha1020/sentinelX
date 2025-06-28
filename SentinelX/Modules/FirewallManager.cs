using System;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;

namespace SentinelX.Modules
{
    public class FirewallManager
    {
        private string dbPath;
        public FirewallManager()
        {
            dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "alerts.db");
        }

        public void BlockIP(string ip, string reason)
        {
            string ruleName = $"BlockIP_{ip}";
            string args = $"advfirewall firewall add rule name=\"{ruleName}\" dir=in action=block remoteip={ip}";
            Process.Start(new ProcessStartInfo
            {
                FileName = "netsh",
                Arguments = args,
                CreateNoWindow = true,
                UseShellExecute = false
            });
            LogBlockedIP(ip, reason, "SentinelX");
        }

        public void UnblockIP(string ip)
        {
            string ruleName = $"BlockIP_{ip}";
            string args = $"advfirewall firewall delete rule name=\"{ruleName}\" remoteip={ip}";
            Process.Start(new ProcessStartInfo
            {
                FileName = "netsh",
                Arguments = args,
                CreateNoWindow = true,
                UseShellExecute = false
            });
            RemoveBlockedIP(ip);
        }

        private void LogBlockedIP(string ip, string reason, string blockedBy)
        {
            try
            {
                string connectionString = $"Data Source={dbPath};Version=3;";
                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string sql = "INSERT OR IGNORE INTO BlockedIPs (IPAddress, Reason, BlockedBy) VALUES (@ip, @reason, @blockedBy)";
                    using (var command = new SQLiteCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@ip", ip);
                        command.Parameters.AddWithValue("@reason", reason);
                        command.Parameters.AddWithValue("@blockedBy", blockedBy);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch { }
        }

        private void RemoveBlockedIP(string ip)
        {
            try
            {
                string connectionString = $"Data Source={dbPath};Version=3;";
                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string sql = "DELETE FROM BlockedIPs WHERE IPAddress = @ip";
                    using (var command = new SQLiteCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@ip", ip);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch { }
        }
    }
} 
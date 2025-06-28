using System;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using SentinelX.Models;

namespace SentinelX.Modules
{
    public class LogMonitor
    {
        public event EventHandler<Alert> AlertGenerated;
        private EventLog securityLog;
        private EventLog systemLog;
        private CancellationTokenSource cts;
        private bool monitoring;
        private RuleEngine ruleEngine;
        private string dbPath;

        public LogMonitor()
        {
            cts = new CancellationTokenSource();
            ruleEngine = new RuleEngine();
            dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "alerts.db");
        }

        public void StartMonitoring()
        {
            if (monitoring) return;
            monitoring = true;
            cts = new CancellationTokenSource();
            Task.Run(() => MonitorLogs(cts.Token));
        }

        public void StopMonitoring()
        {
            monitoring = false;
            cts.Cancel();
        }

        private void MonitorLogs(CancellationToken token)
        {
            try
            {
                securityLog = new EventLog("Security");
                systemLog = new EventLog("System");
                securityLog.EntryWritten += SecurityLog_EntryWritten;
                systemLog.EntryWritten += SystemLog_EntryWritten;
                securityLog.EnableRaisingEvents = true;
                systemLog.EnableRaisingEvents = true;
                while (!token.IsCancellationRequested)
                {
                    Thread.Sleep(1000);
                }
                securityLog.EnableRaisingEvents = false;
                systemLog.EnableRaisingEvents = false;
            }
            catch (Exception)
            {
                // Optionally log error
            }
        }

        private void SecurityLog_EntryWritten(object sender, EntryWrittenEventArgs e)
        {
            ProcessLogEntry(e.Entry, "SecurityLog");
        }

        private void SystemLog_EntryWritten(object sender, EntryWrittenEventArgs e)
        {
            ProcessLogEntry(e.Entry, "SystemLog");
        }

        private void ProcessLogEntry(EventLogEntry entry, string source)
        {
            try
            {
                // Check against rule engine
                if (ruleEngine.CheckMessage(entry.Message, out var matchedRule))
                {
                    var alert = new Alert
                    {
                        Severity = matchedRule.Severity,
                        Source = source,
                        Message = $"{matchedRule.Name}: {entry.Message}",
                        EventId = (int)entry.InstanceId
                    };
                    SaveAlertToDatabase(alert);
                    AlertGenerated?.Invoke(this, alert);
                }
                else
                {
                    // Check for specific event IDs
                    if (entry.InstanceId == 4625) // Failed login
                    {
                        var alert = new Alert
                        {
                            Severity = "High",
                            Source = source,
                            Message = $"Failed login attempt: {entry.Message}",
                            EventId = (int)entry.InstanceId
                        };
                        SaveAlertToDatabase(alert);
                        AlertGenerated?.Invoke(this, alert);
                    }
                    else if (entry.InstanceId == 4624) // Successful login
                    {
                        var alert = new Alert
                        {
                            Severity = "Info",
                            Source = source,
                            Message = $"Successful login: {entry.Message}",
                            EventId = (int)entry.InstanceId
                        };
                        SaveAlertToDatabase(alert);
                        AlertGenerated?.Invoke(this, alert);
                    }
                }
            }
            catch (Exception)
            {
                // Silently handle errors
            }
        }

        private void SaveAlertToDatabase(Alert alert)
        {
            try
            {
                string connectionString = $"Data Source={dbPath};Version=3;";
                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string sql = @"INSERT INTO Alerts (Timestamp, Severity, Source, Message, IPAddress, EventId, Processed) 
                                  VALUES (@timestamp, @severity, @source, @message, @ipaddress, @eventid, @processed)";
                    using (var command = new SQLiteCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@timestamp", alert.Timestamp);
                        command.Parameters.AddWithValue("@severity", alert.Severity);
                        command.Parameters.AddWithValue("@source", alert.Source);
                        command.Parameters.AddWithValue("@message", alert.Message);
                        command.Parameters.AddWithValue("@ipaddress", alert.IPAddress ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@eventid", alert.EventId ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@processed", alert.Processed);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception)
            {
                // Silently handle database errors
            }
        }
    }
} 
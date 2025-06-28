using System;
using System.Windows;
using System.Data.SQLite;
using System.IO;

namespace SentinelX
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            
            // Initialize database
            InitializeDatabase();
            
            // Set up global exception handling
            Current.DispatcherUnhandledException += OnDispatcherUnhandledException;
        }
        
        private void InitializeDatabase()
        {
            try
            {
                string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "alerts.db");
                string dbDirectory = Path.GetDirectoryName(dbPath);
                
                if (!Directory.Exists(dbDirectory))
                {
                    Directory.CreateDirectory(dbDirectory);
                }
                
                string connectionString = $"Data Source={dbPath};Version=3;";
                
                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    
                    // Create alerts table
                    string createTableSql = @"
                        CREATE TABLE IF NOT EXISTS Alerts (
                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                            Timestamp DATETIME DEFAULT CURRENT_TIMESTAMP,
                            Severity TEXT NOT NULL,
                            Source TEXT NOT NULL,
                            Message TEXT NOT NULL,
                            IPAddress TEXT,
                            EventId INTEGER,
                            Processed INTEGER DEFAULT 0
                        )";
                    
                    using (var command = new SQLiteCommand(createTableSql, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                    
                    // Create blocked IPs table
                    string createBlockedIPsSql = @"
                        CREATE TABLE IF NOT EXISTS BlockedIPs (
                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                            IPAddress TEXT NOT NULL UNIQUE,
                            Timestamp DATETIME DEFAULT CURRENT_TIMESTAMP,
                            Reason TEXT,
                            BlockedBy TEXT
                        )";
                    
                    using (var command = new SQLiteCommand(createBlockedIPsSql, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Database initialization failed: {ex.Message}", "Error", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
        private void OnDispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show($"An unexpected error occurred: {e.Exception.Message}", 
                "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            e.Handled = true;
        }
    }
} 
using System;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace SentinelX.Views
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            txtUsername.Focus();
            
            // Handle Enter key
            txtPassword.KeyDown += (s, e) =>
            {
                if (e.Key == System.Windows.Input.Key.Enter)
                {
                    btnLogin_Click(s, e);
                }
            };
        }
        
        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Password;
            
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                txtStatus.Text = "Please enter both username and password.";
                return;
            }
            
            // Simple authentication (in production, use proper authentication)
            if (AuthenticateUser(username, password))
            {
                txtStatus.Text = "Login successful! Loading dashboard...";
                btnLogin.IsEnabled = false;
                
                // Open dashboard
                var dashboard = new Dashboard();
                dashboard.Show();
                this.Close();
            }
            else
            {
                txtStatus.Text = "Invalid username or password.";
                txtPassword.Password = "";
                txtPassword.Focus();
            }
        }
        
        private bool AuthenticateUser(string username, string password)
        {
            // Default credentials (in production, use database or AD authentication)
            if (username.ToLower() == "admin" && password == "admin123")
            {
                return true;
            }
            
            // You can add more users here or implement proper authentication
            return false;
        }
        
        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }
    }
} 
using System.Windows;
using System.Windows.Controls;

namespace SentinelX.Views
{
    public partial class AlertsPage : UserControl
    {
        public AlertsPage()
        {
            InitializeComponent();
        }

        private void BackToDashboard_Click(object sender, RoutedEventArgs e)
        {
            if (Window.GetWindow(this) is Dashboard dashboard)
            {
                dashboard.BackToDashboard();
            }
        }
    }
} 
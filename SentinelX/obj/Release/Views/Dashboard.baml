   M S B A M L   `   `   `  ���� U  QLiveCharts.Wpf, Version=0.9.7.0, Culture=neutral, PublicKeyToken=0bc1f845d1ebb8dfG4clr-namespace:LiveCharts.Wpf;assembly=LiveCharts.WpfLiveCharts.Wpf   	SentinelX   SentinelX.Views.Dashboard   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Dlvc4clr-namespace:LiveCharts.Wpf;assembly=LiveCharts.Wpf     -     � Title$  SentinelX - Dashboard��$	��800��$
��1300��   PrimaryBackgroundBrush#��[   � Icon$ ../Resources/logo.png��.���  � ColumnDefinitions �� $	��220 ��� $��* �.+��� $��0��  SidebarBackgroundBrush#��[ $	��220��$��Stretch=�.+���  �� Orientation$ 
Horizontal=�$��
18,32,0,24q�$	��Top=�.+�A� $��36��$��36��
��������� $��
 SENTINELX��$��20(�$
��Bold"�  SidebarTextBrush#��[ $��Center=�$��8,0,0,0q��� -    ��Name$ 	btnAlerts��$��  Alerts��  SidebarButton#��[ �� -   $ btnDashboard��$��  Dashboard��#��[ �� -   $ 
btnReports��$��	  Reports��#��[ �� -   $ btnSettings��$��
  Settings��#��[ �� -   $ 	btnLogout��$��  Logout��#��[ � $��1��#��[   � RowDefinitions �� $
��Auto ��� $��* �.+�]� $��0��$��0,0,0,18q�$��56��$	��Top=�.+��� $��	Dashboard��$��26(�$
��Bold"�  	TextBrush#��[ $��Center=�$
��Left=�$��18,0,0,0q��� $ 
Horizontal=�$��Right=�$��Right=�$��0,0,18,0q�.+��� $	��180��$��32��$��0,0,12,0q�$��Center=����#F1F3F7���#E5E7EB���#232A3D$��14(�$	��8,0q��� $��Settings��$��32��  AccentButton#��[ $��0,0,12,0q�A� $��36��$��36�����#E5E7EB�� -    ��Name$ MainContentArea��$��1��$��
18,0,18,18q�.���� .+���   Card#��[ ._��� .+��� $��Live Alert Feed��$��18(�$��SemiBold"�#��[ $��0,0,0,12q��  �� $	��180 ��� $��* ��� $	��180 �.+��� $��0��$	��Top=�.+��� $��4.70%��$��32(�$
��Bold"����#3B82F6�� $��Neofony��$��14(����#A0AEC0�� $��Else Detected��$��12(����#A0AEC0�� $��INERCOAN��#��[ $��32��$��0,12,0,0q��� $��1�����#F7F8FA �� CornerRadius$ 8��$	��100��$
��18,0q����#E5E7EB$��1q�._�#   LiveCharts.Wpf.CartesianChart  -     Name$ 
alertChart��&    LiveCharts.Wpf.Charts.Base.Chart	   LegendLocation$	 Right=�
   Series
 �� *AlertSeries+�� $��2��$	��Top=�.+��� $��	Deal Time��$��14(����#A0AEC0�� $��	🕒 8:12��$��16(�$
��Bold"����#3B82F6�� $��Count on Date��$��12(����#A0AEC0�  �� $��2* ��� $��* �.+��� #��[ $��0��$��0,0,18,0q�._��� .+��� $��Live Traffic��$��16(�$��SemiBold"�#��[ $��0,0,0,12q��� ���#F7F8FA$ 8��$	��120�����#E5E7EB$��1q�._�  -	   $ trafficChart��$	 Right=�
 �� *TrafficSeries+�� #��[ $��1��._��� .+��� $��Proxy Settings��$��16(�$��SemiBold"�#��[ $��0,0,0,12q��� $	��120��$��0q�
������ .@��� $��Ctheon Nay   10.730���� $��Ticeer Nptel   0.330���� $��Retwork Npices   0.430���� $��Ceciorinay   0.390���� $��Depey Deets   0.330���� $��Salve Solnicic   0.430���� #��[ $��0,18,0,0q�._��� .+��� $��Live Network Traffic��$��16(�$��SemiBold"�#��[ $��0,0,0,12q��� $��Network Interface:��$��0,0,0,4q��� -
    ��Name$ InterfaceComboBox��$	��320��$��28��$��0,0,0,8q��� -    ��Name$ TrafficFilterBox��$	��220��$��28��$��0,0,0,8q�&   System.Windows.Controls.DataGrid  -     Name$ NetworkTrafficGrid��   AutoGenerateColumns .  $	��220��
����������#232A3D   HeadersVisibility$ Column=�$��0q�   RowBackground �#F7F8FA   AlternatingRowBackground �#E5E7EB   GridLinesVisibility$
 None=����� *FilteredNetworkPackets+ � 	Resources )  #System.Windows.Controls.DataGridRow �� 
TargetType �� Triggers �� Binding �� Value �� Property �� Value��%}   '        �� "   ��  �� *IsSuspicious+ True.a���  � �� �	#FFF8D7DA��  � �� �#B71C1C � ContextMenu �� .@�m� -   $��Copy Row��m� -   $��Export Row to CSV��   Columns 0  *System.Windows.Controls.DataGridTextColumn  ,  &System.Windows.Controls.DataGridColumn   Header$ 	Timestamp��   Width5  /System.Windows.Controls.DataGridLengthConverter$	 120 1  +System.Windows.Controls.DataGridBoundColumn   Binding �� *	Timestamp+  $ 	Source IP��$	 120  �� *
SourceIP+  $ Source Port��$ 80  �� *
SourcePort+  $ Destination IP��$	 120  �� *DestinationIP+  $ Destination Port��$ 80  �� *DestinationPort+  $ Protocol��$ 80  �� *
Protocol+  $ Packet Size��$ 80  �� *
PacketSize+  $ Flags��$	 100  �� *Flags+  $ 
Suspicious��$ 80  �� *IsSuspicious+  $ Reason��$	 200  �� *SuspicionReason+�� -   $��Export Traffic��#��[ $��32��$	��160��$��0,12,0,0q�
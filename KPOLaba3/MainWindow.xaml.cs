using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Color = System.Drawing.Color;
using Image = System.Drawing.Image;
using Pen = System.Drawing.Pen;
using WindowsImage = System.Windows.Controls.Image;

namespace KPOLaba3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly DispatcherTimer _dispatcherTimer;

        public MainWindow()
        {
            InitializeComponent();
            NowClock.ClockSpan = TimeZoneInfo.Local.BaseUtcOffset;
            MoscowClock.ClockSpan = TimeSpan.FromHours(3);
            ParisClock.ClockSpan = TimeSpan.FromHours(2);
            NewYorkClock.ClockSpan = TimeSpan.FromHours(-4);
        }

        private void OnTabSelected(object sender, EventArgs e)
        {
            if (VisualTreeHelper.GetChildrenCount((TabItem)sender) != 0)
            {
                var child = (Grid) VisualTreeHelper.GetChild((TabItem) sender, 0);
                // TODO
                //Clock.RenderClock(child);
            }
        }
    }
}

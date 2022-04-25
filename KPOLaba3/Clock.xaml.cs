using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace KPOLaba3
{
    /// <summary>
    /// Interaction logic for Clock.xaml
    /// </summary>
    public partial class Clock : UserControl
    {
        private readonly DispatcherTimer _dispatcherTimer;
        private static Dictionary<Grid, Image> _images = new Dictionary<Grid, Image>();

        public Clock()
        {
            InitializeComponent();
            _images.Add(NowGrid,NowClock);
            NowClock.Tag = this;
            ClockSpan = new TimeSpan();
            _dispatcherTimer = new DispatcherTimer();
            _dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            _dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            this.DataContext = this;
            _dispatcherTimer.Start();
        }

        public delegate void OnTabSelectedDelegate(object sender, EventArgs e);

        public OnTabSelectedDelegate OnTabSelectedEvent { get; set; }

        public TimeSpan ClockSpan { get; set; }

        private void OnTabSelected(object sender, EventArgs e)
        {
            OnTabSelectedEvent(sender, e);
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (!(double.IsNaN(NowGrid.ActualWidth) | double.IsNaN(NowGrid.ActualHeight)))
            {
                ClockGenerator.MainBitmapRender((NowGrid.ActualWidth, NowGrid.ActualHeight));
                dispatcherTimer_Tick(sender, EventArgs.Empty);
            }
            else
            {
            }
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (this.IsVisible)
            {
                RenderClock(NowClock);
            }
        }

        private static void RenderClock(Image image)
        {
            image.Source = ClockGenerator.ImageSourceFromBitmap(ClockGenerator.TimeRender(DateTime.UtcNow + ((Clock)image.Tag).ClockSpan));
        }

        public static void RenderClock(Grid grid)
        {
            RenderClock(_images[grid]);
        }

        private void NowGrid_OnIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            RenderClock(NowGrid);
        }
    }
}

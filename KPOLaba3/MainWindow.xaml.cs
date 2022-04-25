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
using Pen = System.Drawing.Pen;

namespace KPOLaba3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly DispatcherTimer _dispatcherTimer;

        private Bitmap? _mainBitmap;
        private double _minSide;
        private double factor = 1;
        private (double ActualWidth, double ActualHeight) Grid;

        private const double _divisionWidth = 0.02;
        private readonly Color _divisionColor = Color.Aqua;
        private const double _divisionPointR = 0.75;
        private const double _divisionHeight = 0.1;

        private const double _smallDivisionWidth = 0.01;
        private readonly Color _smallDivisionColor = Color.Aquamarine;
        private const double _smallDivisionPointR = _divisionPointR + _divisionHeight - _smallDivisionHeight;
        private const double _smallDivisionHeight = 0.05;

        private const double _hourHandWidth = 0.02;
        private readonly Color _hourHandColor = Color.Black;
        private const double _hourHandStartR = -0.1;
        private const double _hourHandEndR = 0.4;

        private const double _minuteHandWidth = 0.02;
        private readonly Color _minuteHandColor = Color.Black;
        private const double _minuteHandStartR = -0.15;
        private const double _minuteHandEndR = 0.6;

        private const double _secondHandWidth = 0.02;
        private readonly Color _secondHandColor = Color.Red;
        private const double _secondHandStartR = -0.2;
        private const double _secondHandEndR = 0.65;

        private void MainBitmapRender()
        {
            _mainBitmap = new Bitmap(Convert.ToInt32(Grid.ActualWidth), Convert.ToInt32(Grid.ActualHeight));
            _minSide = Grid.ActualWidth > Grid.ActualHeight ? Grid.ActualHeight : Grid.ActualWidth;
            Graphics graphics = Graphics.FromImage(_mainBitmap);
            var divisionPen = new Pen(_divisionColor, Convert.ToSingle(_minSide * _divisionWidth));
            for (int i = 0; i < 12; i++)
            {
                graphics.DrawLine(divisionPen,
                    Convert.ToSingle(Grid.ActualWidth / 2 + Math.Cos(Math.PI * i / 6) * (_minSide / 2 * _divisionPointR - _minSide / 2 * _divisionHeight)),
                    Convert.ToSingle(Grid.ActualHeight / 2 + Math.Sin(Math.PI * i / 6) * (_minSide / 2 * _divisionPointR - _minSide / 2 * _divisionHeight)),
                    Convert.ToSingle(Grid.ActualWidth / 2 + Math.Cos(Math.PI * i / 6) * (_minSide / 2 * _divisionPointR + _minSide / 2 * _divisionHeight)),
                    Convert.ToSingle(Grid.ActualHeight / 2 + Math.Sin(Math.PI * i / 6) * (_minSide / 2 * _divisionPointR + _minSide / 2 * _divisionHeight)));
            }

            var smallDivisionPen = new Pen(_smallDivisionColor, Convert.ToSingle(_minSide * _smallDivisionWidth));
            for (int i = 0; i < 60; i++)
            {
                if (i % 5 == 0) continue;
                graphics.DrawLine(smallDivisionPen,
                    Convert.ToSingle(Grid.ActualWidth / 2 + Math.Cos(Math.PI * i / 30) * (_minSide / 2 * _smallDivisionPointR - _minSide / 2 * _smallDivisionHeight)),
                    Convert.ToSingle(Grid.ActualHeight / 2 + Math.Sin(Math.PI * i / 30) * (_minSide / 2 * _smallDivisionPointR - _minSide / 2 * _smallDivisionHeight)),
                    Convert.ToSingle(Grid.ActualWidth / 2 + Math.Cos(Math.PI * i / 30) * (_minSide / 2 * _smallDivisionPointR + _minSide / 2 * _smallDivisionHeight)),
                    Convert.ToSingle(Grid.ActualHeight / 2 + Math.Sin(Math.PI * i / 30) * (_minSide / 2 * _smallDivisionPointR + _minSide / 2 * _smallDivisionHeight)));
            }
        }
        public MainWindow()
        {
            InitializeComponent();
            _dispatcherTimer = new DispatcherTimer();
            _dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            _dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
        }

        private BitmapImage BitmapConvert(Bitmap src)
        {
            MemoryStream ms = new MemoryStream();
            ((System.Drawing.Bitmap)src).Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            ms.Seek(0, SeekOrigin.Begin);
            image.StreamSource = ms;
            image.EndInit();
            return image;
        }
        //If you get 'dllimport unknown'-, then add 'using System.Runtime.InteropServices;'
        [DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DeleteObject([In] IntPtr hObject);

        public ImageSource ImageSourceFromBitmap(Bitmap bmp)
        {
            var handle = bmp.GetHbitmap();
            try
            {
                return Imaging.CreateBitmapSourceFromHBitmap(handle, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            }
            finally { DeleteObject(handle); }
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (sender.GetType() == typeof(TabItem))
            {
                if (TabItemNow == sender)
                {
                    NowClock.Source = ImageSourceFromBitmap(TimeRender(DateTime.Now));
                }
                else if (TabItemMoscow == sender)
                {
                    MoscowClock.Source = ImageSourceFromBitmap(TimeRender(DateTime.UtcNow + TimeSpan.FromHours(3)));
                }
                else if (TabItemLondon == sender)
                {
                    LondonClock.Source = ImageSourceFromBitmap(TimeRender(DateTime.UtcNow));
                }
                else if (TabItemParis == sender)
                {
                    ParisClock.Source = ImageSourceFromBitmap(TimeRender(DateTime.UtcNow + TimeSpan.FromHours(2)));
                }
                else if (TabItemNewYork == sender)
                {
                    NewYorkClock.Source = ImageSourceFromBitmap(TimeRender(DateTime.UtcNow + TimeSpan.FromHours(-4)));
                }
            }
            else if (TabItemNow.IsSelected)
            {
                NowClock.Source = ImageSourceFromBitmap(TimeRender(DateTime.Now));
            }
            else if (TabItemMoscow.IsSelected)
            {
                MoscowClock.Source = ImageSourceFromBitmap(TimeRender(DateTime.UtcNow + TimeSpan.FromHours(3)));
            }
            else if (TabItemLondon.IsSelected)
            {
                LondonClock.Source = ImageSourceFromBitmap(TimeRender(DateTime.UtcNow));
            }
            else if (TabItemParis.IsSelected)
            {
                ParisClock.Source = ImageSourceFromBitmap(TimeRender(DateTime.UtcNow + TimeSpan.FromHours(2)));
            }
            else if (TabItemNewYork.IsSelected)
            {
                NewYorkClock.Source = ImageSourceFromBitmap(TimeRender(DateTime.UtcNow + TimeSpan.FromHours(-4)));
            }
        }

        private Bitmap TimeRender(DateTime dateTime)
        {
            if (_mainBitmap == null) MainBitmapRender();
            var tickBitmap = new Bitmap(_mainBitmap);
            Graphics graphics = Graphics.FromImage(tickBitmap);
            var hourHandPen = new Pen(_hourHandColor, Convert.ToSingle(_minSide * _hourHandWidth));
            graphics.DrawLine(hourHandPen,
                Convert.ToSingle(Grid.ActualWidth / 2 + Math.Sin(Math.PI * (dateTime.Hour * 60 + dateTime.Minute) / 360d) * (_minSide / 2 * _hourHandEndR)),
                Convert.ToSingle(Grid.ActualHeight / 2 - Math.Cos(Math.PI * (dateTime.Hour * 60 + dateTime.Minute) / 360d) * (_minSide / 2 * _hourHandEndR)),
                Convert.ToSingle(Grid.ActualWidth / 2 + Math.Sin(Math.PI * (dateTime.Hour * 60 + dateTime.Minute) / 360d) * (_minSide / 2 * _hourHandStartR)),
                Convert.ToSingle(Grid.ActualHeight / 2 - Math.Cos(Math.PI * (dateTime.Hour * 60 + dateTime.Minute) / 360d) * (_minSide / 2 * _hourHandStartR)));
            
            var minuteHandPen = new Pen(_minuteHandColor, Convert.ToSingle(_minSide * _minuteHandWidth));
            graphics.DrawLine(minuteHandPen,
                Convert.ToSingle(Grid.ActualWidth / 2 + Math.Sin(Math.PI * (dateTime.Minute) / 30d) * (_minSide / 2 * _minuteHandEndR)),
                Convert.ToSingle(Grid.ActualHeight / 2 - Math.Cos(Math.PI * (dateTime.Minute) / 30d) * (_minSide / 2 * _minuteHandEndR)),
                Convert.ToSingle(Grid.ActualWidth / 2 + Math.Sin(Math.PI * (dateTime.Minute) / 30d) * (_minSide / 2 * _minuteHandStartR)),
                Convert.ToSingle(Grid.ActualHeight / 2 - Math.Cos(Math.PI * (dateTime.Minute) / 30d) * (_minSide / 2 * _minuteHandStartR)));

            var secondHandPen = new Pen(_secondHandColor, Convert.ToSingle(_minSide * _secondHandWidth));
            graphics.DrawLine(secondHandPen,
                Convert.ToSingle(Grid.ActualWidth / 2 + Math.Sin(Math.PI * (dateTime.Second) / 30d) * (_minSide / 2 * _secondHandEndR)),
                Convert.ToSingle(Grid.ActualHeight / 2 - Math.Cos(Math.PI * (dateTime.Second) / 30d) * (_minSide / 2 * _secondHandEndR)),
                Convert.ToSingle(Grid.ActualWidth / 2 + Math.Sin(Math.PI * (dateTime.Second) / 30d) * (_minSide / 2 * _secondHandStartR)),
                Convert.ToSingle(Grid.ActualHeight / 2 - Math.Cos(Math.PI * (dateTime.Second) / 30d) * (_minSide / 2 * _secondHandStartR)));
            return tickBitmap;
        }

        private void Window_ContentRendered_1(object sender, EventArgs e)
        {
            _dispatcherTimer.Start();
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Grid.ActualWidth = NowGrid.ActualWidth * factor;
            Grid.ActualHeight = NowGrid.ActualHeight * factor;
            if (!(double.IsNaN(Grid.ActualWidth) | double.IsNaN(Grid.ActualHeight)))
            {
                MainBitmapRender();
                dispatcherTimer_Tick(sender, EventArgs.Empty);
            }
            else {}
        }

        private void OnTabSelected(object sender, EventArgs e)
        {
            Debug.Print(((TabItem)sender).Name);
            if (Grid != (0, 0))
                dispatcherTimer_Tick(sender, e);
        }
    }
}

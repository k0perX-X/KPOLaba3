using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Color = System.Drawing.Color;
using Pen = System.Drawing.Pen;

namespace KPOLaba3;

public static class ClockGenerator
{
    private static Bitmap? _mainBitmap;
    private static double _minSide;
    private static double factor = 1;
    private static (double ActualWidth, double ActualHeight) GridSize = (200, 200);

    private const double _divisionWidth = 0.02;
    private static readonly Color _divisionColor = Color.Aqua;
    private const double _divisionPointR = 0.75;
    private const double _divisionHeight = 0.1;

    private const double _smallDivisionWidth = 0.01;
    private static readonly Color _smallDivisionColor = Color.Aquamarine;
    private const double _smallDivisionPointR = _divisionPointR + _divisionHeight - _smallDivisionHeight;
    private const double _smallDivisionHeight = 0.05;

    private const double _hourHandWidth = 0.02;
    private static readonly Color _hourHandColor = Color.Black;
    private const double _hourHandStartR = -0.1;
    private const double _hourHandEndR = 0.4;

    private const double _minuteHandWidth = 0.02;
    private static readonly Color _minuteHandColor = Color.Gray;
    private const double _minuteHandStartR = -0.15;
    private const double _minuteHandEndR = 0.6;

    private const double _secondHandWidth = 0.015;
    private static readonly Color _secondHandColor = Color.Red;
    private const double _secondHandStartR = -0.2;
    private const double _secondHandEndR = 0.65;

    public static void MainBitmapRender((double ActualWidth, double ActualHeight) grid)
    {
        grid.ActualHeight *= factor;
        grid.ActualWidth *= factor;
        if (_mainBitmap != null)
            if (_mainBitmap.Width == Convert.ToInt32(grid.ActualWidth) & (_mainBitmap.Height == Convert.ToInt32(grid.ActualHeight))) 
                return;
        GridSize = grid;
        _mainBitmap = new Bitmap(Convert.ToInt32(grid.ActualWidth), Convert.ToInt32(grid.ActualHeight));
        _minSide = grid.ActualWidth > grid.ActualHeight ? grid.ActualHeight : grid.ActualWidth;
        Graphics graphics = Graphics.FromImage(_mainBitmap);
        var divisionPen = new Pen(_divisionColor, Convert.ToSingle(_minSide * _divisionWidth));
        for (int i = 0; i < 12; i++)
        {
            graphics.DrawLine(divisionPen,
                Convert.ToSingle(grid.ActualWidth / 2 + Math.Cos(Math.PI * i / 6) * (_minSide / 2 * _divisionPointR - _minSide / 2 * _divisionHeight)),
                Convert.ToSingle(grid.ActualHeight / 2 + Math.Sin(Math.PI * i / 6) * (_minSide / 2 * _divisionPointR - _minSide / 2 * _divisionHeight)),
                Convert.ToSingle(grid.ActualWidth / 2 + Math.Cos(Math.PI * i / 6) * (_minSide / 2 * _divisionPointR + _minSide / 2 * _divisionHeight)),
                Convert.ToSingle(grid.ActualHeight / 2 + Math.Sin(Math.PI * i / 6) * (_minSide / 2 * _divisionPointR + _minSide / 2 * _divisionHeight)));
        }

        var smallDivisionPen = new Pen(_smallDivisionColor, Convert.ToSingle(_minSide * _smallDivisionWidth));
        for (int i = 0; i < 60; i++)
        {
            if (i % 5 == 0) continue;
            graphics.DrawLine(smallDivisionPen,
                Convert.ToSingle(grid.ActualWidth / 2 + Math.Cos(Math.PI * i / 30) * (_minSide / 2 * _smallDivisionPointR - _minSide / 2 * _smallDivisionHeight)),
                Convert.ToSingle(grid.ActualHeight / 2 + Math.Sin(Math.PI * i / 30) * (_minSide / 2 * _smallDivisionPointR - _minSide / 2 * _smallDivisionHeight)),
                Convert.ToSingle(grid.ActualWidth / 2 + Math.Cos(Math.PI * i / 30) * (_minSide / 2 * _smallDivisionPointR + _minSide / 2 * _smallDivisionHeight)),
                Convert.ToSingle(grid.ActualHeight / 2 + Math.Sin(Math.PI * i / 30) * (_minSide / 2 * _smallDivisionPointR + _minSide / 2 * _smallDivisionHeight)));
        }
    }
    [DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool DeleteObject([In] IntPtr hObject);

    public static ImageSource ImageSourceFromBitmap(Bitmap bmp)
    {
        var handle = bmp.GetHbitmap();
        try
        {
            return Imaging.CreateBitmapSourceFromHBitmap(handle, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
        }
        finally { DeleteObject(handle); }
    }
    public static Bitmap TimeRender(DateTime dateTime)
    {
        if (_mainBitmap == null) MainBitmapRender(GridSize);
        var tickBitmap = new Bitmap(_mainBitmap);
        Graphics graphics = Graphics.FromImage(tickBitmap);
        var hourHandPen = new Pen(_hourHandColor, Convert.ToSingle(_minSide * _hourHandWidth));
        graphics.DrawLine(hourHandPen,
            Convert.ToSingle(GridSize.ActualWidth / 2 + Math.Sin(Math.PI * (dateTime.Hour * 60 + dateTime.Minute) / 360d) * (_minSide / 2 * _hourHandEndR)),
            Convert.ToSingle(GridSize.ActualHeight / 2 - Math.Cos(Math.PI * (dateTime.Hour * 60 + dateTime.Minute) / 360d) * (_minSide / 2 * _hourHandEndR)),
            Convert.ToSingle(GridSize.ActualWidth / 2 + Math.Sin(Math.PI * (dateTime.Hour * 60 + dateTime.Minute) / 360d) * (_minSide / 2 * _hourHandStartR)),
            Convert.ToSingle(GridSize.ActualHeight / 2 - Math.Cos(Math.PI * (dateTime.Hour * 60 + dateTime.Minute) / 360d) * (_minSide / 2 * _hourHandStartR)));

        var minuteHandPen = new Pen(_minuteHandColor, Convert.ToSingle(_minSide * _minuteHandWidth));
        graphics.DrawLine(minuteHandPen,
            Convert.ToSingle(GridSize.ActualWidth / 2 + Math.Sin(Math.PI * (dateTime.Minute) / 30d) * (_minSide / 2 * _minuteHandEndR)),
            Convert.ToSingle(GridSize.ActualHeight / 2 - Math.Cos(Math.PI * (dateTime.Minute) / 30d) * (_minSide / 2 * _minuteHandEndR)),
            Convert.ToSingle(GridSize.ActualWidth / 2 + Math.Sin(Math.PI * (dateTime.Minute) / 30d) * (_minSide / 2 * _minuteHandStartR)),
            Convert.ToSingle(GridSize.ActualHeight / 2 - Math.Cos(Math.PI * (dateTime.Minute) / 30d) * (_minSide / 2 * _minuteHandStartR)));

        var secondHandPen = new Pen(_secondHandColor, Convert.ToSingle(_minSide * _secondHandWidth));
        graphics.DrawLine(secondHandPen,
            Convert.ToSingle(GridSize.ActualWidth / 2 + Math.Sin(Math.PI * (dateTime.Second) / 30d) * (_minSide / 2 * _secondHandEndR)),
            Convert.ToSingle(GridSize.ActualHeight / 2 - Math.Cos(Math.PI * (dateTime.Second) / 30d) * (_minSide / 2 * _secondHandEndR)),
            Convert.ToSingle(GridSize.ActualWidth / 2 + Math.Sin(Math.PI * (dateTime.Second) / 30d) * (_minSide / 2 * _secondHandStartR)),
            Convert.ToSingle(GridSize.ActualHeight / 2 - Math.Cos(Math.PI * (dateTime.Second) / 30d) * (_minSide / 2 * _secondHandStartR)));
        return tickBitmap;
    }
}
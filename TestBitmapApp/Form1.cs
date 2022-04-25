namespace TestBitmapApp
{
    public partial class Form1 : Form
    {
        private Bitmap? _mainBitmap;
        private const double _divisionWidth = 0.02;
        private readonly Color _divisionColor = Color.Aqua;
        private const double _divisionPointR = 0.85;
        private const double _divisionHeight = 0.1;
        private const double _smallDivisionWidth = 0.01;
        private readonly Color _smallDivisionColor = Color.Aquamarine;
        private const double _smallDivisionPointR = _divisionPointR + _divisionHeight - _smallDivisionHeight;
        private const double _smallDivisionHeight = 0.05;
        private (int ActualWidth, int ActualHeight) Grid;
        public Form1()
        {
            InitializeComponent();
            Grid.ActualWidth = pictureBox1.Width;
            Grid.ActualHeight = pictureBox1.Height;
            MainBitmapRender();
            pictureBox1.Image = _mainBitmap;
        }
        private void MainBitmapRender()
        {
            _mainBitmap = new Bitmap(Convert.ToInt32(Grid.ActualWidth), Convert.ToInt32(Grid.ActualHeight));
            double minSide = Grid.ActualWidth > Grid.ActualHeight ? Grid.ActualHeight : Grid.ActualWidth;
            Graphics graphics = Graphics.FromImage(_mainBitmap);
            var divisionPen = new Pen(_divisionColor, Convert.ToSingle(minSide * _divisionWidth));
            for (int i = 0; i < 12; i++)
            {
                graphics.DrawLine(divisionPen,
                    Convert.ToSingle(Grid.ActualWidth / 2 + Math.Cos(Math.PI * i / 6) * (minSide / 2 * _divisionPointR - minSide / 2 * _divisionHeight)),
                    Convert.ToSingle(Grid.ActualHeight / 2 + Math.Sin(Math.PI * i / 6) * (minSide / 2 * _divisionPointR - minSide / 2 * _divisionHeight)),
                    Convert.ToSingle(Grid.ActualWidth / 2 + Math.Cos(Math.PI * i / 6) * (minSide / 2 * _divisionPointR + minSide / 2 * _divisionHeight)),
                    Convert.ToSingle(Grid.ActualHeight / 2 + Math.Sin(Math.PI * i / 6) * (minSide / 2 * _divisionPointR + minSide / 2 * _divisionHeight)));
            }

            var smallDivisionPen = new Pen(_smallDivisionColor, Convert.ToSingle(minSide * _smallDivisionWidth));
            for (int i = 0; i < 60; i++)
            {
                if (i % 5 == 0) continue;
                graphics.DrawLine(smallDivisionPen,
                    Convert.ToSingle(Grid.ActualWidth / 2 + Math.Cos(Math.PI * i / 30) * (minSide / 2 * _smallDivisionPointR - minSide / 2 * _smallDivisionHeight)),
                    Convert.ToSingle(Grid.ActualHeight / 2 + Math.Sin(Math.PI * i / 30) * (minSide / 2 * _smallDivisionPointR - minSide / 2 * _smallDivisionHeight)),
                    Convert.ToSingle(Grid.ActualWidth / 2 + Math.Cos(Math.PI * i / 30) * (minSide / 2 * _smallDivisionPointR + minSide / 2 * _smallDivisionHeight)),
                    Convert.ToSingle(Grid.ActualHeight / 2 + Math.Sin(Math.PI * i / 30) * (minSide / 2 * _smallDivisionPointR + minSide / 2 * _smallDivisionHeight)));
            }

        }

    }
}
using CollatzLibrary;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace CollatzGraph
{   
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // Display peak values or counts
        private enum Mode { Count, Peak }  
        private Mode _mode = Mode.Count;

        private readonly ConcurrentBag<SequenceItem> _coords = new ConcurrentBag<SequenceItem>();

        // Max calculated values
        private uint _maxCount;
        private ulong _maxPeak;

        // See range (x-axis)
        private ulong _lo;
        private ulong _hi;

        // Chnage colour on each redraw
        private readonly SolidColorBrush[] _brushes = { Brushes.Blue, Brushes.Red, Brushes.Black, Brushes.Magenta };
        private int _brushIdx = 0;
        
        private void Generate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;

                Calculate();

                Draw();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                Mouse.OverrideCursor = null;
            }
        }

        private void Calculate()
        {
            var lo = ulong.Parse(fromBox.Text);
            var hi = ulong.Parse(toBox.Text);

            if (lo != _lo || hi != _hi) // Don't redraw if nothing changed
            {
                _lo = lo;
                _hi = hi;

                CollatzCalculator calculator = new CollatzCalculator();

                _coords.Clear();

                // Calculate for each seed and save result
                Parallel.For((long)_lo, (long)_hi, seed =>
                {
                    var entry = calculator.GetEntry((ulong)seed);

                    _coords.Add(entry);
                });

                _maxCount = _coords.Max(v => v.Count);
                _maxPeak = _coords.Max(v => v.Peak);
            }
        }

        private void Draw()
        {
            if (canvas != null && _coords.Count > 0)
            {
                try
                {
                    Mouse.OverrideCursor = Cursors.Wait;

                    _brushIdx = (_brushIdx + 1) % _brushes.Length; // Cycle display colour

                    canvas.Children.Clear();
                    scale.Children.Clear();

                    switch (_mode)
                    {
                        case Mode.Count: DrawGrid(_maxCount, i => i.Count); break;
                        case Mode.Peak: DrawGrid(_maxPeak, i => i.Peak); break;
                    }
                }
                finally
                {
                    Mouse.OverrideCursor = null;
                }
            }
        }

        private void DrawGrid(double maxY, Func<SequenceItem, double> selector)
        {
            double yScale = double.Parse(clipBox.Text) / 100;  // Only show lower percentage of y values

            var actualMaxY = maxY * yScale;

            // Scale factors from values to canvas coordinates
            var heightFactor = (double)canvas.ActualHeight / actualMaxY;
            double widthFactor = (double)canvas.ActualWidth / (_hi - _lo);

            DrawGraph(selector, widthFactor, heightFactor);
            DrawYScale(actualMaxY, heightFactor);
        }

        private void DrawGraph(Func<SequenceItem, double> selector, double widthFactor, double heightFactor)
        {
            var grid = new int[(int)canvas.ActualWidth, (int)canvas.ActualHeight];

            foreach (var coord in _coords)
            {
                int x = (int)((coord.Current - _lo) * widthFactor);
                int y = (int)(canvas.ActualHeight - selector(coord) * heightFactor);

                if (x >= 0 && y >= 0 && x < grid.GetLength(0) && y < grid.GetLength(1))
                    ++grid[x, y];  // Value indicates number of hits of this pixel. Could be used to display a colour for intensity, but currently we just check for > 0
            }

            for (int x = 0; x < grid.GetLength(0); ++x)
                for (int y = 0; y < grid.GetLength(1); ++y)
                {
                    if (grid[x, y] > 0)
                    {
                        // Line sgement to represent point
                        var line = new Line
                        {
                            X1 = x,
                            X2 = x,
                            Y1 = y,
                            Y2 = y + 1,
                            Stroke = _brushes[_brushIdx]
                        };

                        canvas.Children.Add(line);
                    }
                }
        }

        private void DrawYScale(double actualMaxY, double heightFactor)
        {
            IEnumerable<ulong> scaleValues = GetScaleValues(actualMaxY); // Values to be marked on y-axis

            foreach (var val in scaleValues)
            {
                // Label
                var text = new TextBlock();

                text.Text = $"{val:N0}";

                Canvas.SetRight(text, 15);
                Canvas.SetBottom(text, val * heightFactor - 6);

               scale.Children.Add(text);

                // Marker line
                var line = new Line
                {
                    X1 = scale.ActualWidth - 10,
                    X2 = scale.ActualWidth,
                    Y1 = scale.ActualHeight - val * heightFactor,
                    Y2 = scale.ActualHeight - val * heightFactor,
                    Stroke = Brushes.Black,
                    StrokeThickness = 1,
                    SnapsToDevicePixels = true
                };

                line.SetValue(RenderOptions.EdgeModeProperty, EdgeMode.Aliased);

                scale.Children.Add(line);
            }
        }

        private IEnumerable<ulong> GetScaleValues(double max)
        {
            // The idea is to show as many powers of two as possible

            // Determine highest power of two less than maximum value
            double highest2pow = Math.Pow(2, (int)Math.Log2(max));

            // Number of values to show up to the power of 2 (there will be a few more beyond it) adjusted for display height
            var displaycount = new int[] { 2, 4, 8, 16 }[Math.Min(3,(int)(canvas.ActualHeight/300))];

            // Gap between scale points
            ulong step = (ulong)(highest2pow / displaycount);

            var vals = new List<ulong>();

            ulong mark = step;

            while (mark < max)
            {
                vals.Add(mark);
                mark += step;
            }

            return vals;
        }

        private void Canvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Draw();
        }

        private void Counts_Checked(object sender, RoutedEventArgs e)
        {
            _mode = Mode.Count;

            if (clipBox != null)
                clipBox.Text = "100";

            Draw();
        }

        private void Peaks_Checked(object sender, RoutedEventArgs e)
        {
            _mode = Mode.Peak;

            if (clipBox != null)
                clipBox.Text = "0.1";

            Draw();
        }
    }
}

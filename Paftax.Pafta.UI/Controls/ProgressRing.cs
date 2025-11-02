using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Paftax.Pafta.UI.Controls
{
    public class ProgressRing : Control
    {
        private double _indeterminateOffset = 0.0;
        private readonly Stopwatch _stopwatch = new();
        private EventHandler? _renderHandler;
        private TimeSpan _lastRenderTime;

        static ProgressRing()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ProgressRing),
                new FrameworkPropertyMetadata(typeof(ProgressRing)));
        }

        #region Dependency Properties

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(nameof(Value), typeof(double), typeof(ProgressRing),
                new PropertyMetadata(0.0, OnValueChanged));

        public static readonly DependencyProperty MaximumProperty =
            DependencyProperty.Register(nameof(Maximum), typeof(double), typeof(ProgressRing),
                new PropertyMetadata(100.0));

        public static readonly DependencyProperty StrokeThicknessProperty =
            DependencyProperty.Register(nameof(StrokeThickness), typeof(double), typeof(ProgressRing),
                new PropertyMetadata(4.0));

        public static readonly DependencyProperty IsIndeterminateProperty =
            DependencyProperty.Register(nameof(IsIndeterminate), typeof(bool), typeof(ProgressRing),
                new PropertyMetadata(false, OnIsIndeterminateChanged));

        public static readonly DependencyProperty SpeedPropery =
            DependencyProperty.Register(nameof(Speed), typeof(double), typeof(ProgressRing),
                new PropertyMetadata(2.0));
        #endregion

        #region Properties
        public double Value
        {
            get => (double)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        public double Maximum
        {
            get => (double)GetValue(MaximumProperty);
            set => SetValue(MaximumProperty, value);
        }

        public double StrokeThickness
        {
            get => (double)GetValue(StrokeThicknessProperty);
            set => SetValue(StrokeThicknessProperty, value);
        }

        public bool IsIndeterminate
        {
            get => (bool)GetValue(IsIndeterminateProperty);
            set => SetValue(IsIndeterminateProperty, value);
        }

        public double Speed
        {
            get => (double)GetValue(SpeedPropery);
            set => SetValue(SpeedPropery, value);
        }

        #endregion

        public ProgressRing()
        {
            Loaded += ProgressRing_Loaded;
            Unloaded += ProgressRing_Unloaded;
        }

        private void ProgressRing_Loaded(object sender, RoutedEventArgs e)
        {
            if (IsIndeterminate)
                StartIndeterminateAnimation();
        }

        private void ProgressRing_Unloaded(object sender, RoutedEventArgs e)
        {
            StopIndeterminateAnimation();
        }

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ProgressRing ring)
                ring.InvalidateVisual();
        }

        private static void OnIsIndeterminateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ProgressRing ring)
            {
                if ((bool)e.NewValue)
                    ring.StartIndeterminateAnimation();
                else
                    ring.StopIndeterminateAnimation();

                ring.InvalidateVisual();
            }
        }

        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);
            DrawSpinner(dc);
        }

        private void DrawSpinner(DrawingContext dc)
        {
            if (ActualWidth <= 0 || ActualHeight <= 0) return;

            double size = Math.Min(ActualWidth, ActualHeight);
            double radius = (size - StrokeThickness) * 0.5;
            if (radius <= 0) return;

            double cx = ActualWidth * 0.5;
            double cy = ActualHeight * 0.5;

            double startAngle = -90.0 + (IsIndeterminate ? _indeterminateOffset : 0.0);

            double sweepAngle;
            if (IsIndeterminate)
            {
                sweepAngle = 90.0;
            }
            else
            {
                double max = Math.Max(0.000001, Maximum);
                double ratio = Math.Max(0.0, Math.Min(1.0, Value / max));
                sweepAngle = 359.999 * ratio;

                if (ratio > 0 && sweepAngle < 3.0) sweepAngle = 3.0;
            }

            double toRad = Math.PI / 180.0;
            Point start = new(
                cx + radius * Math.Cos(startAngle * toRad),
                cy + radius * Math.Sin(startAngle * toRad));

            Point end = new(
                cx + radius * Math.Cos((startAngle + sweepAngle) * toRad),
                cy + radius * Math.Sin((startAngle + sweepAngle) * toRad));

            bool isLargeArc = sweepAngle > 180.0;

            var geom = new StreamGeometry();
            using (var ctx = geom.Open())
            {
                ctx.BeginFigure(start, isFilled: false, isClosed: false);
                ctx.ArcTo(end, new Size(radius, radius), rotationAngle: 0,
                          isLargeArc, SweepDirection.Clockwise, isStroked: true, isSmoothJoin: false);
            }
            geom.Freeze();

            var pen = new Pen(Foreground, StrokeThickness)
            {
                StartLineCap = PenLineCap.Round,
                EndLineCap = PenLineCap.Round
            };

            dc.DrawGeometry(null, pen, geom);
        }

        private void StartIndeterminateAnimation()
        {
            if (_renderHandler != null) return;

            _stopwatch.Restart();
            _lastRenderTime = TimeSpan.Zero;
            _renderHandler = OnRendering;
            CompositionTarget.Rendering += _renderHandler;
        }

        private void StopIndeterminateAnimation()
        {
            if (_renderHandler != null)
            {
                CompositionTarget.Rendering -= _renderHandler;
                _renderHandler = null;
            }
            _stopwatch.Stop();
        }

        private void OnRendering(object? sender, EventArgs e)
        {
            const double legacyTickMs = 20.0;
            double degreesPerSecond = Math.Max(0.0, Speed) * (1000.0 / legacyTickMs);

            var now = _stopwatch.Elapsed;
            var dt = now - _lastRenderTime;
            _lastRenderTime = now;

            _indeterminateOffset = (_indeterminateOffset + degreesPerSecond * dt.TotalSeconds) % 360.0;

            InvalidateVisual();
        }
    }
}
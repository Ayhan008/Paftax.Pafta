using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Paftax.Pafta.UI.Test.FrameworkElements
{
    internal class DrawingCanvas : FrameworkElement
    {
        private readonly VisualCollection _children;
        private Matrix _transformMatrix = Matrix.Identity;

        private bool _isPanning = false;
        private Point _lastMousePosition;

        public DrawingCanvas()
        {
            _children = new VisualCollection(this);
            Focusable = true;
            Background = Brushes.Transparent;
            Width = 800;
            Height = 800;
            this.IsHitTestVisible = true;

            Loaded += (s, e) => this.Focus();
            this.MouseWheel += OnMouseWheel;
            this.MouseDown += OnMouseDown;
            this.MouseUp += OnMouseUp;
            this.MouseMove += OnMouseMove;
        }

        public Brush Background
        {
            get => _background;
            set
            {
                _background = value;
                InvalidateVisual();
            }
        }
        private Brush _background = Brushes.Transparent;

        protected override Size MeasureOverride(Size availableSize)
        {
            // Ensure the canvas always has a size (default to 100x100 if unconstrained)
            if (double.IsInfinity(availableSize.Width) || double.IsInfinity(availableSize.Height))
                return new Size(100, 100);
            return availableSize;
        }

        protected override HitTestResult HitTestCore(PointHitTestParameters hitTestParameters)
        {
            // Always return a hit so mouse events fire everywhere
            return new PointHitTestResult(this, hitTestParameters.HitPoint);
        }
        protected override Size ArrangeOverride(Size finalSize) => finalSize;

        protected override int VisualChildrenCount => _children.Count;

        protected override Visual GetVisualChild(int index) => _children[index];

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            // Make entire area hit-testable so mouse events fire even on empty space
            drawingContext.DrawRectangle(Background, null, new Rect(new Point(), RenderSize));

            drawingContext.PushTransform(new MatrixTransform(_transformMatrix));
            drawingContext.Pop();
        }

        private void OnMouseWheel(object sender, MouseWheelEventArgs e)
        {
            Point mousePosition = e.GetPosition(this);
            double zoomFactor = e.Delta > 0 ? 1.1 : 1 / 1.1;

            Matrix m = _transformMatrix;
            m.Translate(-mousePosition.X, -mousePosition.Y);
            m.Scale(zoomFactor, zoomFactor);
            m.Translate(mousePosition.X, mousePosition.Y);

            _transformMatrix = m;
            InvalidateVisual();
        }

        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.MiddleButton == MouseButtonState.Pressed ||
                (Keyboard.IsKeyDown(Key.LeftShift) && e.LeftButton == MouseButtonState.Pressed))
            {
                _isPanning = true;
                _lastMousePosition = e.GetPosition(this);
                CaptureMouse();
            }
        }

        private void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            _isPanning = false;
            ReleaseMouseCapture();
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (_isPanning)
            {
                Point pos = e.GetPosition(this);
                Vector delta = pos - _lastMousePosition;

                _transformMatrix.Translate(delta.X, delta.Y);

                _lastMousePosition = pos;
                InvalidateVisual();
            }
        }

        public void AddLine(Point start, Point end, Brush stroke, double thickness = 2)
        {
            var visual = new DrawingVisual();
            using (var dc = visual.RenderOpen())
            {
                dc.DrawLine(new Pen(stroke, thickness), start, end);
            }
            _children.Add(visual);
        }

        public void AddRectangle(Rect rect, Brush fill, Brush stroke, double thickness = 2)
        {
            var visual = new DrawingVisual();
            using (var dc = visual.RenderOpen())
            {
                dc.DrawRectangle(fill, new Pen(stroke, thickness), rect);
            }
            _children.Add(visual);
        }

        public void AddEllipse(Rect bounds, Brush fill, Brush stroke, double thickness = 2)
        {
            var visual = new DrawingVisual();
            using (var dc = visual.RenderOpen())
            {
                dc.DrawEllipse(fill, new Pen(stroke, thickness),
                               new Point(bounds.X + bounds.Width / 2, bounds.Y + bounds.Height / 2),
                               bounds.Width / 2, bounds.Height / 2);
            }
            _children.Add(visual);
        }
    }
}
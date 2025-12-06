using Paftax.Pafta.Drawings.Elements;
using Paftax.Pafta.Drawings.Visuals;
using Paftax.Pafta.Shared.Geometries;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Paftax.Pafta.Drawings
{
    public class DrawingCanvas : Canvas
    {
        private Matrix _modelMatrix = Matrix.Identity;
        private Matrix _viewMatrix = Matrix.Identity;
        private readonly MatrixTransform _totalTransform = new();

        private readonly List<DrawingElement> _elements = [];

        #region Dependency Properties
        public static readonly DependencyProperty AnnotationScaleProperty = DependencyProperty.Register(
            nameof(AnnotationScale),
            typeof(double),
            typeof(DrawingCanvas),
            new FrameworkPropertyMetadata(1.0, OnScaleChanged));

        public double AnnotationScale
        {
            get => (double)GetValue(AnnotationScaleProperty);
            set => SetValue(AnnotationScaleProperty, value);
        }

        private static void OnScaleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is DrawingCanvas canvas)
            {
                double newScale = (double)e.NewValue;

                foreach (DrawingElement element in canvas._elements)
                {
                    element.Scale = newScale;
                    if (element.Visual is GeometryVisual gv && element.IsAnnotation)
                        gv.Scale = newScale;
                    element.InvalidateVisual();
                }
            }
        }

        public static readonly DependencyProperty ModelMousePositionProperty = DependencyProperty.Register(
            nameof(ModelMousePosition),
            typeof(Point),
            typeof(DrawingCanvas),
            new FrameworkPropertyMetadata(new Point(0, 0), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public Point ModelMousePosition
        {
            get => (Point)GetValue(ModelMousePositionProperty);
            set => SetValue(ModelMousePositionProperty, value);
        }
        #endregion

        #region Constructor
        static DrawingCanvas()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DrawingCanvas),
                new FrameworkPropertyMetadata(typeof(DrawingCanvas)));
        }

        public DrawingCanvas()
        {
            Focusable = true;
            Background = Brushes.LightGray;

            Loaded += OnLoaded;
            SizeChanged += OnSizeChanged;
            MouseMove += OnMouseMove;
            MouseDown += OnMouseDown;
            MouseUp += OnMouseUp;
            MouseWheel += OnMouseWheel;
            KeyDown += OnKeyDown;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Focus();
            UpdateModelMatrix(new Size(ActualWidth, ActualHeight));
            ZoomToFitElements();
        }

        private void OnSizeChanged(object? sender, SizeChangedEventArgs e)
        {
            Focus();
            UpdateModelMatrix(e.NewSize);
            ZoomToFitElements();
        }

        private void UpdateModelMatrix(Size size)
        {
            _modelMatrix = Matrix.Identity;
            _modelMatrix.Scale(1, -1);
            _modelMatrix.Translate(size.Width / 2, size.Height / 2);
            UpdateTotalMatrix();
        }
        #endregion

        #region Transform Helpers
        private void UpdateTotalMatrix()
        {
            Matrix total = _modelMatrix;
            total.Append(_viewMatrix);
            _totalTransform.Matrix = total;

            foreach (var el in _elements)
                el.RenderTransform = _totalTransform;
        }

        private Point ScreenToModel(Point screen)
        {
            Matrix total = _modelMatrix;
            total.Append(_viewMatrix);
            if (total.HasInverse)
            {
                total.Invert();
                return total.Transform(screen);
            }
            return new Point();
        }
        #endregion

        #region Pan & Zoom
        private bool _isPanning;
        private Point _lastMousePosition;
        private const double ZoomFactor = 1.1;

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Z || e.Key == Key.E)
            {
                ZoomToFitElements();
            }
        }
        private void OnMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (_isPanning) return;

            Point screenPos = e.GetPosition(this);
            double scale = e.Delta < 0 ? 1 / ZoomFactor : ZoomFactor;

            _viewMatrix.Translate(-screenPos.X, -screenPos.Y);
            _viewMatrix.Scale(scale, scale);
            _viewMatrix.Translate(screenPos.X, screenPos.Y);

            UpdateTotalMatrix();
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            Point current = e.GetPosition(this);
            ModelMousePosition = UnitConverter.PointToMm(ScreenToModel(current));

            if (_isPanning)
            {
                Vector delta = current - _lastMousePosition;
                _viewMatrix.Translate(delta.X, delta.Y);
                _lastMousePosition = current;
                UpdateTotalMatrix();
            }
        }

        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftShift) && e.RightButton == MouseButtonState.Pressed)
            {
                _isPanning = true;
                _lastMousePosition = e.GetPosition(this);
                CaptureMouse();
                Cursor = Cursors.SizeAll;
            }
        }

        private void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (_isPanning)
            {
                _isPanning = false;
                ReleaseMouseCapture();
                Cursor = Cursors.Arrow;
            }
        }
        #endregion

        #region Element Management
        public void AddElement(DrawingElement element)
        {
            _elements.Add(element);
            element.RenderTransform = _totalTransform;

            if (element.Visual is GeometryVisual gv && element.IsAnnotation)
                gv.Scale = AnnotationScale;

            Children.Add(element);
        }

        public void RemoveElement(DrawingElement element)
        {
            _elements.Remove(element);
            Children.Remove(element);
        }

        private void ZoomToFitElements()
        {
            if (_elements.Count == 0)
                return;

            Rect? bounds = null;
            foreach (DrawingElement el in _elements)
            {
                if (el.IsAnnotation)
                    continue;

                Bounding2 b = el.Bounding;
                Rect rect = new(b.Min, b.Max);
                bounds = bounds.HasValue ? Rect.Union(bounds.Value, rect) : rect;
            }

            if (bounds is null || bounds.Value.IsEmpty)
                return;

            var modelBounds = bounds.Value;

            double marginRatio = 2.0;
            modelBounds.Inflate(modelBounds.Width * marginRatio, modelBounds.Height * marginRatio);

            double viewWidth = ActualWidth;
            double viewHeight = ActualHeight;

            if (viewWidth <= 1 || viewHeight <= 1)
                return;

            double scaleX = viewWidth / modelBounds.Width;
            double scaleY = viewHeight / modelBounds.Height;
            double targetScale = Math.Min(scaleX, scaleY);

            Point modelCenter = new(
                modelBounds.X + modelBounds.Width / 2.0,
                modelBounds.Y + modelBounds.Height / 2.0);

            _viewMatrix = Matrix.Identity;
            _viewMatrix.Translate(-modelCenter.X, -modelCenter.Y);
            _viewMatrix.Scale(targetScale, targetScale);
            _viewMatrix.Translate(viewWidth / 2.0, viewHeight / 2.0);

            UpdateTotalMatrix();
        }
        #endregion
    }
}

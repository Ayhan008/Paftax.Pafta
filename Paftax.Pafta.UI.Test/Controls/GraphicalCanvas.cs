using Paftax.Pafta.Drawings.Elements;
using Paftax.Pafta.Drawings.Visuals;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Paftax.Pafta.UI.Test.Controls
{
    public class GraphicalCanvas : Canvas
    {
        private Matrix _matrix = Matrix.Identity;
        private readonly MatrixTransform _matrixTransform = new();
        private readonly List<DrawingElement> _elements = [];

        #region Properties
        public static readonly DependencyProperty AnnotationScaleProperty = DependencyProperty.Register(
                nameof(AnnotationScale),
                typeof(double),
                typeof(GraphicalCanvas),
                new FrameworkPropertyMetadata(1.0, OnScaleChanged));

        public double AnnotationScale
        {
            get => (double)GetValue(AnnotationScaleProperty);
            set => SetValue(AnnotationScaleProperty, value);
        }

        private static void OnScaleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is GraphicalCanvas GraphicalCanvas)
            {
                double newScale = (double)e.NewValue;

                foreach (DrawingElement element in GraphicalCanvas._elements)
                {
                    element.Scale = newScale;
                    if (element.Visual is GeometryVisual gv)
                    {
                        gv.Scale = newScale;
                    }
                    element.InvalidateVisual();
                }
            }
        }

        public static readonly DependencyProperty ModelMousePositionProperty = DependencyProperty.Register(
                nameof(ModelMousePosition),
                typeof(Point),
                typeof(GraphicalCanvas),
                new FrameworkPropertyMetadata(new Point(0, 0), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public Point ModelMousePosition
        {
            get => (Point)GetValue(ModelMousePositionProperty);
            set => SetValue(ModelMousePositionProperty, value);
        }
        #endregion

        #region Constructor and Initialization
        static GraphicalCanvas()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(GraphicalCanvas), new FrameworkPropertyMetadata(typeof(GraphicalCanvas)));
        }
        public GraphicalCanvas()
        {
            Background = Brushes.LightGray;
            AnnotationScale = 5;

            Loaded += OnLoaded;
            SizeChanged += OnSizeChanged;
            MouseMove += OnMouseMove;
            MouseDown += OnMouseDown;
            MouseUp += OnMouseUp;
            MouseWheel += OnMouseWheel;

            ElevationMarker elevationMarker = new()
            {
                Origin = new Point(0, 0)
            };
            AddElement(elevationMarker);

            foreach (var el in _elements)
            {
                el.RenderTransform = _matrixTransform;
                if (el.Visual is GeometryVisual gv)
                {
                    gv.Scale = AnnotationScale;
                }
            }
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            UpdateWorldTransform(new Size(ActualWidth, ActualHeight));
        }

        private void OnSizeChanged(object? sender, SizeChangedEventArgs e)
        {
            UpdateWorldTransform(e.NewSize);
        }

        private void UpdateWorldTransform(Size size)
        {
            _matrix = Matrix.Identity;
            _matrix.Scale(1, -1);
            _matrix.Translate(size.Width / 2, size.Height / 2);

            _matrixTransform.Matrix = _matrix;

            foreach (var el in _elements)
                el.InvalidateVisual();
        }
        #endregion

        #region Pan and Zoom Methods
        private bool _isPanning;
        private Point _lastMousePosition;
        private const double ZoomFactor = 1.1;

        private void OnMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (_isPanning)
                return;

            Point mousePosition = e.GetPosition(this);
            double scale = e.Delta > 0 ? ZoomFactor : 1 / ZoomFactor;

            _matrix.Translate(-mousePosition.X, -mousePosition.Y);
            _matrix.Scale(scale, scale);
            _matrix.Translate(mousePosition.X, mousePosition.Y);
            _matrixTransform.Matrix = _matrix;
        }
        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            Point currentScreen = e.GetPosition(this);
            ModelMousePosition = ToModel(currentScreen);

            if (_isPanning)
            {
                Vector delta = currentScreen - _lastMousePosition;

                _matrix.Translate(delta.X, delta.Y);
                _matrixTransform.Matrix = _matrix;

                _lastMousePosition = currentScreen;
            }
        }

        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftShift) && e.RightButton == MouseButtonState.Pressed)
            {
                _isPanning = true;
                _lastMousePosition = e.GetPosition(this);
                CaptureMouse();
            }

            Cursor = Keyboard.IsKeyDown(Key.LeftShift) && e.RightButton == MouseButtonState.Pressed
                ? Cursors.SizeAll
                : Cursors.Arrow;
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
            Children.Add(element);
        }
        public void RemoveElement(DrawingElement element)
        {
            _elements.Remove(element);
            Children.Remove(element);
        }
        #endregion

        #region Helpers
        private Point ToModel(Point screenPoint)
        {
            Matrix inverse = _matrix;
            if (inverse.HasInverse)
                inverse.Invert();

            return inverse.Transform(screenPoint);
        }
        #endregion
    }
}

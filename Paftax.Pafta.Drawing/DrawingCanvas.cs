using Paftax.Pafta.Drawing.Elements;
using Paftax.Pafta.Drawing.Elements.Abstracts;
using Paftax.Pafta.Drawing.Structs;
using Paftax.Pafta.Drawing.Utilities;
using Paftax.Pafta.Drawing.Visuals.Abstracts;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Paftax.Pafta.Drawing
{
    public class DrawingCanvas : Canvas
    {
        public event Action<Point, Point>? MouseMovedInContent;

        public List<DrawingElement> ModelElements = [];
        public List<DrawingElement> ScreenElements = [];

        private readonly Canvas _contentCanvas = new();

        #region Scale Property (Annotation için)
        public static readonly DependencyProperty ScaleProperty =
            DependencyProperty.Register(
                nameof(Scale),
                typeof(double),
                typeof(DrawingCanvas),
                new PropertyMetadata(1.0, OnScaleChanged));

        public double Scale
        {
            get => (double)GetValue(ScaleProperty);
            set => SetValue(ScaleProperty, value);
        }

        public static void OnScaleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is DrawingCanvas drawingCanvas)
            {
                double newScale = (double)e.NewValue;

                foreach (var element in drawingCanvas.ScreenElements)
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
        #endregion

        #region Pan & Zoom
        private readonly ScaleTransform _zoomTransform = new(1, 1);
        private readonly ScaleTransform _flipYTransform = new(1, -1);
        private readonly TranslateTransform _panTransform = new();
        private readonly TransformGroup _transformGroup = new();

        private Point _lastMousePos;
        private bool _isPanning;

        public double Zoom => _zoomTransform.ScaleX;
        #endregion

        public DrawingCanvas()
        {
            Background = Brushes.LightGray;
            ClipToBounds = true;
            
            Children.Add(_contentCanvas);

            _transformGroup.Children.Add(_flipYTransform);
            _transformGroup.Children.Add(_panTransform);
            _transformGroup.Children.Add(_zoomTransform);
            _contentCanvas.RenderTransform = _transformGroup;

            Canvas.SetLeft(_contentCanvas, ActualWidth / 2.0);
            Canvas.SetTop(_contentCanvas, ActualHeight / 2.0);
            _flipYTransform.CenterX = 0;
            _flipYTransform.CenterY = 0;

            Loaded += (s, e) => { CenterContent(); ZoomToFitAll(); };
            SizeChanged += (s, e) => CenterContent();

            MouseWheel += OnMouseWheel;
            MouseDown += OnMouseDown;
            MouseMove += OnMouseMove;
            MouseUp += OnMouseUp;
        }

        private void CenterContent()
        {
            Canvas.SetLeft(_contentCanvas, ActualWidth / 2.0);
            Canvas.SetTop(_contentCanvas, ActualHeight / 2.0);
        }

        private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if ((Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift)) &&
                e.RightButton == MouseButtonState.Pressed)
            {
                _isPanning = true;
                _lastMousePos = e.GetPosition(this);
                CaptureMouse();
            }

            if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
            {
                Cursor = Cursors.SizeAll;
            }
            else
            {
                Cursor = Cursors.Arrow;
            }
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (_isPanning)
            {
                var pos = e.GetPosition(this);
                var dx = pos.X - _lastMousePos.X;
                var dy = pos.Y - _lastMousePos.Y;

                _panTransform.X += dx / _zoomTransform.ScaleX;
                _panTransform.Y += dy / _zoomTransform.ScaleY;

                _lastMousePos = pos;
            }
            var screenOnContent = e.GetPosition(_contentCanvas);

            var worldX = (screenOnContent.X - ActualWidth / 2.0);
            var worldY = (screenOnContent.Y - ActualHeight / 2.0);
            var worldPoint = new Point(worldX, worldY);

            MouseMovedInContent?.Invoke(
                UnitConverter.PointToM(screenOnContent),
                UnitConverter.PointToM(worldPoint)
            );
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

        private void OnMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
                return;

            double zoomFactor = e.Delta > 0 ? 1.1 : 0.9;

            var mouseScreenPos = e.GetPosition(this);
            var inverseTransform = _transformGroup.Inverse;
            if (inverseTransform == null) return;

            var mouseWorldBefore = inverseTransform.Transform(mouseScreenPos);

            _zoomTransform.ScaleX *= zoomFactor;
            _zoomTransform.ScaleY *= zoomFactor;

            var mouseWorldAfter = inverseTransform.Transform(mouseScreenPos);

            _panTransform.X += (mouseWorldAfter.X - mouseWorldBefore.X) * _zoomTransform.ScaleX;
            _panTransform.Y += (mouseWorldAfter.Y - mouseWorldBefore.Y) * _zoomTransform.ScaleY;
        }

        #region Element Management
        public void AddElement(DrawingElement element)
        {
            if (element.IsAnnotation == true)
            {
                ScreenElements.Add(element);
                if (element.Visual is GeometryVisual gv)
                {
                    gv.Scale = 20;
                }
            }

            else
                ModelElements.Add(element);

            _contentCanvas.Children.Add(element);
            Canvas.SetZIndex(element, element.IsAnnotation == true ? 1 : 0);  
        }

        public void ZoomToFitAll()
        {
            foreach (var element in ModelElements)
            {
                Bounding2 bounds = element.BoundingXY;

                double scaleX = ActualWidth / bounds.Width;
                double scaleY = ActualHeight / bounds.Height;

                double targetScale = Math.Min(scaleX, scaleY) * 0.9;
                _zoomTransform.ScaleX = targetScale;
                _zoomTransform.ScaleY = targetScale;
                _panTransform.X = -((bounds.Min.X + bounds.Max.X) / 2) * targetScale + ActualWidth / 2;
                _panTransform.Y = -((bounds.Min.Y + bounds.Max.Y) / 2) * targetScale + ActualHeight / 2;
            }
        }
        #endregion
    }
}

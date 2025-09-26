using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Paftax.Pafta.UI.AttachedProperties
{
    internal static class CanvasPan
    {
        public static readonly DependencyProperty IsEnabledProperty =
            DependencyProperty.RegisterAttached(
                "IsEnabled",
                typeof(bool),
                typeof(CanvasPan),
                new PropertyMetadata(false, OnIsEnabledChanged));
        public static void SetIsEnabled(UIElement element, bool value) =>
            element.SetValue(IsEnabledProperty, value);

        public static bool GetIsEnabled(UIElement element) =>
            (bool)element.GetValue(IsEnabledProperty);

        private static void OnIsEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Canvas canvas)
            {
                if ((bool)e.NewValue)
                {
                    canvas.MouseDown += Canvas_MouseDown;
                    canvas.MouseMove += Canvas_MouseMove;
                    canvas.MouseUp += Canvas_MouseUp;
                    canvas.Cursor = Cursors.Hand;
                }
                else
                {
                    canvas.MouseDown -= Canvas_MouseDown;
                    canvas.MouseMove -= Canvas_MouseMove;
                    canvas.MouseUp -= Canvas_MouseUp;
                    canvas.Cursor = Cursors.Arrow;
                }
            }
        }

        private static Point _start;
        private static bool _isDragging = false;

        private static void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Canvas canvas && e.ChangedButton == MouseButton.Left)
            {
                _start = e.GetPosition(canvas);
                _isDragging = true;
                canvas.CaptureMouse();
            }
        }

        private static void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isDragging && sender is Canvas canvas)
            {
                var pos = e.GetPosition(canvas);
                var dx = pos.X - _start.X;
                var dy = pos.Y - _start.Y;

                foreach (UIElement child in canvas.Children)
                {
                    Canvas.SetLeft(child, Canvas.GetLeft(child) + dx);
                    Canvas.SetTop(child, Canvas.GetTop(child) + dy);
                }

                _start = pos;
            }
        }

        private static void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (sender is Canvas canvas)
            {
                _isDragging = false;
                canvas.ReleaseMouseCapture();
            }
        }
    }

}

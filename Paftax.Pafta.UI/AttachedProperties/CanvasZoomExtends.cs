using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Paftax.Pafta.UI.AttachedProperties
{
    internal static class CanvasZoomExtends
    {
        public static readonly DependencyProperty ZoomToFitProperty =
            DependencyProperty.RegisterAttached(
                "ZoomToFit",
                typeof(bool),
                typeof(CanvasZoomExtends),
                new PropertyMetadata(false, OnZoomToFitChanged));

        public static void SetZoomToFit(UIElement element, bool value) =>
            element.SetValue(ZoomToFitProperty, value);

        public static bool GetZoomToFit(UIElement element) =>
            (bool)element.GetValue(ZoomToFitProperty);

        private static void OnZoomToFitChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Canvas canvas && (bool)e.NewValue)
            {
                ZoomToFit(canvas);
            }
        }

        public static void ZoomToFit(Canvas canvas, double margin = 20)
        {
            if (canvas == null || canvas.Children.Count == 0)
                return;

            double minX = double.MaxValue;
            double minY = double.MaxValue;
            double maxX = double.MinValue;
            double maxY = double.MinValue;

            foreach (UIElement child in canvas.Children)
            {
                double left = Canvas.GetLeft(child);
                double top = Canvas.GetTop(child);
                double right = left + (child.RenderSize.Width);
                double bottom = top + (child.RenderSize.Height);

                if (left < minX) minX = left;
                if (top < minY) minY = top;
                if (right > maxX) maxX = right;
                if (bottom > maxY) maxY = bottom;
            }

            double canvasWidth = canvas.ActualWidth - 2 * margin;
            double canvasHeight = canvas.ActualHeight - 2 * margin;

            double contentWidth = maxX - minX;
            double contentHeight = maxY - minY;

            if (contentWidth == 0 || contentHeight == 0)
                return;

            double scaleX = canvasWidth / contentWidth;
            double scaleY = canvasHeight / contentHeight;

            double scale = Math.Min(scaleX, scaleY);

            if (canvas.RenderTransform is not ScaleTransform st)
            {
                st = new ScaleTransform(1.0, 1.0);
                canvas.RenderTransform = st;
            }

            st.ScaleX = scale;
            st.ScaleY = scale;

            double offsetX = -minX * scale + margin;
            double offsetY = -minY * scale + margin;

            canvas.RenderTransformOrigin = new Point(0, 0);

            var tg = new TransformGroup();
            tg.Children.Add(st);
            tg.Children.Add(new TranslateTransform(offsetX, offsetY));
            canvas.RenderTransform = tg;
        }
    }
}

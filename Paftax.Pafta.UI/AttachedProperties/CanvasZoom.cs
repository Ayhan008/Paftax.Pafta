using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Paftax.Pafta.UI.AttachedProperties
{
    internal static class CanvasZoom
    {
        public static readonly DependencyProperty IsEnabledProperty =
            DependencyProperty.RegisterAttached(
                "IsEnabled",
                typeof(bool),
                typeof(CanvasZoom),
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
                    EnsureTransforms(canvas);
                    canvas.PreviewMouseWheel += Canvas_PreviewMouseWheel;
                }
                else
                {
                    canvas.PreviewMouseWheel -= Canvas_PreviewMouseWheel;
                }
            }
        }

        private static void Canvas_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (sender is not Canvas canvas) return;
            if (canvas.RenderTransform is not TransformGroup tg) return;

            // Remove Alt requirement so zoom feels natural
            double factor = e.Delta > 0 ? 1.1 : 0.9;

            var scale = (ScaleTransform)tg.Children[0];
            var translate = (TranslateTransform)tg.Children[1];

            var pointer = e.GetPosition(canvas);

            // Convert pointer to content coordinates
            double contentX = (pointer.X - translate.X) / scale.ScaleX;
            double contentY = (pointer.Y - translate.Y) / scale.ScaleY;

            scale.ScaleX *= factor;
            scale.ScaleY *= factor;

            // Keep pointer position stable
            translate.X = pointer.X - contentX * scale.ScaleX;
            translate.Y = pointer.Y - contentY * scale.ScaleY;

            e.Handled = true;
        }

        private static void EnsureTransforms(Canvas canvas)
        {
            if (canvas.RenderTransform is TransformGroup tg &&
                tg.Children.Count == 2 &&
                tg.Children[0] is ScaleTransform &&
                tg.Children[1] is TranslateTransform)
                return;

            var group = new TransformGroup();
            group.Children.Add(new ScaleTransform(1, 1));
            group.Children.Add(new TranslateTransform());
            canvas.RenderTransform = group;
        }
    }
}

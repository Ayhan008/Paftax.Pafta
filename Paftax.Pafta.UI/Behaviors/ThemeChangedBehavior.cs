using System.Windows;

namespace Paftax.Pafta.UI.Behaviors
{
    public static class ThemeChangedBehavior
    {
        public static readonly DependencyProperty EnableThemeProperty =
            DependencyProperty.RegisterAttached(
                "EnableTheme",
                typeof(bool),
                typeof(ThemeChangedBehavior),
                new PropertyMetadata(false, OnEnableThemeChanged));

        public static bool GetEnableTheme(DependencyObject obj) => (bool)obj.GetValue(EnableThemeProperty);
        public static void SetEnableTheme(DependencyObject obj, bool value) => obj.SetValue(EnableThemeProperty, value);

        private static void OnEnableThemeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FrameworkElement fe && (bool)e.NewValue)
            {
                UIThemeService.ApplyTheme(UIThemeController.CurrentTheme, fe);

                fe.Loaded += (s, args) =>
                {
                    UIThemeService.ApplyTheme(UIThemeController.CurrentTheme, fe);
                };

                UIThemeController.ThemeChanged += theme =>
                {
                    UIThemeService.ApplyTheme(theme, fe);
                };

                fe.Unloaded += (s, args) =>
                {
                    UIThemeController.ThemeChanged -= theme => UIThemeService.ApplyTheme(theme, fe);
                };
            }
        }
    }
}

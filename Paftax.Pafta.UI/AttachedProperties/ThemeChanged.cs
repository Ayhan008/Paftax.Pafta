using Paftax.Pafta.UI.Services;
using System.Windows;

namespace Paftax.Pafta.UI.AttachedProperties
{
    public static class ThemeChanged
    {
        public static readonly DependencyProperty EnableThemeProperty =
            DependencyProperty.RegisterAttached(
                "EnableTheme",
                typeof(bool),
                typeof(ThemeChanged),
                new PropertyMetadata(false, OnEnableThemeChanged));
        public static bool GetEnableTheme(DependencyObject obj) => (bool)obj.GetValue(EnableThemeProperty);
        public static void SetEnableTheme(DependencyObject obj, bool value) => obj.SetValue(EnableThemeProperty, value);
        private static void OnEnableThemeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FrameworkElement fe && (bool)e.NewValue)
            {
                ThemeService.ApplyTheme(UI.Theme.CurrentTheme, fe);

                fe.Loaded += (s, args) =>
                {
                    ThemeService.ApplyTheme(UI.Theme.CurrentTheme, fe);
                };

                UI.Theme.ThemeChanged += theme =>
                {
                    ThemeService.ApplyTheme(theme, fe);
                };

                fe.Unloaded += (s, args) =>
                {
                    UI.Theme.ThemeChanged -= theme => ThemeService.ApplyTheme(theme, fe);
                };
            }
        }
    }
}

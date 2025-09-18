using System.Windows;

namespace Paftax.Pafta.UI
{
    public static class WindowThemeService
    {
        private const string DarkThemePath = "pack://application:,,,/PaftaxPaftaUI;component/Resources/Themes/Dark.xaml";
        private const string LightThemePath = "pack://application:,,,/PaftaxPaftaUI;component/Resources/Themes/Light.xaml";

        /// <summary>
        /// Configures and applies the specified theme to the given window.
        /// </summary>
        /// <param name="window"></param>
        /// <param name="theme"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void ApplyTheme(Window window, string theme)
        {
            if (window == null) throw new ArgumentNullException(nameof(window));

            string path = theme.Equals("Dark", StringComparison.OrdinalIgnoreCase)
                ? DarkThemePath
                : LightThemePath;

            var newDict = new ResourceDictionary { Source = new Uri(path, UriKind.Absolute) };

            window.Loaded += (s, e) =>
            {
                var oldThemes = window.Resources.MergedDictionaries
                    .Where(d => d.Source?.ToString() == DarkThemePath || d.Source?.ToString() == LightThemePath)
                    .ToList();

                foreach (var old in oldThemes)
                    window.Resources.MergedDictionaries.Remove(old);

                // Yeni temayý ekle
                window.Resources.MergedDictionaries.Add(newDict);
            };
        }
    }
}

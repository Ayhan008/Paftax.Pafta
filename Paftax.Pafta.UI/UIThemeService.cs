using System.Windows;

namespace Paftax.Pafta.UI
{
    public static class UIThemeService
    {
        private const string _assemblyName = "PaftaxPaftaUI";
        private static readonly string _darkThemePath = $"pack://application:,,,/{_assemblyName};component/Resources/Themes/Dark.xaml";
        private static readonly string _lightThemePath = $"pack://application:,,,/{_assemblyName};component/Resources/Themes/Light.xaml";

        /// <summary>
        /// Apply Theme.
        /// </summary>
        /// <param name="window"></param>
        /// <param name="theme"></param>
        public static void ApplyThemeToWindow(Window window, string theme)
        {
            string dictionaryPath = theme.Equals("Dark", StringComparison.OrdinalIgnoreCase)
                ? _darkThemePath
                : _lightThemePath;

            var dict = new ResourceDictionary { Source = new Uri(dictionaryPath, UriKind.Absolute) };

            var existingTheme = window.Resources.MergedDictionaries
                .FirstOrDefault(d => d.Source != null &&
                                     (d.Source.ToString().EndsWith("Dark.xaml", StringComparison.OrdinalIgnoreCase) ||
                                      d.Source.ToString().EndsWith("Light.xaml", StringComparison.OrdinalIgnoreCase)));

            if (existingTheme != null)
                window.Resources.MergedDictionaries.Remove(existingTheme);

            window.Resources.MergedDictionaries.Add(dict);
        }

        public static void ApplyThemeToApplication(string theme)
        {
            string dictionaryPath = theme.Equals("Dark", StringComparison.OrdinalIgnoreCase)
                ? _darkThemePath
                : _lightThemePath;

            var dict = new ResourceDictionary { Source = new Uri(dictionaryPath, UriKind.Absolute) };
            var existingTheme = Application.Current.Resources.MergedDictionaries
                .FirstOrDefault(d => d.Source != null &&
                                     (d.Source.ToString().EndsWith("Dark.xaml", StringComparison.OrdinalIgnoreCase) ||
                                      d.Source.ToString().EndsWith("Light.xaml", StringComparison.OrdinalIgnoreCase)));

            if (existingTheme != null)
                Application.Current.Resources.MergedDictionaries.Remove(existingTheme);

            Application.Current.Resources.MergedDictionaries.Add(dict);
        }
    }
}

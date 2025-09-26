using System.Windows;

namespace Paftax.Pafta.UI.Services
{
    public static class ThemeService
    {
        private const string _assemblyName = "PaftaxPaftaUI";
        private static readonly string _darkThemePath = $"pack://application:,,,/{_assemblyName};component/Resources/Themes/Dark.xaml";
        private static readonly string _lightThemePath = $"pack://application:,,,/{_assemblyName};component/Resources/Themes/Light.xaml";

        /// <summary>
        /// Apply theme to Application and optionally a specific FrameworkElement (Window, UserControl, etc.)
        /// </summary>
        public static void ApplyTheme(string theme, FrameworkElement? element = null)
        {
            string dictionaryPath = theme.Equals("Dark", StringComparison.OrdinalIgnoreCase)
                ? _darkThemePath
                : _lightThemePath;

            var dict = new ResourceDictionary { Source = new Uri(dictionaryPath, UriKind.Absolute) };

            // Application resources
            var existingAppTheme = Application.Current.Resources.MergedDictionaries
                .FirstOrDefault(d => d.Source != null &&
                                     (d.Source.ToString().EndsWith("Dark.xaml", StringComparison.OrdinalIgnoreCase) ||
                                      d.Source.ToString().EndsWith("Light.xaml", StringComparison.OrdinalIgnoreCase)));
            if (existingAppTheme != null)
                Application.Current.Resources.MergedDictionaries.Remove(existingAppTheme);
            Application.Current.Resources.MergedDictionaries.Add(dict);

            if (element != null)
            {
                ApplyThemeToElement(element, dict);
            }
            else
            {
                foreach (Window window in Application.Current.Windows)
                {
                    ApplyThemeToElement(window, dict);
                }
            }
        }

        private static void ApplyThemeToElement(FrameworkElement element, ResourceDictionary dict)
        {
            if (element == null) return;

            var existingTheme = element.Resources.MergedDictionaries
                .FirstOrDefault(d => d.Source != null &&
                                     (d.Source.ToString().EndsWith("Dark.xaml", StringComparison.OrdinalIgnoreCase) ||
                                      d.Source.ToString().EndsWith("Light.xaml", StringComparison.OrdinalIgnoreCase)));
            if (existingTheme != null)
                element.Resources.MergedDictionaries.Remove(existingTheme);

            element.Resources.MergedDictionaries.Add(dict);
        }
    }
}

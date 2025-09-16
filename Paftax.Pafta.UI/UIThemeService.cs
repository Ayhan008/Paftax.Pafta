using System.Windows;

namespace Paftax.Pafta.UI
{
    public class UIThemeService
    {
        /// <summary>
        /// Changes the application theme by swapping ResourceDictionaries.
        /// </summary>
        public static void ApplyTheme(string theme)
        {
            string dictionaryPath = theme.Equals("Dark", StringComparison.OrdinalIgnoreCase)
                ? "pack://application:,,,/Paftax.Pafta.UI;component/Resources/Theme/Dark.xaml"
                : "pack://application:,,,/Paftax.Pafta.UI;component/Resources/Theme/Light.xaml";

            ResourceDictionary dict = new()
            {
                Source = new Uri(dictionaryPath, UriKind.Absolute)
            };

            var oldDict = Application.Current.Resources.MergedDictionaries.Count > 0
                ? Application.Current.Resources.MergedDictionaries[0]
                : null;

            if (oldDict != null)
                Application.Current.Resources.MergedDictionaries.Remove(oldDict);

            Application.Current.Resources.MergedDictionaries.Add(dict);
        }
    }
}

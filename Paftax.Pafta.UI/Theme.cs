namespace Paftax.Pafta.UI
{
    public static class Theme
    {
        public static event Action<string>? ThemeChanged;
        private static string _currentTheme = "Dark";
        public static string CurrentTheme
        {
            get => _currentTheme;
            private set => _currentTheme = value;
        }

        public static void SetTheme(string theme)
        {
            if (string.IsNullOrEmpty(theme)) return;
            if (string.Equals(_currentTheme, theme, StringComparison.OrdinalIgnoreCase)) return;

            _currentTheme = theme;
            ThemeChanged?.Invoke(theme);
        }
    }
}
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Events;
using Paftax.Pafta.Shared;
using Paftax.Pafta.UI;
using System.Diagnostics;
using System.Windows.Media.Imaging;

namespace Paftax.Pafta.Revit2026.Services
{
    internal class ThemeService(UIControlledApplication application, string tabName)
    {
        private readonly List<RibbonItem> _ribbonItems = [];
        private readonly string _resourcesFolder = @"C:\Program Files\Paftax\Revit Addins\Pafta\Shared Resources";

        /// <summary>
        /// Initializes the service: collects ribbon items for all tabs and subscribes to theme changes.
        /// </summary>
        public void Initialize()
        {
            CollectAllRibbonItems();
            UpdateRibbonItemImages();
            Theme.SetTheme(GetThemeString());
        }

        /// <summary>
        /// Event handler called when the UI theme changes.
        /// </summary>
        public void OnThemeChanged(object? sender, ThemeChangedEventArgs e)
        {
            UpdateRibbonItemImages();
            Theme.SetTheme(GetThemeString());
        }

        #region Ribbon Item Management
        /// <summary>
        /// Collects all ribbon items from all tabs.
        /// </summary>
        private void CollectAllRibbonItems()
        {
            _ribbonItems.Clear();

            foreach (RibbonPanel panel in application.GetRibbonPanels(tabName))
            {
                foreach (RibbonItem item in panel.GetItems())
                {
                    if (item != null)
                        _ribbonItems.Add(item);
                }
            }

        }

        public static string GetThemeString()
        {
            UITheme theme = UIThemeManager.CurrentTheme;
            string themeString = theme == UITheme.Dark ? "Dark" : "Light";
            return themeString;
        }

        /// <summary>
        /// Updates the images of ribbon items based on the current UI theme.
        /// </summary>
        private void UpdateRibbonItemImages()
        {
            UITheme theme = UIThemeManager.CurrentTheme;
            string themeString = theme == UITheme.Dark ? "Dark" : "Light";

            foreach (RibbonItem ribbonItem in _ribbonItems)
            {
                if (ribbonItem is PushButton pushButton)
                {
                    string pushButtonName = pushButton.Name;
                    string imagePath = Path.Combine(_resourcesFolder, $"{pushButtonName}_{themeString}.tiff");

                    if (File.Exists(imagePath))
                    {
                        try
                        {
                            BitmapImage bitmap = new();
                            bitmap.BeginInit();
                            bitmap.UriSource = new Uri(imagePath, UriKind.Absolute);
                            bitmap.CacheOption = BitmapCacheOption.OnLoad;
                            bitmap.EndInit();
                            bitmap.Freeze();

                            pushButton.LargeImage = bitmap;
                            pushButton.Image = bitmap;
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine($"Error loading image for {pushButtonName}: {ex.Message}");
                        }
                    }
                }
            }
        }
        #endregion
    }
}

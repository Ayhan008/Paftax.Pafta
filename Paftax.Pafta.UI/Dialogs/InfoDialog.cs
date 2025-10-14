using Paftax.Pafta.Shared.Enums;
using Paftax.Pafta.UI.ViewModels;

namespace Paftax.Pafta.UI.Dialogs
{
    public static class InfoDialog
    {
        public static void Show(string title, string message, IconType iconType = IconType.Success, int width = 450, int height = 300)
        {
            InfoDialogViewModel viewModel = new()
            {
                Message = message,
                IconType = iconType
            };

            MainWindow mainWindow = new()
            {
                Title = title,
                Width = width,
                Height = height,
                DataContext = viewModel,
                ShowHelpButton = false,
                ShowCloseButton = true,
                ShowMaximizeButton = false,
                ShowMinimizeButton = false,
            };

            viewModel.CloseAction += mainWindow.Close;
            mainWindow.ShowDialog();     
        }
    }
}

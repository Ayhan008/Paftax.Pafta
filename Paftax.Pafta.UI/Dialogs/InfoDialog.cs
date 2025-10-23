using Paftax.Pafta.Shared.Enums;
using Paftax.Pafta.UI.Services;
using Paftax.Pafta.UI.ViewModels;

namespace Paftax.Pafta.UI.Dialogs
{
    public static class InfoDialog
    {
        public static void Show(string message, DialogOptions dialogOptions, IconType iconType = IconType.Success)
        {
            InfoDialogViewModel viewModel = new()
            {
                Message = message,
                IconType = iconType
            };

            MainWindow mainWindow = new()
            {
                Title = dialogOptions.Title,
                Width = dialogOptions.Width,
                Height = dialogOptions.Height,
                DataContext = viewModel,
                ShowCloseButton = true,
                ShowHelpButton = dialogOptions.ShowHelpButton,
                ShowMaximizeButton = dialogOptions.ShowMaximizeButton,
                ShowMinimizeButton = dialogOptions.ShowMinimizeButton
            };

            viewModel.CloseAction += mainWindow.Close;
            mainWindow.ShowDialog();
        }

        public static void Show(string title, string message, IconType iconType = IconType.Success)
        {
            DialogOptions dialogOptions = new()
            {
                Title = title,
                Width = 500,
                Height = 300,
                ShowHelpButton = false,
                ShowCloseButton = true,
                ShowMaximizeButton = false,
                ShowMinimizeButton = false,
            };
            Show(message, dialogOptions, iconType);
        }
    }
}

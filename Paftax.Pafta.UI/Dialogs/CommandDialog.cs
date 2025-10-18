using CommunityToolkit.Mvvm.ComponentModel;
using Paftax.Pafta.Shared.Interfaces;
using Paftax.Pafta.UI.Services;
using System.Windows;

namespace Paftax.Pafta.UI.Dialogs
{
    public static class CommandDialog
    {
        private static readonly Dictionary<Type, Window> OpenWindows = [];

        public static void Show(ObservableObject viewModel)
        {
            DialogOptions dialogOptions = new()
            {
                Title = "Info Dialog",
                Width = 400,
                Height = 300,
                ShowCloseButton = true,
                ShowHelpButton = false,
                ShowMaximizeButton = false,
                ShowMinimizeButton = false
            };

            Show(viewModel, dialogOptions);
        }

        public static void Show(ObservableObject viewModel, DialogOptions dialogOptions)
        {
            Type vmType = viewModel.GetType();

            if (OpenWindows.TryGetValue(vmType, out var existingWindow))
            {
                existingWindow.Activate();
                existingWindow.Topmost = true;
                existingWindow.Topmost = false;
                return;
            }

            var baseWindow = new MainWindow
            {
                DataContext = viewModel,
                Title = dialogOptions.Title,
                Width = dialogOptions.Width,
                Height = dialogOptions.Height,
                ShowCloseButton = true,
                ShowHelpButton = dialogOptions.ShowHelpButton,
                ShowMaximizeButton = dialogOptions.ShowMaximizeButton,
                ShowMinimizeButton = dialogOptions.ShowMinimizeButton,
                Topmost = true
            };

            baseWindow.Closed += (s, e) =>
            {
                OpenWindows.Remove(vmType);
            };

            if (viewModel is ICloseable closeableViewModel)
            {
                closeableViewModel.CloseAction += baseWindow.Close;
            }

            OpenWindows[vmType] = baseWindow;

            if (dialogOptions.Async == false)
                baseWindow.ShowDialog();
            else
                baseWindow.Show();
        }
    }
}

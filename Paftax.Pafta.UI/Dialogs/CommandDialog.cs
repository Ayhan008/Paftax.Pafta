using CommunityToolkit.Mvvm.ComponentModel;
using Paftax.Pafta.Shared.Interfaces;

namespace Paftax.Pafta.UI.Dialogs
{
    public static class CommandDialog<TViewModel> where TViewModel : ObservableObject, new()
    {
        public static void Show(string title, int width, int height)
        {
            var viewModel = new TViewModel();
            Show(viewModel, title, width, height);
        }

        public static void Show(TViewModel viewModel, string title, int width, int height)
        {
            var baseWindow = new MainWindow
            {
                DataContext = viewModel,
                Title = title,
                Width = width,
                Height = height
            };

            if (viewModel is ICloseable closeableViewModel)
            {
                closeableViewModel.CloseAction += baseWindow.Close;
            }

            baseWindow.ShowDialog();
        }

        public static void ShowAsync(TViewModel viewModel, string title, int width, int height)
        {
            var baseWindow = new MainWindow
            {
                DataContext = viewModel,
                Title = title,
                Width = width,
                Height = height
            };

            if (viewModel is ICloseable closeableViewModel)
            {
                closeableViewModel.CloseAction += baseWindow.Close;
            }

            baseWindow.Show();
        }
    }
}

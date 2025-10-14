using CommunityToolkit.Mvvm.ComponentModel;
using Paftax.Pafta.UI.ViewModels;

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
            baseWindow.Show();
        }
    }
}

using CommunityToolkit.Mvvm.ComponentModel;
using Paftax.Pafta.UI.ViewModels;

namespace Paftax.Pafta.UI.Services
{
    public class DialogService<TViewModel>(TViewModel viewModel) where TViewModel : ObservableObject
    {
        private readonly TViewModel _viewModel = viewModel;
        public void ShowDialog(string title, int width, int height)
        {
            MainViewModel mainViewModel = new(_viewModel);
  
            MainWindow baseWindow = new()
            {
                DataContext = mainViewModel,
                Title = title,
                Width = width,
                Height = height
            };
            baseWindow.ShowDialog();
        }
        public void ShowDialogAsync(string title, int width, int height)
        {
            MainViewModel mainViewModel = new(_viewModel);
            MainWindow baseWindow = new()
            {
                DataContext = mainViewModel,
                Title = title,
                Width = width,
                Height = height
            };
            baseWindow.Show();
        }
    }
}

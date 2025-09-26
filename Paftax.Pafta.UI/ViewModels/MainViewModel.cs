using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Paftax.Pafta.Shared.Interfaces;

namespace Paftax.Pafta.UI.ViewModels
{
    public partial class MainViewModel(object viewModel) : ObservableObject, IBaseViewModel
    {
        [ObservableProperty]
        private object? currentView = viewModel;

        public event Action? CloseRequest;

        [RelayCommand]
        private void Close()
        {
            CloseRequest?.Invoke();
        }
    }
}

using CommunityToolkit.Mvvm.ComponentModel;

namespace Paftax.Pafta.UI.ViewModels
{
    public partial class MainWindowViewModel : BaseViewModel
    {
        [ObservableProperty]
        private BaseViewModel? currentViewModel;
    }
}

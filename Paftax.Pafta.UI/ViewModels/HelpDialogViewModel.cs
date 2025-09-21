using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Paftax.Pafta.UI.ViewModels
{
    public partial class HelpDialogViewModel : BaseViewModel
    {
        [RelayCommand]
        private void Ok()
        {
            OnCloseRequest();
        }
    }
}

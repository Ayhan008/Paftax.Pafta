using CommunityToolkit.Mvvm.ComponentModel;

namespace Paftax.Pafta.UI.ViewModels.Abstracts
{
    public abstract class ViewModel : ObservableObject
    {
        public Action? CloseAction;
    }
}

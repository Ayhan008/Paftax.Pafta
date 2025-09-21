using CommunityToolkit.Mvvm.ComponentModel;

namespace Paftax.Pafta.UI.ViewModels
{
    public abstract partial class BaseViewModel : ObservableObject
    {
        [ObservableProperty]
        private string title = "Base View";

        [ObservableProperty]
        private string helpText = string.Empty;

        public event Action? CloseRequest;

        protected void OnCloseRequest()
        {
            CloseRequest?.Invoke();
        }
    }
}

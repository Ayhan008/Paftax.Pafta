using CommunityToolkit.Mvvm.ComponentModel;

namespace Paftax.Pafta.UI.ViewModels
{
    public abstract class BaseViewModel : ObservableObject
    {
        public event Action? CancelRequest;

        protected void OnCancelRequest()
        {
            CancelRequest?.Invoke();
        }
    }
}

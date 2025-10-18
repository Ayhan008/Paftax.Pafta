using CommunityToolkit.Mvvm.ComponentModel;

namespace Paftax.Pafta.UI.Test.ViewModels
{
    public partial class ProgressRingDemoViewModel : ObservableObject
    {
        [ObservableProperty]
        private double _value;
        [ObservableProperty]
        private bool _isIndeterminate = false;
    }
}

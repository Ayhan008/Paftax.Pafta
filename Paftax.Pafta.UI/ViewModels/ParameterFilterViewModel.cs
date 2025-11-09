using CommunityToolkit.Mvvm.ComponentModel;

namespace Paftax.Pafta.UI.ViewModels
{
    public partial class ParameterFilterViewModel : ObservableObject
    {
        public required ParameterFilterViewModel Model { get; init; }
        public string Name => Model.Name;
        public int TemplateCount => Model.TemplateCount;
        public bool IsActive => Model.IsActive;

        [ObservableProperty] private bool isChecked;
    }
}

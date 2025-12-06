using CommunityToolkit.Mvvm.ComponentModel;
using Paftax.Pafta.Shared.Models.Element;

namespace Paftax.Pafta.UI.ViewModels
{
    public partial class SelectableElementViewModel<T>(T model) : ObservableObject where T : ElementModel
    {
        public T Model { get; } = model;

        [ObservableProperty] private bool isChecked;
    }
}
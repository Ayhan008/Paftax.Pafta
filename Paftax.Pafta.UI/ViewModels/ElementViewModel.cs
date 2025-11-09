using CommunityToolkit.Mvvm.ComponentModel;
using Paftax.Pafta.Shared.Models;

namespace Paftax.Pafta.UI.ViewModels
{
    public partial class ElementViewModel(ElementModel model) : ObservableObject
    {
        public ElementModel Model { get; } = model;
        public string Name => Model.Name;

        [ObservableProperty] private bool isChecked;
    }
}
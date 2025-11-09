using CommunityToolkit.Mvvm.ComponentModel;
using Paftax.Pafta.Shared.Models;

namespace Paftax.Pafta.UI.ViewModels
{
    public partial class GarbageElementViewModel(GarbageElementModel model) : ObservableObject
    {
        public GarbageElementModel Model { get; } = model;
        public string Name => Model.Name;
        public int Count => Model.Count;
        public bool IsUsed => Model.IsUsed;

        [ObservableProperty] private bool isChecked;
    }
}
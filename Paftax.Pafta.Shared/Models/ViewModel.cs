using CommunityToolkit.Mvvm.ComponentModel;

namespace Paftax.Pafta.Shared.Models
{
    public partial class ViewModel : ObservableObject
    {
        public required long Id { get; set; }
        public required string Name { get; set; }
        public bool IsPlaced { get; set; }
        public string ViewType { get; set; } = "Unknown";
        public string IsPlacedGlyph => IsPlaced ? "\uf5ee" : "\uf5ed";

        [ObservableProperty]
        private bool isChecked;
    }
}

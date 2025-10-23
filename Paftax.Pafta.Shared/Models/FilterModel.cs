using CommunityToolkit.Mvvm.ComponentModel;

namespace Paftax.Pafta.Shared.Models
{
    public partial class FilterModel : ObservableObject
    {
        public required string Name { get; set; }
        public required long Id { get; set; }
        public bool IsActive { get; set; }
        public int TemplateCount { get; set; }

        [ObservableProperty]
        private bool isChecked;
    }
}

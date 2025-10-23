using CommunityToolkit.Mvvm.ComponentModel;
using Paftax.Pafta.Shared.Interfaces;

namespace Paftax.Pafta.Shared.Models
{
    public partial class ViewTemplateModel : ObservableObject
    {
        public required string Name { get; set; }
        public required long Id { get; set; }
        public bool IsActive { get; set; }
        public int ViewCount { get; set; }

        [ObservableProperty]
        private bool isChecked;
    }
}

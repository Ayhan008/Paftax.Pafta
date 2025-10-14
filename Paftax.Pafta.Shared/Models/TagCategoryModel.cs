using CommunityToolkit.Mvvm.ComponentModel;

namespace Paftax.Pafta.Shared.Models
{
    public partial class TagCategoryModel : ObservableObject
    {
        public int Count { get; set; }
        public string Category { get; set; } = string.Empty;
        public List<TagModel> Tags { get; set; } = [];

        [ObservableProperty]
        private bool isChecked;
    }
}

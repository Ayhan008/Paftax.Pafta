using CommunityToolkit.Mvvm.ComponentModel;
using Paftax.Pafta.Shared.Interfaces;

namespace Paftax.Pafta.Shared.Models
{
    public partial class TagCategoryModel : ObservableObject, IUserCheckable
    {
        public int Count { get; set; }
        public string Category { get; set; } = string.Empty;
        public List<TagModel> Tags { get; set; } = [];

        [ObservableProperty]
        private bool isChecked;
    }
}

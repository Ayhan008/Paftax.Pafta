using CommunityToolkit.Mvvm.ComponentModel;

namespace Paftax.Pafta.Shared.Models
{
    public partial class TagModel : ObservableObject
    {
        public required long Id { get; set; }
        public string TagCategory { get; set; } = string.Empty;
        public bool IsOrphaned { get; set; }
    }
}

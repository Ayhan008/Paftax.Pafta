using CommunityToolkit.Mvvm.ComponentModel;
using Paftax.Pafta.Shared.Interfaces;

namespace Paftax.Pafta.Shared.Models
{
    public partial class TagModel : ObservableObject, IElement
    {
        public required long Id { get; set; }
        public string TagCategory { get; set; } = string.Empty;
        public bool IsOrphaned { get; set; }
    }
}

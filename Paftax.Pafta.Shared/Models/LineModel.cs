using CommunityToolkit.Mvvm.ComponentModel;
using Paftax.Pafta.Shared.Interfaces;

namespace Paftax.Pafta.Shared.Models
{
    public partial class LineModel : ObservableObject, IUserCheckable
    {
        public required long Id { get; set; }
        public required string Name { get; set; }
        public int Count { get; set; }

        [ObservableProperty]
        private bool isChecked;
    }
}

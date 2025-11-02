using System.Drawing;

namespace Paftax.Pafta.UI.Test.Models
{
    internal class ElevationMarkerModel
    {
        public Point Location { get; set; }
        public ViewModel? NorthElevation { get; set; }
        public ViewModel? SouthElevation { get; set; }
        public ViewModel? EastElevation { get; set; }
        public ViewModel? WestElevation { get; set; }

    }
}

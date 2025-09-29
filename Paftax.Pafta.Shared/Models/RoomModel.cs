using Paftax.Pafta.Drawing.Elements;

namespace Paftax.Pafta.Shared.Models
{
    public class RoomModel
    {
        public required Room RoomGeometry { get; set; }
        public required string Name { get; set; }
        public required string Number { get; set; }
    }
}

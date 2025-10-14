using Paftax.Pafta.Drawing.Entities;

namespace Paftax.Pafta.Shared.Models
{
    public class RoomModel
    {
        public required RoomGeometry RoomGeometry { get; set; }
        public required string Name { get; set; }
        public required string Number { get; set; }
    }
}

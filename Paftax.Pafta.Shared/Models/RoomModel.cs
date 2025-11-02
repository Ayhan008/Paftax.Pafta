using Paftax.Pafta.Drawing.Elements;

namespace Paftax.Pafta.Shared.Models
{
    public class RoomModel
    {
        public SpatialBoundary? Boundary { get; set; }
        public required string Name { get; set; }
        public required string Number { get; set; }
        public long LevelId { get; set; }
        public long Id { get; set; }
    }
}

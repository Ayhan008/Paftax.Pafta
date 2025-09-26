using System.Drawing;

namespace Paftax.Pafta.Shared.Models
{
    public class RoomGeometryData
    {
        public List<PointF> GeomertyPoints { get; set; } = [];
        public float Area { get; set; }
        public float Volume { get; set; }
    }
}

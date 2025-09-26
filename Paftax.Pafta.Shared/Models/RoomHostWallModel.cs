using System.Drawing;
using System.Numerics;

namespace Paftax.Pafta.Shared.Models
{
    public class RoomHostWallModel
    {
        public required long HostBoundarySegmentId { get; set; }
        public required long HostRoomId { get; set; }
        public required long Id { get; set; }
        public double Length { get; set; }
        public double Height { get; set; }
        public Vector2 IntersectedLength { get; set; }
        public Point StartPointOnInsersection { get; set; }
        public Point EndPointOnInsersection { get; set; }
    }
}

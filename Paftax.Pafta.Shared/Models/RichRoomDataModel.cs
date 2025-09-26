namespace Paftax.Pafta.Shared.Models
{
    public class RichRoomDataModel
    {
        public required long Id { get; set; }
        public required string Name { get; set; }
        public required string Number { get; set; }
        public double Area { get; set; }
        public List<RoomHostWallModel> HostWalls { get; set; } = [];
        public RoomGeometryData RoomGeometryData { get; set; } = new();
        public BoundingBoxData BoundingBoxData { get; set; } = new();
    }
}

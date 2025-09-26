namespace Paftax.Pafta.Shared.Models
{
    public class RoomModel
    {
        public required long Id { get; set; }
        public required string Name { get; set; }
        public required int Number { get; set; }
        public int Area { get; set; }
    }
}

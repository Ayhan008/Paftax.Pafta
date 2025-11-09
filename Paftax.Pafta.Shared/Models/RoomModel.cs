namespace Paftax.Pafta.Shared.Models
{
    public class RoomModel : SpatialElementModel
    {
        public double Volume { get; set; }
        public List<CurveModel> Boundaries { get; set; } = [];
    }
}

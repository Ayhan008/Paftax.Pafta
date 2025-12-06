namespace Paftax.Pafta.Shared.Models.Element
{
    public abstract class SpatialElementModel : ElementModel
    {
        public double Area { get; set; }
        public string Number { get; set; } = string.Empty;
        public List<List<BoundarySegment>> BoundarySegments { get; set; } = [];
    }
}

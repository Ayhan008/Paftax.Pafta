using Paftax.Pafta.Shared.Models.Geometry;

namespace Paftax.Pafta.Shared.Models.Element
{
    public class ViewModel : ElementModel
    {
        public required string Title { get; set; }
        public required string ViewType { get; set; } = "Unknown";
        public XYZModel Origin { get; set; } = new();
        public int Scale { get; set; }
        public long ViewTemplateId { get; set; }     
    }
}

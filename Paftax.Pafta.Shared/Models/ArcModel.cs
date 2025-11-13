using Paftax.Pafta.Shared.Geometries;

namespace Paftax.Pafta.Shared.Models
{
    public class ArcModel : CurveModel
    {
        public Point2 Start { get; set; }
        public Point2 End { get; set; }
        public Point2 Center { get; set; }
        public double Radius { get; set; }
    }
}

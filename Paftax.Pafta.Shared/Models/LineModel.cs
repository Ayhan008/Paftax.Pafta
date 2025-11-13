using Paftax.Pafta.Shared.Geometries;

namespace Paftax.Pafta.Shared.Models
{
    public class LineModel : CurveModel
    {
        public Point2 Start { get; set; }
        public Point2 End { get; set; }
    }
}

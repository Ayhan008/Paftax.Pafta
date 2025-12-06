namespace Paftax.Pafta.Shared.Models.Geometry
{
    public class ArcModel : CurveModel
    {
        public double Radius { get; set; }
        public XYZModel? Center { get; set; }
        public XYZModel? Normal { get; set; }
        public XYZModel? XDirection { get; set; }
        public XYZModel? YDirection { get; set; }
        public bool IsClockwise { get; set; }
    }
}

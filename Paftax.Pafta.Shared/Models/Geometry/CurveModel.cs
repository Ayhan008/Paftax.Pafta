namespace Paftax.Pafta.Shared.Models.Geometry
{
    public abstract class CurveModel
    {
        public double Length { get; set; }
        public XYZModel Start { get; set; } = new XYZModel(); //Index 0
        public XYZModel End { get; set; } = new XYZModel(); // Index 1
    }
}

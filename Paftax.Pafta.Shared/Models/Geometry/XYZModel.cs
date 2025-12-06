namespace Paftax.Pafta.Shared.Models.Geometry
{
    public class XYZModel
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public XYZModel? BasisX { get; set; }
        public XYZModel? BasisY { get; set; }
        public XYZModel? BasisZ { get; set; }

    }
}

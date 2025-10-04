using Paftax.Pafta.Drawing.Structs;

namespace Paftax.Pafta.Drawing.Geometries
{
    public sealed class Arc(XY start, XY end, XY center, double radius) : Curve
    {
        public double Radius { get; } = radius;
        public XY Start { get; } = start;
        public XY End { get; } = end;
        public XY Center { get; } = center;
        public override double Length => throw new NotImplementedException();
        public override XY GetEndPoint(int index) => index == 0 ? Start : End;
    }
}

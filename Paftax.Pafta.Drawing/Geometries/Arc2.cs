using Paftax.Pafta.Drawing.Structs;

namespace Paftax.Pafta.Drawing.Geometries
{
    public sealed class Arc2(Point2 start, Point2 end, Point2 center, double radius) : Curve2
    {
        public double Radius { get; } = radius;
        public Point2 Start { get; } = start;
        public Point2 End { get; } = end;
        public Point2 Center { get; } = center;
        public override double Length => throw new NotImplementedException();
        public override Point2 GetEndPoint(int index) => index == 0 ? Start : End;
    }
}

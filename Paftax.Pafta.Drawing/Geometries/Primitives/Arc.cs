namespace Paftax.Pafta.Drawing.Geometries.Primitives
{
    public sealed class Arc(PointXY start, PointXY end, PointXY center, double radius) : Curve
    {
        public double Radius { get; } = radius;
        public PointXY Start { get; } = start;
        public PointXY End { get; } = end;
        public PointXY Center { get; } = center;

        public override double Length => throw new NotImplementedException();

        public override PointXY GetEndPoint(int index) =>
            index == 0 ? Start : End;
    }
}

namespace Paftax.Pafta.Drawing.Geometries.Primitives
{
    public sealed class Line(PointXY start, PointXY end) : Curve
    {
        public PointXY Start { get; } = start;
        public PointXY End { get; } = end;

        public override PointXY GetEndPoint(int index) =>
            index == 0 ? Start : End;

        public override double Length =>
            Math.Sqrt(Math.Pow(End.X - Start.X, 2) +
                      Math.Pow(End.Y - Start.Y, 2));
    }
}

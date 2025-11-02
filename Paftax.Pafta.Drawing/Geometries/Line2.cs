using Paftax.Pafta.Drawing.Structs;

namespace Paftax.Pafta.Drawing.Geometries
{
    public sealed class Line2(Point2 start, Point2 end) : Curve2
    {
        public Point2 Start { get; } = start;
        public Point2 End { get; } = end;

        public override Point2 GetEndPoint(int index) =>
            index == 0 ? Start : End;

        public override double Length =>
            Math.Sqrt(Math.Pow(End.X - Start.X, 2) +
                      Math.Pow(End.Y - Start.Y, 2));
    }
}

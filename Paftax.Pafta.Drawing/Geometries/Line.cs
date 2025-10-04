using Paftax.Pafta.Drawing.Structs;

namespace Paftax.Pafta.Drawing.Geometries
{
    public sealed class Line(XY start, XY end) : Curve
    {
        public XY Start { get; } = start;
        public XY End { get; } = end;

        public override XY GetEndPoint(int index) =>
            index == 0 ? Start : End;

        public override double Length =>
            Math.Sqrt(Math.Pow(End.X - Start.X, 2) +
                      Math.Pow(End.Y - Start.Y, 2));
    }
}

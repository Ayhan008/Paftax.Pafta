using Paftax.Pafta.Drawing.Structs;

namespace Paftax.Pafta.Drawing.Geometries
{
    public sealed class Polyline2(IEnumerable<Curve2> curves) : Curve2
    {
        public IReadOnlyList<Curve2> Segments { get; } =
            [.. (curves ?? throw new ArgumentNullException(nameof(curves)))];

        public override Point2 GetEndPoint(int index) =>
            index == 0 ? Segments[0].GetEndPoint(0) : Segments[^1].GetEndPoint(1);

        public override double Length => Segments.Sum(s => s.Length);
    }
}

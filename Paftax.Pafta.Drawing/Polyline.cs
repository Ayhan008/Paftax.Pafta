using Paftax.Pafta.Shared.Geometries;

namespace Paftax.Pafta.Drawings
{
    public sealed class Polyline(IEnumerable<Curve> curves) : Curve
    {
        public IReadOnlyList<Curve> Segments { get; } =
            [.. (curves ?? throw new ArgumentNullException(nameof(curves)))];

        public override Point2 GetEndPoint(int index) =>
            index == 0 ? Segments[0].GetEndPoint(0) : Segments[^1].GetEndPoint(1);

        public override double Length => Segments.Sum(s => s.Length);
    }
}

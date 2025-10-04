using Paftax.Pafta.Drawing.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paftax.Pafta.Drawing.Geometries
{
    public sealed class Polyline(IEnumerable<Curve> curves) : Curve
    {
        public IReadOnlyList<Curve> Segments { get; } =
            [.. (curves ?? throw new ArgumentNullException(nameof(curves)))];

        public override XY GetEndPoint(int index) =>
            index == 0 ? Segments[0].GetEndPoint(0) : Segments[^1].GetEndPoint(1);

        public override double Length => Segments.Sum(s => s.Length);
    }
}

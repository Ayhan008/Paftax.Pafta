using System.Numerics;

namespace Paftax.Pafta.Shared.Models
{
    public class BoundingBoxData
    {
        public Vector3 Min { get; set; }

        public Vector3 Max { get; set; }

        public Vector3 Center => new(
            (Min.X + Max.X) / 2,
            (Min.Y + Max.Y) / 2,
            (Min.Z + Max.Z) / 2
        );

        public Vector3 Size => new(
            Max.X - Min.X,
            Max.Y - Min.Y,
            Max.Z - Min.Z
        );

        public float Area => (Max.X - Min.X) * (Max.Y - Min.Y);
    }
}

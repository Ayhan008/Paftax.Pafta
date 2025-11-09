using System.Windows.Media.Media3D;

namespace Paftax.Pafta.Shared.Geometries
{
    public struct Bounding3(Point3 min, Point3 max)
    {
        public Point3 Min { get; set; } = min;
        public Point3 Max { get; set; } = max;

        public readonly double Width => Max.X - Min.X;
        public readonly double Height => Max.Y - Min.Y;
        public readonly double Depth => Max.Z - Min.Z;

        public readonly Point3 Center => new((Min.X + Max.X) / 2, (Min.Y + Max.Y) / 2, (Min.Z + Max.Z) / 2);

        public void ExpandToInclude(Point3 point)
        {
            if (point.X < Min.X) Min = new Point3(point.X, Min.Y, Min.Z);
            if (point.Y < Min.Y) Min = new Point3(Min.X, point.Y, Min.Z);
            if (point.Z < Min.Z) Min = new Point3(Min.X, Min.Y, point.Z);

            if (point.X > Max.X) Max = new Point3(point.X, Max.Y, Max.Z);
            if (point.Y > Max.Y) Max = new Point3(Max.X, point.Y, Max.Z);
            if (point.Z > Max.Z) Max = new Point3(Max.X, Max.Y, point.Z);
        }

        public void ExpandToInclude(Bounding3 other)
        {
            if (other.Min.X < Min.X) Min = new Point3(other.Min.X, Min.Y, Min.Z);
            if (other.Min.Y < Min.Y) Min = new Point3(Min.X, other.Min.Y, Min.Z);
            if (other.Min.Z < Min.Z) Min = new Point3(Min.X, Min.Y, other.Min.Z);

            if (other.Max.X > Max.X) Max = new Point3(other.Max.X, Max.Y, Max.Z);
            if (other.Max.Y > Max.Y) Max = new Point3(Max.X, other.Max.Y, Max.Z);
            if (other.Max.Z > Max.Z) Max = new Point3(Max.X, Max.Y, other.Max.Z);
        }

        public void IntersectWith(Bounding3 other)
        {
            Min = new Point3(
                Math.Max(Min.X, other.Min.X),
                Math.Max(Min.Y, other.Min.Y),
                Math.Max(Min.Z, other.Min.Z)
            );

            Max = new Point3(
                Math.Min(Max.X, other.Max.X),
                Math.Min(Max.Y, other.Max.Y),
                Math.Min(Max.Z, other.Max.Z)
            );
        }

        public readonly Bounding2 ToBoundingXY() => new(new Point2(Min.X, Min.Y), new Point2(Max.X, Max.Y));

        public static implicit operator Rect3D(Bounding3 b) => new(b.Min.X, b.Min.Y, b.Min.Z, b.Width, b.Height, b.Depth);

        public static implicit operator Bounding3(Rect3D r) => new(new Point3(r.X, r.Y, r.Z), new Point3(r.X + r.SizeX, r.Y + r.SizeY, r.Z + r.SizeZ));
        public override readonly string ToString() => $"Min: {Min}, Max: {Max}";

    }
}

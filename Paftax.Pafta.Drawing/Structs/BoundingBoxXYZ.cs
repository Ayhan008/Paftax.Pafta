using System.Windows.Media.Media3D;

namespace Paftax.Pafta.Drawing.Structs
{
    public struct BoundingBoxXYZ(PointXYZ min, PointXYZ max)
    {
        public PointXYZ Min { get; set; } = min;
        public PointXYZ Max { get; set; } = max;
        public readonly double Width => Max.X - Min.X;
        public readonly double Height => Max.Y - Min.Y;
        public readonly double Depth => Max.Z - Min.Z;
        public readonly PointXYZ Center => new((Min.X + Max.X) / 2, (Min.Y + Max.Y) / 2, (Min.Z + Max.Z) / 2);
        public void Contains(PointXYZ point)
        {
            if (point.X < Min.X) Min = new PointXYZ(point.X, Min.Y, Min.Z);
            if (point.Y < Min.Y) Min = new PointXYZ(Min.X, point.Y, Min.Z);
            if (point.Z < Min.Z) Min = new PointXYZ(Min.X, Min.Y, point.Z);
            if (point.X > Max.X) Max = new PointXYZ(point.X, Max.Y, Max.Z);
            if (point.Y > Max.Y) Max = new PointXYZ(Max.X, point.Y, Max.Z);
            if (point.Z > Max.Z) Max = new PointXYZ(Max.X, Max.Y, point.Z);
        }
        public void Contains(BoundingBoxXYZ other)
        {
            if (other.Min.X < Min.X) Min = new PointXYZ(other.Min.X, Min.Y, Min.Z);
            if (other.Min.Y < Min.Y) Min = new PointXYZ(Min.X, other.Min.Y, Min.Z);
            if (other.Min.Z < Min.Z) Min = new PointXYZ(Min.X, Min.Y, other.Min.Z);
            if (other.Max.X > Max.X) Max = new PointXYZ(other.Max.X, Max.Y, Max.Z);
            if (other.Max.Y > Max.Y) Max = new PointXYZ(Max.X, other.Max.Y, Max.Z);
            if (other.Max.Z > Max.Z) Max = new PointXYZ(Max.X, Max.Y, other.Max.Z);
        }
        public void IntersectWith(BoundingBoxXYZ other)
        {
            Min = new PointXYZ(Math.Max(Min.X, other.Min.X), Math.Max(Min.Y, other.Min.Y), Math.Max(Min.Z, other.Min.Z));
            Max = new PointXYZ(Math.Min(Max.X, other.Max.X), Math.Min(Max.Y, other.Max.Y), Math.Min(Max.Z, other.Max.Z));
        }

        public readonly BoundingXY ToBoundingXY()
        {
            return new BoundingXY(new PointXY(Min.X, Min.Y), new PointXY(Max.X, Max.Y));
        }
    }
}

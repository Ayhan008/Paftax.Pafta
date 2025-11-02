namespace Paftax.Pafta.Drawing.Structs
{
    public struct Bounding3(Point3 min, Point3 max)
    {
        public Point3 Min { get; set; } = min;
        public Point3 Max { get; set; } = max;
        public readonly double Width => Max.X - Min.X;
        public readonly double Height => Max.Y - Min.Y;
        public readonly double Depth => Max.Z - Min.Z;
        public readonly Point3 Center => new((Min.X + Max.X) / 2, (Min.Y + Max.Y) / 2, (Min.Z + Max.Z) / 2);
        public void Contains(Point3 point)
        {
            if (point.X < Min.X) Min = new Point3(point.X, Min.Y, Min.Z);
            if (point.Y < Min.Y) Min = new Point3(Min.X, point.Y, Min.Z);
            if (point.Z < Min.Z) Min = new Point3(Min.X, Min.Y, point.Z);
            if (point.X > Max.X) Max = new Point3(point.X, Max.Y, Max.Z);
            if (point.Y > Max.Y) Max = new Point3(Max.X, point.Y, Max.Z);
            if (point.Z > Max.Z) Max = new Point3(Max.X, Max.Y, point.Z);
        }
        public void Contains(Bounding3 other)
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
            Min = new Point3(Math.Max(Min.X, other.Min.X), Math.Max(Min.Y, other.Min.Y), Math.Max(Min.Z, other.Min.Z));
            Max = new Point3(Math.Min(Max.X, other.Max.X), Math.Min(Max.Y, other.Max.Y), Math.Min(Max.Z, other.Max.Z));
        }

        public readonly Bounding2 ToBoundingXY()
        {
            return new Bounding2(new Point2(Min.X, Min.Y), new Point2(Max.X, Max.Y));
        }
    }
}

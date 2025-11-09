using System.Windows;

namespace Paftax.Pafta.Shared.Geometries
{
    public struct Bounding2(Point2 min, Point2 max)
    {
        public Point2 Min { get; set; } = min;
        public Point2 Max { get; set; } = max;

        public readonly double Width => Max.X - Min.X;
        public readonly double Height => Max.Y - Min.Y;
        public readonly Point2 Center => new((Min.X + Max.X) / 2, (Min.Y + Max.Y) / 2);

        public void ExpandToInclude(Point2 point)
        {
            if (point.X < Min.X) Min = new Point2(point.X, Min.Y);
            if (point.Y < Min.Y) Min = new Point2(Min.X, point.Y);
            if (point.X > Max.X) Max = new Point2(point.X, Max.Y);
            if (point.Y > Max.Y) Max = new Point2(Max.X, point.Y);
        }

        public void ExpandToInclude(Bounding2 other)
        {
            if (other.Min.X < Min.X) Min = new Point2(other.Min.X, Min.Y);
            if (other.Min.Y < Min.Y) Min = new Point2(Min.X, other.Min.Y);
            if (other.Max.X > Max.X) Max = new Point2(other.Max.X, Max.Y);
            if (other.Max.Y > Max.Y) Max = new Point2(Max.X, other.Max.Y);
        }

        public void IntersectWith(Bounding2 other)
        {
            Min = new Point2(Math.Max(Min.X, other.Min.X), Math.Max(Min.Y, other.Min.Y));
            Max = new Point2(Math.Min(Max.X, other.Max.X), Math.Min(Max.Y, other.Max.Y));
        }

        public readonly Bounding2 Union(Bounding2 other)
        {
            Point2 newMin = new(Math.Min(Min.X, other.Min.X), Math.Min(Min.Y, other.Min.Y));
            Point2 newMax = new(Math.Max(Max.X, other.Max.X), Math.Max(Max.Y, other.Max.Y));
            return new Bounding2(newMin, newMax);
        }

        public static implicit operator Rect(Bounding2 b) => new(b.Min.X, b.Min.Y, b.Width, b.Height);


        public static implicit operator Bounding2(Rect r) => new(new Point2(r.Left, r.Top), new Point2(r.Right, r.Bottom));

    }
}

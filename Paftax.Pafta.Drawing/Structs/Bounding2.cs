namespace Paftax.Pafta.Drawing.Structs
{
    public struct Bounding2(Point2 min, Point2 max)
    {
        public Point2 Min { get; set; } = min;
        public Point2 Max { get; set; } = max;
        public readonly double Width
        {
            get
            {
                return Max.X - Min.X;
            }
        }

        public readonly double Height
        {
            get
            {
                return Max.Y - Min.Y;
            }
        }

        public readonly Point2 Center
        {
            get
            {
                return new Point2((Min.X + Max.X) / 2, (Min.Y + Max.Y) / 2);
            }
        }

        public void Contains(Point2 point)
        {
            if (point.X < Min.X) Min = new Point2(point.X, Min.Y);
            if (point.Y < Min.Y) Min = new Point2(Min.X, point.Y);
            if (point.X > Max.X) Max = new Point2(point.X, Max.Y);
            if (point.Y > Max.Y) Max = new Point2(Max.X, point.Y);
        }

        public void Contains(Bounding2 other)
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
    }
}

namespace Paftax.Pafta.Drawing.Structs
{
    public struct BoundingXY(PointXY min, PointXY max)
    {
        public PointXY Min { get; set; } = min;
        public PointXY Max { get; set; } = max;
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

        public readonly PointXY Center
        {
            get
            {
                return new PointXY((Min.X + Max.X) / 2, (Min.Y + Max.Y) / 2);
            }
        }

        public void Contains(PointXY point)
        {
            if (point.X < Min.X) Min = new PointXY(point.X, Min.Y);
            if (point.Y < Min.Y) Min = new PointXY(Min.X, point.Y);
            if (point.X > Max.X) Max = new PointXY(point.X, Max.Y);
            if (point.Y > Max.Y) Max = new PointXY(Max.X, point.Y);
        }

        public void Contains(BoundingXY other)
        {
            if (other.Min.X < Min.X) Min = new PointXY(other.Min.X, Min.Y);
            if (other.Min.Y < Min.Y) Min = new PointXY(Min.X, other.Min.Y);
            if (other.Max.X > Max.X) Max = new PointXY(other.Max.X, Max.Y);
            if (other.Max.Y > Max.Y) Max = new PointXY(Max.X, other.Max.Y);
        }

        public void IntersectWith(BoundingXY other)
        {
            Min = new PointXY(Math.Max(Min.X, other.Min.X), Math.Max(Min.Y, other.Min.Y));
            Max = new PointXY(Math.Min(Max.X, other.Max.X), Math.Min(Max.Y, other.Max.Y));
        }
    }
}

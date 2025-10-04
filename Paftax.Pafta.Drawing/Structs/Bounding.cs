namespace Paftax.Pafta.Drawing.Structs
{
    public struct Bounding(XY min, XY max)
    {
        public XY Min { get; set; } = min;
        public XY Max { get; set; } = max;
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

        public readonly XY Center
        {
            get
            {
                return new XY((Min.X + Max.X) / 2, (Min.Y + Max.Y) / 2);
            }
        }

        public void Contains(XY point)
        {
            if (point.X < Min.X) Min = new XY(point.X, Min.Y);
            if (point.Y < Min.Y) Min = new XY(Min.X, point.Y);
            if (point.X > Max.X) Max = new XY(point.X, Max.Y);
            if (point.Y > Max.Y) Max = new XY(Max.X, point.Y);
        }

        public void Contains(Bounding other)
        {
            if (other.Min.X < Min.X) Min = new XY(other.Min.X, Min.Y);
            if (other.Min.Y < Min.Y) Min = new XY(Min.X, other.Min.Y);
            if (other.Max.X > Max.X) Max = new XY(other.Max.X, Max.Y);
            if (other.Max.Y > Max.Y) Max = new XY(Max.X, other.Max.Y);
        }

        public void IntersectWith(Bounding other)
        {
            Min = new XY(Math.Max(Min.X, other.Min.X), Math.Max(Min.Y, other.Min.Y));
            Max = new XY(Math.Min(Max.X, other.Max.X), Math.Min(Max.Y, other.Max.Y));
        }
    }
}

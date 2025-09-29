using Paftax.Pafta.Drawing.Geometries.Primitives;

namespace Paftax.Pafta.Drawing.Geometries
{
    public class BoundingXY
    {
        public PointXY Min { get; private set; } = new(double.MaxValue, double.MaxValue);
        public PointXY Max { get; private set; } = new(double.MinValue, double.MinValue);

        public double Width => Max.X - Min.X;
        public double Height => Max.Y - Min.Y;
        public PointXY Center => new((Min.X + Max.X) / 2.0, (Min.Y + Max.Y) / 2.0);

        public BoundingXY() { }

        public void IncludePoint(PointXY point)
        {
            var min = Min;
            var max = Max;

            if (point.X < min.X) min.X = point.X;
            if (point.X > max.X) max.X = point.X;
            if (point.Y < min.Y) min.Y = point.Y;
            if (point.Y > max.Y) max.Y = point.Y;

            Min = min;
            Max = max;
        }

        public void IncludePoints(IEnumerable<PointXY> points)
        {
            foreach (var p in points)
            {
                IncludePoint(p);
            }
        }

        public void IncludeBounding(BoundingXY other)
        {
            IncludePoint(other.Min);
            IncludePoint(other.Max);
        }

        public bool Contains(PointXY point) =>
            point.X >= Min.X && point.X <= Max.X &&
            point.Y >= Min.Y && point.Y <= Max.Y;
    }
}

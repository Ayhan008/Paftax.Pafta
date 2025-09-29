namespace Paftax.Pafta.Drawing.Geometries.Primitives
{
    public struct PointXY(double x, double y)
    {
        public double X { get; set; } = x;
        public double Y { get; set; } = y;
        public static PointXY operator +(PointXY a, PointXY b) => new(a.X + b.X, a.Y + b.Y);
        public static PointXY operator -(PointXY a, PointXY b) => new(a.X - b.X, a.Y - b.Y);
        public static PointXY operator *(PointXY a, double scalar) => new(a.X * scalar, a.Y * scalar);
        public static PointXY operator /(PointXY a, double scalar) => new(a.X / scalar, a.Y / scalar);
        public override readonly string ToString() => $"({X}, {Y})";
    }
}

using System.Windows;

namespace Paftax.Pafta.Drawing.Structs
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

        public static implicit operator Point(PointXY p) => new(p.X, p.Y);
        public static implicit operator PointXY(Point p) => new(p.X, p.Y);
    }
}

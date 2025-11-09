using System.Windows;

namespace Paftax.Pafta.Shared.Geometries
{
    public struct Point2(double x, double y)
    {
        public double X { get; set; } = x;
        public double Y { get; set; } = y;
        public static Point2 operator +(Point2 a, Point2 b) => new(a.X + b.X, a.Y + b.Y);
        public static Point2 operator -(Point2 a, Point2 b) => new(a.X - b.X, a.Y - b.Y);
        public static Point2 operator *(Point2 a, double scalar) => new(a.X * scalar, a.Y * scalar);
        public static Point2 operator /(Point2 a, double scalar) => new(a.X / scalar, a.Y / scalar);
        public override readonly string ToString() => $"({X}, {Y})";

        public static implicit operator Point(Point2 p) => new(p.X, p.Y);
        public static implicit operator Point2(Point p) => new(p.X, p.Y);
    }
}

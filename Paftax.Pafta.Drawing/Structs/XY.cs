using System.Windows;

namespace Paftax.Pafta.Drawing.Structs
{
    public struct XY(double x, double y)
    {
        public double X { get; set; } = x;
        public double Y { get; set; } = y;
        public static XY operator +(XY a, XY b) => new(a.X + b.X, a.Y + b.Y);
        public static XY operator -(XY a, XY b) => new(a.X - b.X, a.Y - b.Y);
        public static XY operator *(XY a, double scalar) => new(a.X * scalar, a.Y * scalar);
        public static XY operator /(XY a, double scalar) => new(a.X / scalar, a.Y / scalar);
        public override readonly string ToString() => $"({X}, {Y})";

        public static implicit operator Point(XY p) => new(p.X, p.Y);
        public static implicit operator XY(Point p) => new(p.X, p.Y);
    }
}

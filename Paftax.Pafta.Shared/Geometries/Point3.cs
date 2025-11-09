using System.Windows.Media.Media3D;

namespace Paftax.Pafta.Shared.Geometries
{
    public struct Point3(double x, double y, double z)
    {
        public double X { get; set; } = x;
        public double Y { get; set; } = y;
        public double Z { get; set; } = z;

        public static Point3 operator +(Point3 a, Point3 b) => new(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        public static Point3 operator -(Point3 a, Point3 b) => new(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        public static Point3 operator *(Point3 a, double scalar) => new(a.X * scalar, a.Y * scalar, a.Z * scalar);
        public static Point3 operator /(Point3 a, double scalar) => new(a.X / scalar, a.Y / scalar, a.Z / scalar);

        public static implicit operator Point3D(Point3 v) => new(v.X, v.Y, v.Z);
        public static implicit operator Point3(Point3D v) => new(v.X, v.Y, v.Z);


        public override readonly string ToString() => $"({X}, {Y}, {Z})";

        public readonly Point2 ToPointXY() => new(X, Y);
    }
}

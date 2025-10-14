using System.Windows.Media.Media3D;

namespace Paftax.Pafta.Drawing.Structs
{
    public struct PointXYZ(double x, double y, double z)
    {
        public double X { get; set; } = x;
        public double Y { get; set; } = y;
        public double Z { get; set; } = z;
        public static PointXYZ operator +(PointXYZ a, PointXYZ b) => new(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        public static PointXYZ operator -(PointXYZ a, PointXYZ b) => new(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        public static PointXYZ operator *(PointXYZ a, double scalar) => new(a.X * scalar, a.Y * scalar, a.Z * scalar);
        public static PointXYZ operator /(PointXYZ a, double scalar) => new(a.X / scalar, a.Y / scalar, a.Z / scalar);

        public static implicit operator Point3D(PointXYZ v)
        {
            throw new NotImplementedException();
        }

        public override readonly string ToString() => $"({X}, {Y}, {Z})";
        public readonly PointXY ToPointXY() => new(X, Y);
    }
}

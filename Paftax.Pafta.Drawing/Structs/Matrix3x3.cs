using System.Windows;

namespace Paftax.Pafta.Drawing.Structs
{
    public struct Matrix3x3(
        double m11, double m12, double m13,
        double m21, double m22, double m23,
        double m31, double m32, double m33)
    {
        public double M11 = m11, M12 = m12, M13 = m13;
        public double M21 = m21, M22 = m22, M23 = m23;
        public double M31 = m31, M32 = m32, M33 = m33;


        #region Identity
        public static Matrix3x3 Identity =>
            new(1, 0, 0,
                0, 1, 0,
                0, 0, 1);
        #endregion

        #region Operators
        public static Matrix3x3 operator *(Matrix3x3 a, Matrix3x3 b)
        {
            return new Matrix3x3(
                a.M11 * b.M11 + a.M12 * b.M21 + a.M13 * b.M31,
                a.M11 * b.M12 + a.M12 * b.M22 + a.M13 * b.M32,
                a.M11 * b.M13 + a.M12 * b.M23 + a.M13 * b.M33,

                a.M21 * b.M11 + a.M22 * b.M21 + a.M23 * b.M31,
                a.M21 * b.M12 + a.M22 * b.M22 + a.M23 * b.M32,
                a.M21 * b.M13 + a.M22 * b.M23 + a.M23 * b.M33,

                a.M31 * b.M11 + a.M32 * b.M21 + a.M33 * b.M31,
                a.M31 * b.M12 + a.M32 * b.M22 + a.M33 * b.M32,
                a.M31 * b.M13 + a.M32 * b.M23 + a.M33 * b.M33
            );
        }

        public static Vector operator *(Matrix3x3 m, Vector v)
        {
            double x = m.M11 * v.X + m.M12 * v.Y + m.M13 * 1.0;
            double y = m.M21 * v.X + m.M22 * v.Y + m.M23 * 1.0;
            return new Vector(x, y);
        }

        public static Point operator *(Matrix3x3 m, Point p)
        {
            double x = m.M11 * p.X + m.M12 * p.Y + m.M13 * 1.0;
            double y = m.M21 * p.X + m.M22 * p.Y + m.M23 * 1.0;
            return new Point(x, y);
        }
        #endregion

        #region Transformation Helpers
        public static Matrix3x3 CreateTranslation(double dx, double dy)
        {
            return new Matrix3x3(
                1, 0, dx,
                0, 1, dy,
                0, 0, 1
            );
        }

        public static Matrix3x3 CreateScale(double sx, double sy)
        {
            return new Matrix3x3(
                sx, 0, 0,
                0, sy, 0,
                0, 0, 1
            );
        }

        public static Matrix3x3 CreateRotation(double angleRadians)
        {
            double c = Math.Cos(angleRadians);
            double s = Math.Sin(angleRadians);
            return new Matrix3x3(
                c, -s, 0,
                s, c, 0,
                0, 0, 1
            );
        }
        #endregion

        #region Determinant & Inverse
        public readonly double Determinant
        {
            get
            {
                return M11 * (M22 * M33 - M23 * M32) -
                       M12 * (M21 * M33 - M23 * M31) +
                       M13 * (M21 * M32 - M22 * M31);
            }
        }

        public readonly Matrix3x3 Inverse()
        {
            double det = Determinant;
            if (Math.Abs(det) < 1e-12)
                throw new InvalidOperationException("Matrix is singular and cannot be inverted.");

            double inv = 1.0 / det;

            return new Matrix3x3(
                inv * (M22 * M33 - M23 * M32),
                inv * (M13 * M32 - M12 * M33),
                inv * (M12 * M23 - M13 * M22),

                inv * (M23 * M31 - M21 * M33),
                inv * (M11 * M33 - M13 * M31),
                inv * (M13 * M21 - M11 * M23),

                inv * (M21 * M32 - M22 * M31),
                inv * (M12 * M31 - M11 * M32),
                inv * (M11 * M22 - M12 * M21)
            );
        }
        #endregion

        public override readonly string ToString()
        {
            return $"[{M11}, {M12}, {M13}] \n[{M21}, {M22}, {M23}] \n[{M31}, {M32}, {M33}]";
        }
    }
}

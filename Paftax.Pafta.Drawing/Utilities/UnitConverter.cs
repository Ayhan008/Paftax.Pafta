using System.Windows;

namespace Paftax.Pafta.Drawing.Utilities
{
    public static class UnitConverter
    {
        private const double InchesPerFoot = 12.0;
        private const double MmPerInch = 25.4;
        private const double PointsPerInch = 72.0;
        private const double CmPerInch = 2.54;
        private const double MPerInch = 0.0254;

        #region Millimeter ↔ Inch
        public static double MmToInch(double mm) => mm / MmPerInch;
        public static double InchToMm(double inch) => inch * MmPerInch;

        public static Point MmToInch(Point pt) => new Point(MmToInch(pt.X), MmToInch(pt.Y));
        public static Point InchToMm(Point pt) => new Point(InchToMm(pt.X), InchToMm(pt.Y));
        #endregion

        #region Inch ↔ Feet
        public static double InchToFeet(double inch) => inch / InchesPerFoot;
        public static double FeetToInch(double feet) => feet * InchesPerFoot;

        public static Point InchToFeet(Point pt) => new Point(InchToFeet(pt.X), InchToFeet(pt.Y));
        public static Point FeetToInch(Point pt) => new Point(FeetToInch(pt.X), FeetToInch(pt.Y));
        #endregion

        #region Mm ↔ Feet
        public static double MmToFeet(double mm) => InchToFeet(MmToInch(mm));
        public static double FeetToMm(double feet) => InchToMm(FeetToInch(feet));

        public static Point MmToFeet(Point pt) => new Point(MmToFeet(pt.X), MmToFeet(pt.Y));
        public static Point FeetToMm(Point pt) => new Point(FeetToMm(pt.X), FeetToMm(pt.Y));
        #endregion

        #region Inch ↔ Points
        public static double InchToPoint(double inch) => inch * PointsPerInch;
        public static double PointToInch(double pt) => pt / PointsPerInch;

        public static Point InchToPoint(Point pt) => new Point(InchToPoint(pt.X), InchToPoint(pt.Y));
        public static Point PointToInch(Point pt) => new Point(PointToInch(pt.X), PointToInch(pt.Y));
        #endregion

        #region Mm ↔ Points
        public static double MmToPoint(double mm) => InchToPoint(MmToInch(mm));
        public static double PointToMm(double pt) => InchToMm(PointToInch(pt));

        public static Point MmToPoint(Point pt) => new Point(MmToPoint(pt.X), MmToPoint(pt.Y));
        public static Point PointToMm(Point pt) => new Point(PointToMm(pt.X), PointToMm(pt.Y));
        #endregion

        #region Feet ↔ Points
        public static double FeetToPoint(double feet) => InchToPoint(FeetToInch(feet));
        public static double PointToFeet(double pt) => InchToFeet(PointToInch(pt));

        public static Point FeetToPoint(Point pt) => new Point(FeetToPoint(pt.X), FeetToPoint(pt.Y));
        public static Point PointToFeet(Point pt) => new Point(PointToFeet(pt.X), PointToFeet(pt.Y));
        #endregion

        #region Point ↔ CM
        public static double PointToCm(double pt) => InchToCm(PointToInch(pt));
        public static double CmToPoint(double cm) => InchToPoint(CmToInch(cm));

        public static Point PointToCm(Point pt) => new Point(PointToCm(pt.X), PointToCm(pt.Y));
        public static Point CmToPoint(Point pt) => new Point(CmToPoint(pt.X), CmToPoint(pt.Y));

        private static double InchToCm(double inch) => inch * CmPerInch;
        private static double CmToInch(double cm) => cm / CmPerInch;
        #endregion

        #region Point ↔ M
        public static double PointToM(double pt) => InchToM(PointToInch(pt));
        public static double MToPoint(double m) => InchToPoint(MToInch(m));

        public static Point PointToM(Point pt) => new Point(PointToM(pt.X), PointToM(pt.Y));
        public static Point MToPoint(Point pt) => new Point(MToPoint(pt.X), MToPoint(pt.Y));

        private static double InchToM(double inch) => inch * MPerInch;
        private static double MToInch(double m) => m / MPerInch;
        #endregion
    }
}

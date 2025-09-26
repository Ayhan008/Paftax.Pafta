using Autodesk.Revit.DB;

namespace Paftax.Pafta.Revit2026.Utilities
{
    internal static class Converter
    {
        #region ElementId -> long

        public static long ToLong(this ElementId elementId) => elementId.Value;

        public static ElementId ToElementId(this long id) => new(id);

        #endregion

        public static System.Windows.Point ToWPF(this XYZ xyz)
        {
            if (xyz == null) return new System.Windows.Point(0, 0);
            return new System.Windows.Point(xyz.X, xyz.Y);
        }
    }
}
using Autodesk.Revit.DB;

namespace Paftax.Pafta.Revit2026.Utilities
{
    internal static class Converters
    {
        #region ElementId -> long

        public static long ToLong(this ElementId elementId) => elementId.Value;

        public static ElementId ToElementId(this long id) => new(id);

        #endregion
    }
}
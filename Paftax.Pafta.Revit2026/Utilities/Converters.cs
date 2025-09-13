using Autodesk.Revit.DB;

namespace Paftax.Pafta.Revit2026.Utilities
{
    internal class Converters
    {
        public static long ElementIdToLong(ElementId elementId)
        {
            return elementId.Value;
        }

        public static ElementId LongToElementId(long id)
        {
            return new ElementId(id);
        }
    }
}

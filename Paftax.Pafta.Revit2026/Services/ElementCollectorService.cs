using Autodesk.Revit.DB;

namespace Paftax.Pafta.Revit2026.Services
{
    public class ElementCollectorService(Document doc)
    {
        private readonly Document _doc = doc;

        public List<T> GetElementsInDocument<T>() where T : Element
        {
            return [.. new FilteredElementCollector(_doc)
                .OfClass(typeof(T))
                .Cast<T>()];
        }

        public T? GetElementById<T>(long id) where T : Element
        {
            var elem = _doc.GetElement(new ElementId(id));
            return elem as T;
        }

        public List<T> GetElementsByIds<T>(IEnumerable<long> ids) where T : Element
        {
            return [.. ids
                .Select(id => GetElementById<T>(id))
                .Where(e => e != null)
                .Cast<T>()];
        }

        public T? GetElementByFamilyAndTypeName<T>(string familyName, string typeName) where T : Element
        {
            return new FilteredElementCollector(_doc)
                .OfClass(typeof(T))
                .Cast<T>()
                .FirstOrDefault(e => e.Name.Equals(typeName, StringComparison.OrdinalIgnoreCase) && 
                (e is FamilyInstance fi && fi.Symbol.Family.Name.Equals(familyName, StringComparison.OrdinalIgnoreCase)));

        }
    }
}

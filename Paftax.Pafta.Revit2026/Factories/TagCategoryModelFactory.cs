using Autodesk.Revit.DB;
using Paftax.Pafta.Shared.Models;

namespace Paftax.Pafta.Revit2026.Factories
{
    internal class TagCategoryModelFactory
    {
        public static List<TagCategoryModel> CreateModels(Document document)
        {
            IEnumerable<IndependentTag> tags = new FilteredElementCollector(document)
                .OfClass(typeof(IndependentTag))
                .Cast<IndependentTag>();

            List<TagCategoryModel> result = [.. tags
                .GroupBy(t => t.Category?.Name ?? "Unknown")
                .Select(g => new TagCategoryModel
                {
                    Category = g.Key,
                    Count = g.Count(t => t.IsOrphaned),
                    Tags = [.. g.Select(t => TagModelFactory.FromTag(t))]
                })];

            return result;
        }
    }
}

using Autodesk.Revit.DB;
using Paftax.Pafta.Shared.Models;

namespace Paftax.Pafta.Revit2026.Factories
{
    internal class TagCategoryModelFactory(Document document)
    {
        private readonly Document _document = document;
        public List<TagCategoryModel> CreateModels()
        {
            IEnumerable<IndependentTag> tags = new FilteredElementCollector(_document)
                .OfClass(typeof(IndependentTag))
                .Cast<IndependentTag>();

            List<TagCategoryModel> result = [.. tags
                .GroupBy(t => t.Category?.Name ?? "Unknown")
                .Select(g => new TagCategoryModel
                {
                    Category = g.Key,
                    Count = g.Count(t => t.IsOrphaned),
                    Tags = [.. g.Select(t => TagModelFactory.CreateModel(t))]
                })];

            return result;
        }
    }
}
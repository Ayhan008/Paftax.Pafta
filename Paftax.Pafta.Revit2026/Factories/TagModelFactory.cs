using Autodesk.Revit.DB;
using Paftax.Pafta.Shared.Models;

namespace Paftax.Pafta.Revit2026.Factories
{
    internal class TagModelFactory(Document document)
    {
        private readonly Document _document = document;
        public static TagModel CreateModel(IndependentTag independentTag)
        {
            return new TagModel
            {
                Id = independentTag.Id.Value,
                IsOrphaned = independentTag.IsOrphaned,
                TagCategory = independentTag.Category.Name
            };

        }
    }
}

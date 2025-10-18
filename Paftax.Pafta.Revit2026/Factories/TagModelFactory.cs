using Autodesk.Revit.DB;
using Paftax.Pafta.Revit2026.Utilities;
using Paftax.Pafta.Shared.Models;

namespace Paftax.Pafta.Revit2026.Factories
{
    internal class TagModelFactory
    {
        public static TagModel FromTag(IndependentTag independentTag)
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

using Autodesk.Revit.DB;
using Paftax.Pafta.Revit2026.Utilities;
using Paftax.Pafta.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paftax.Pafta.Revit2026.Factories
{
    internal class TagModelFactory
    {
        public static TagModel FromTag(IndependentTag independentTag)
        {
            return new TagModel
            {
                Id = independentTag.Id.ToLong(),
                IsOrphaned = independentTag.IsOrphaned,
                TagCategory = independentTag.Category.Name
            };
            
        }
    }
}

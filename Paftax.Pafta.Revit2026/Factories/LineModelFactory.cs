using Autodesk.Revit.DB;
using Paftax.Pafta.Revit2026.Utilities;
using Paftax.Pafta.Shared.Models;

namespace Paftax.Pafta.Revit2026.Factories
{
    internal class LineModelFactory
    {
        /// <summary>
        /// Creates a list of LineModel instances representing line categories in the given Revit document.
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        public static List<LineModel> CreateModels(Document document)
        {
            // All line models
            List<LineModel> lineModels = [];

            // Get line categories and their usage counts
            Category linesCat = document.Settings.Categories.get_Item(BuiltInCategory.OST_Lines);
            if (linesCat == null)
                return lineModels;

            foreach (Category subCat in linesCat.SubCategories)
            {
                // Skip system categories
                if (subCat.Name.StartsWith('<') && subCat.Name.EndsWith('>'))                                  
                    continue;

                int count = 0;
                if (subCat.Id == new ElementId(BuiltInCategory.OST_Lines))
                {
                    // If the subcategory is the main Lines category, count all line elements in the document
                    count += new FilteredElementCollector(document)
                        .OfClass(typeof(CurveElement))
                        .WhereElementIsNotElementType()
                        .Where(e => e.Category != null && e.Category.Id == subCat.Id)
                        .Count();
                }

                // Count line elements in all non-template views
                IEnumerable<View> views = new FilteredElementCollector(document)
                    .OfClass(typeof(View))
                    .Cast<View>()
                    .Where(v => v.IsTemplate == false);

                foreach (View view in views)
                {
                    try
                    {
                        count += new FilteredElementCollector(document, view.Id)
                            .OfClass(typeof(CurveElement))
                            .WhereElementIsNotElementType()
                            .Where(e => e.Category != null && e.Category.Id == subCat.Id)
                            .Count();
                    }
                    catch
                    {
                        continue;
                    }
                }

                // Create and add the line model to the list
                lineModels.Add(new LineModel
                {
                    Id = subCat.Id.Value,
                    Name = subCat.Name,
                    Count = count
                });
            }
            return lineModels;
        }
    }
}

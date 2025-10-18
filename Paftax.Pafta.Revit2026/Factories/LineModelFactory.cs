using Autodesk.Revit.DB;
using Paftax.Pafta.Shared.Models;

namespace Paftax.Pafta.Revit2026.Factories
{
    internal class LineModelFactory
    {
        private static volatile bool _cancelRequested = false;
        public static bool CancelRequested
        {
            get => _cancelRequested;
            set => _cancelRequested = value;
        }

        /// <summary>
        /// Creates a list of LineModel instances representing line categories in the given Revit document.
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        public static List<LineModel> CreateModels(Document document)
        {
            List<LineModel> lineModels = [];

            Category linesCat = document.Settings.Categories.get_Item(BuiltInCategory.OST_Lines);
            if (linesCat == null)
                return lineModels;

            foreach (Category subCat in linesCat.SubCategories)
            {
                if (CancelRequested)
                    break;

                if (subCat.Name.StartsWith('<') && subCat.Name.EndsWith('>'))
                    continue;

                int count = 0;
                if (subCat.Id == new ElementId(BuiltInCategory.OST_Lines))
                {
                    count += new FilteredElementCollector(document)
                        .OfClass(typeof(CurveElement))
                        .WhereElementIsNotElementType()
                        .Where(e => e.Category != null && e.Category.Id == subCat.Id)
                        .Count();
                }

                IEnumerable<View> views = new FilteredElementCollector(document)
                    .OfClass(typeof(View))
                    .Cast<View>()
                    .Where(v => v.IsTemplate == false);

                foreach (View view in views)
                {
                    if (CancelRequested)
                        break;

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

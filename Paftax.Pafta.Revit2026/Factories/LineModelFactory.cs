using Autodesk.Revit.DB;
using Paftax.Pafta.Shared.Models;

namespace Paftax.Pafta.Revit2026.Factories
{
    internal class LineModelFactory(Document document)
    {
        private readonly Document _document = document;
        private CancellationToken _token = CancellationToken.None;

        public LineModelFactory Cancellable(CancellationToken token)
        {
            _token = token;
            return this;
        }

        /// <summary>
        /// Creates a list of LineModel instances representing line categories in the given Revit document.
        /// </summary>
        public List<LineModel> CreateModels()
        {
            List<LineModel> lineModels = [];

            Category linesCat = _document.Settings.Categories.get_Item(BuiltInCategory.OST_Lines);
            if (linesCat == null)
                return lineModels;

            foreach (Category subCat in linesCat.SubCategories)
            {
                _token.ThrowIfCancellationRequested();

                if (subCat.Name.StartsWith('<') && subCat.Name.EndsWith('>'))
                    continue;

                int count = 0;
                if (subCat.Id == new ElementId(BuiltInCategory.OST_Lines))
                {
                    count += new FilteredElementCollector(_document)
                        .OfClass(typeof(CurveElement))
                        .WhereElementIsNotElementType()
                        .Where(e => e.Category != null && e.Category.Id == subCat.Id)
                        .Count();
                }

                IEnumerable<View> views = new FilteredElementCollector(_document)
                    .OfClass(typeof(View))
                    .Cast<View>()
                    .Where(v => !v.IsTemplate);

                foreach (View view in views)
                {
                    _token.ThrowIfCancellationRequested();

                    try
                    {
                        count += new FilteredElementCollector(_document, view.Id)
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

        public async Task<List<LineModel>> CreateModelsAsync()
        {
            return await Task.Run(() =>
            {
                _token.ThrowIfCancellationRequested();
                return CreateModels();
            }, _token);
        }
    }
}

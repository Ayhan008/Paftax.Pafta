using Autodesk.Revit.DB;
using Paftax.Pafta.Shared.Models.Element;

namespace Paftax.Pafta.Revit2026.Factories
{
    internal class SheetModelFactory(Document document, bool isCancellable = false)
    {
        private readonly Document _document = document;
        private readonly CancellationToken _token = isCancellable ? new CancellationToken() : CancellationToken.None;

        public List<SheetModel> CreateFromSheets()
        {
            List<SheetModel> sheets = [];

            IEnumerable<ViewSheet> revitSheets = new FilteredElementCollector(_document)
                .OfClass(typeof(ViewSheet))
                .Cast<ViewSheet>();

            foreach (ViewSheet revitSheet in revitSheets)
            {
                _token.ThrowIfCancellationRequested();

                SheetModel sheetModel = new()
                {
                    Id = revitSheet.Id.Value,
                    Title = revitSheet.Title,
                    Name = revitSheet.Name,
                    Number = revitSheet.SheetNumber,
                    ViewType = revitSheet.ViewType.ToString(),
                    IsPlaceholder = revitSheet.IsPlaceholder
                };
                sheets.Add(sheetModel);
            }
            return sheets;
        }
    }
}

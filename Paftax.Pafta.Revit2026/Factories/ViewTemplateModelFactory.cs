using Autodesk.Revit.DB;
using Paftax.Pafta.Shared.Models;

namespace Paftax.Pafta.Revit2026.Factories
{
    internal class ViewTemplateModelFactory(Document document)
    {
        private readonly Document _document = document;
        public List<ViewTemplateModel> CreateModels()
        {
            List<ViewTemplateModel> viewTemplateModels = [];

            IEnumerable<View> viewTemplates = new FilteredElementCollector(_document)
                .OfClass(typeof(View))
                .Cast<View>()
                .Where(v => v.IsTemplate);

            IEnumerable<View> normalViews = new FilteredElementCollector(_document)
                .OfClass(typeof(View))
                .Cast<View>()
                .Where(v => !v.IsTemplate);

            foreach (var template in viewTemplates)
            {
                int viewCount = normalViews.Count(v => v.ViewTemplateId == template.Id);

                viewTemplateModels.Add(new ViewTemplateModel
                {
                    Id = template.Id.Value,
                    Name = template.Name,
                    ViewCount = viewCount,
                    IsChecked = false
                });
            }

            return viewTemplateModels;
        }
    }
}

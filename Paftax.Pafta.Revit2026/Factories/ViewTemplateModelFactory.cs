using Autodesk.Revit.DB;
using Paftax.Pafta.Revit2026.Utilities;
using Paftax.Pafta.Shared.Models;

namespace Paftax.Pafta.Revit2026.Factories
{
    internal class ViewTemplateModelFactory
    {
        public static List<ViewTemplateModel> CreateModels(Document document)
        {

            IEnumerable<View> viewTemplates = new FilteredElementCollector(document)
                .OfClass(typeof(View))
                .Cast<View>()
                .Where(v => v.IsTemplate);

            var normalViews = new FilteredElementCollector(document)
                .OfClass(typeof(View))
                .Cast<View>()
                .Where(v => !v.IsTemplate)
                .ToList();

            List<ViewTemplateModel> models = [];

            foreach (var template in viewTemplates)
            {
                int viewCount = normalViews.Count(v => v.ViewTemplateId == template.Id);

                models.Add(new ViewTemplateModel
                {
                    Id = template.Id.Value,
                    Name = template.Name,
                    ViewCount = viewCount,
                    IsChecked = false
                });
            }

            return models;
        }
    }
}

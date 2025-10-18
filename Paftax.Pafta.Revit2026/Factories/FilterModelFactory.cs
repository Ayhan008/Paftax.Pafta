using Autodesk.Revit.DB;
using Paftax.Pafta.Shared.Models;

namespace Paftax.Pafta.Revit2026.Factories
{
    internal class FilterModelFactory
    {
        private static volatile bool _cancelRequested = false;
        public static bool CancelRequested
        {
            get => _cancelRequested;
            set => _cancelRequested = value;
        }

        /// <summary>
        /// Creates a list of FilterModel instances representing parameter filters in the given Revit document.
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        public static List<FilterModel> CreateModels(Document document)
        {
            List<FilterModel> filterModels = [];

            IEnumerable<ParameterFilterElement> parameterFilters = new FilteredElementCollector(document)
                .OfClass(typeof(ParameterFilterElement))
                .Cast<ParameterFilterElement>()
                .Where(f => f.IsValidObject);

            IEnumerable<View> viewTemplates = new FilteredElementCollector(document)
                .OfClass(typeof(View))
                .Cast<View>()
                .Where(v => v.IsTemplate && v.AreGraphicsOverridesAllowed());

            foreach (ParameterFilterElement parameterFilter in parameterFilters)
            {
                if (CancelRequested)
                    break;

                int usageCount = 0;

                foreach (View viewTemplate in viewTemplates)
                {
                    if (CancelRequested)
                        break;

                    ICollection<ElementId> assignedFilters = viewTemplate.GetFilters();
                    if (assignedFilters.Contains(parameterFilter.Id))
                    {
                        usageCount++;
                    }
                }

                filterModels.Add(new FilterModel
                {
                    Id = parameterFilter.Id.Value,
                    Name = parameterFilter.Name,
                    TemplateCount = usageCount,
                    IsChecked = false
                });
            }

            return filterModels;
        }
    }
}

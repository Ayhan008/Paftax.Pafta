using Autodesk.Revit.DB;
using Paftax.Pafta.Revit2026.Utilities;
using Paftax.Pafta.Shared.Models;

namespace Paftax.Pafta.Revit2026.Factories
{
    internal class FilterModelFactory
    {
        /// <summary>
        /// Creates a list of FilterModel instances representing parameter filters in the given Revit document.
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        public static List<FilterModel> CreateModels(Document document)
        {
            // All filter models
            List<FilterModel> filterModels = [];

            // Get all parameter filters in the document
            IEnumerable<ParameterFilterElement> parameterFilters = new FilteredElementCollector(document)
                .OfClass(typeof(ParameterFilterElement))
                .Cast<ParameterFilterElement>()
                .Where(f => f.IsValidObject);

            // Get all view templates in the document
            IEnumerable<View> viewTemplates = new FilteredElementCollector(document)
                .OfClass(typeof(View))
                .Cast<View>()
                .Where(v => v.IsTemplate && v.AreGraphicsOverridesAllowed());

            // For each parameter filter, count how many view templates use it
            foreach (ParameterFilterElement parameterFilter in parameterFilters)
            {
                int usageCount = 0; // Initialize usage count
                foreach (View viewTemplate in viewTemplates)
                {
                    // Get the filters assigned to the view template
                    ICollection<ElementId> assignedFilters = viewTemplate.GetFilters();
                    if (assignedFilters.Contains(parameterFilter.Id))
                    {
                        usageCount++; // Increment count if the filter is used in this view template
                    }
                }
                // Create and add the filter model to the list
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

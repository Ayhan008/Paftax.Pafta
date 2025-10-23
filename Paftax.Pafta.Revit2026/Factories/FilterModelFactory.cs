using Autodesk.Revit.DB;
using Paftax.Pafta.Shared.Models;

internal class FilterModelFactory
{
    private readonly Document? _document;
    private readonly IEnumerable<ParameterFilterElement>? _providedFilters;
    private CancellationToken _token = CancellationToken.None;

    public FilterModelFactory(Document document)
    {
        _document = document;
    }

    public FilterModelFactory(IEnumerable<ParameterFilterElement> parameterFilters)
    {
        _providedFilters = parameterFilters;
    }
    public FilterModelFactory Cancellable(CancellationToken token)
    {
        _token = token;
        return this;
    }

    public List<FilterModel> CreateModels()
    {
        List<FilterModel> models = [];

        IEnumerable<ParameterFilterElement> parameterFilters;

        if (_providedFilters is not null)
        {
            parameterFilters = _providedFilters.Where(f => f.IsValidObject);
        }
        else
        {
            ArgumentNullException.ThrowIfNull(_document, nameof(_document));
            parameterFilters = new FilteredElementCollector(_document)
                .OfClass(typeof(ParameterFilterElement))
                .Cast<ParameterFilterElement>()
                .Where(f => f.IsValidObject);
        }

        IEnumerable<View> viewTemplates = [];
        if (_document is not null)
        {
            viewTemplates = new FilteredElementCollector(_document)
                .OfClass(typeof(View))
                .Cast<View>()
                .Where(v => v.IsTemplate && v.AreGraphicsOverridesAllowed());
        }

        foreach (var parameterFilter in parameterFilters)
        {
            _token.ThrowIfCancellationRequested();

            int usageCount = 0;

            foreach (var viewTemplate in viewTemplates)
            {
                _token.ThrowIfCancellationRequested();
                if (viewTemplate.GetFilters().Contains(parameterFilter.Id))
                    usageCount++;
            }

            models.Add(new FilterModel
            {
                Id = parameterFilter.Id.Value,
                Name = parameterFilter.Name,
                TemplateCount = usageCount,
                IsChecked = false
            });
        }

        return models;
    }
}

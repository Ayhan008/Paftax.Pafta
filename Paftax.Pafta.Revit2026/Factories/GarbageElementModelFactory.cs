using Autodesk.Revit.DB;
using Paftax.Pafta.Shared.Models;

namespace Paftax.Pafta.Revit2026.Factories
{
    internal class GarbageElementModelFactory(Document document, bool isCancellable = false)
    {
        private readonly Document _document = document;
        private readonly CancellationToken _token = isCancellable ? new CancellationToken() : CancellationToken.None;

        public List<GarbageElementModel> CreateFromLines()
        {
            List<GarbageElementModel> lines = [];

            Category category = _document.Settings.Categories.get_Item(BuiltInCategory.OST_Lines);

            foreach (Category subCategory in category.SubCategories)
            {
                _token.ThrowIfCancellationRequested();

                if (subCategory.Name.StartsWith('<') && subCategory.Name.EndsWith('>'))
                    continue;

                int count = new FilteredElementCollector(_document)
                    .OfClass(typeof(CurveElement))
                    .WhereElementIsNotElementType()
                    .Where(e => e.Category != null && e.Category.Id == subCategory.Id)
                    .Count();

                GarbageElementModel lineModel = new()
                {
                    Id = subCategory.Id.Value,
                    Name = subCategory.Name,
                    Count = count,
                    IsUsed = count > 0
                };
                lines.Add(lineModel);
            }
            return lines;
        }

        public List<GarbageElementModel> CreateFromMaterials()
        {
            List<GarbageElementModel> materials = [];

            IEnumerable<Material> revitMaterials = new FilteredElementCollector(_document)
                .OfClass(typeof(Material))
                .Cast<Material>();

            IEnumerable<Element> elements = new FilteredElementCollector(_document)
                .WhereElementIsNotElementType()
                .ToElements();

            var materialCounts = new Dictionary<ElementId, int>();

            foreach (var elem in elements)
            {
                _token.ThrowIfCancellationRequested();

                var matIds = elem.GetMaterialIds(false);

                foreach (var id in matIds)
                {
                    _token.ThrowIfCancellationRequested();

                    if (!materialCounts.TryAdd(id, 1))
                        materialCounts[id]++;
                }
            }

            foreach (var mat in revitMaterials)
            {
                _token.ThrowIfCancellationRequested();

                GarbageElementModel materialModel = new()
                {
                    Id = mat.Id.Value,
                    Name = mat.Name,
                    Count = materialCounts.TryGetValue(mat.Id, out int value) ? value : 0,
                    IsUsed = materialCounts.ContainsKey(mat.Id) && materialCounts[mat.Id] > 0
                };
                materials.Add(materialModel);
            }

            return materials;
        }

        public List<GarbageElementModel> CreateFromViews()
        {
            List<GarbageElementModel> views = [];

            IEnumerable<View> revitViews = new FilteredElementCollector(_document)
                .OfClass(typeof(View))
                .Cast<View>();

            foreach (View view in revitViews)
            {
                _token.ThrowIfCancellationRequested();

                bool isPlaced = true;

                if (view.GetPlacementOnSheetStatus() == ViewPlacementOnSheetStatus.NotPlaced)
                {
                    isPlaced = false;
                }
                GarbageElementModel viewModel = new()
                {
                    Id = view.Id.Value,
                    Name = view.Name,
                    Count = isPlaced ? 1 : 0,
                    IsUsed = isPlaced
                };
                views.Add(viewModel);
            }
            return views;
        }

        public List<GarbageElementModel> CreateFromFilters()
        {
            List<GarbageElementModel> filters = [];

            IEnumerable<ParameterFilterElement> revitFilters = new FilteredElementCollector(_document)
                .OfClass(typeof(ParameterFilterElement))
                .Cast<ParameterFilterElement>();

            IEnumerable<View> viewTemplates = new FilteredElementCollector(_document)
                .OfClass(typeof(View))
                .Cast<View>()
                .Where(v => v.IsTemplate && v.AreGraphicsOverridesAllowed());

            foreach (ParameterFilterElement filter in revitFilters)
            {
                _token.ThrowIfCancellationRequested();

                int count = viewTemplates.Count(v => v.GetFilters().Contains(filter.Id));

                GarbageElementModel filterModel = new()
                {
                    Id = filter.Id.Value,
                    Name = filter.Name,
                    Count = count,
                    IsUsed = count > 0
                };
                filters.Add(filterModel);
            }
            return filters;
        }

        public List<GarbageElementModel> CreateFromViewTemplates()
        {
            List<GarbageElementModel> viewTemplates = [];

            IEnumerable<View> revitViewTemplates = new FilteredElementCollector(_document)
                .OfClass(typeof(View))
                .Cast<View>()
                .Where(v => v.IsTemplate);

            IEnumerable<View> normalViews = new FilteredElementCollector(_document)
                .OfClass(typeof(View))
                .Cast<View>()
                .Where(v => !v.IsTemplate);

            foreach (View template in revitViewTemplates)
            {
                int count = normalViews.Count(v => v.ViewTemplateId == template.Id);

                GarbageElementModel viewTemplateModel = new()
                {
                    Id = template.Id.Value,
                    Name = template.Name,
                    Count = count,
                    IsUsed = count > 0
                };
                viewTemplates.Add(viewTemplateModel);
            }
            return viewTemplates;
        }
    }
}

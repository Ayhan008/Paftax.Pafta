using Autodesk.Revit.DB;
using Paftax.Pafta.Shared.Models;

namespace Paftax.Pafta.Revit2026.Factories
{
    internal class MaterialModelFactory(Document document)
    {
        private readonly Document _document = document;
        private CancellationToken _token = CancellationToken.None;

        public MaterialModelFactory Cancellable(CancellationToken token)
        {
            _token = token;
            return this;
        }

        public List<MaterialModel> CreateModels()
        {
            var materials = new FilteredElementCollector(_document)
                .OfClass(typeof(Material))
                .Cast<Material>()
                .ToList();

            var elements = new FilteredElementCollector(_document)
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

            var models = new List<MaterialModel>();

            foreach (var mat in materials)
            {
                _token.ThrowIfCancellationRequested();

                models.Add(new MaterialModel
                {
                    Id = mat.Id.Value,
                    Name = mat.Name,
                    Count = materialCounts.TryGetValue(mat.Id, out int count) ? count : 0
                });
            }

            return models;
        }
    }
}

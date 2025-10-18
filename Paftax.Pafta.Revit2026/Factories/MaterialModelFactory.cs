using Autodesk.Revit.DB;
using Paftax.Pafta.Revit2026.Utilities;
using Paftax.Pafta.Shared.Models;

namespace Paftax.Pafta.Revit2026.Factories
{
    internal class MaterialModelFactory
    {
        public static List<MaterialModel> CreateModels(Document document)
        {
            var materials = new FilteredElementCollector(document)
                .OfClass(typeof(Material))
                .Cast<Material>()
                .ToList();

            var elements = new FilteredElementCollector(document)
                .WhereElementIsNotElementType()
                .ToElements();

            var materialCounts = new Dictionary<ElementId, int>();

            foreach (var elem in elements)
            {
                var matIds = elem.GetMaterialIds(false);
                foreach (var id in matIds)
                {
                    if (!materialCounts.TryAdd(id, 1))
                        materialCounts[id]++;
                }
            }

            var models = materials.Select(mat => new MaterialModel
            {
                Id = mat.Id.Value,
                Name = mat.Name,
                Count = materialCounts.TryGetValue(mat.Id, out int count) ? count : 0
            }).ToList();

            return models;
        }
    }
}

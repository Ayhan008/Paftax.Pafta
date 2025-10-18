using Autodesk.Revit.DB;
using Paftax.Pafta.Shared.Models;

namespace Paftax.Pafta.Revit2026.Factories
{
    internal class MaterialModelFactory
    {
        private static volatile bool _cancelRequested = false;
        public static bool CancelRequested
        {
            get => _cancelRequested;
            set => _cancelRequested = value;
        }

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
                if (CancelRequested)
                    break;

                var matIds = elem.GetMaterialIds(false);
                foreach (var id in matIds)
                {
                    if (CancelRequested)
                        break;

                    if (!materialCounts.TryAdd(id, 1))
                        materialCounts[id]++;
                }
            }

            var models = new List<MaterialModel>();

            foreach (var mat in materials)
            {
                if (CancelRequested)
                    break;

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

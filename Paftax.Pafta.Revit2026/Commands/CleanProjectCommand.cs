using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Paftax.Pafta.Revit2026.Factories;
using Paftax.Pafta.Shared.Models;
using Paftax.Pafta.UI.Models;
using Paftax.Pafta.UI.ViewModels;
using Paftax.Pafta.UI.Views;
using System.Windows.Controls;

namespace Paftax.Pafta.Revit2026.Commands
{
    [Transaction(TransactionMode.Manual)]
    internal class CleanProjectCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication app = commandData.Application;
            Document doc = app.ActiveUIDocument.Document;

            var filterableViews = CreateFilterable(doc,
                f => f.CreateFromViews(),
                unUsedLabel: "Unplaced");

            var filterableLines = CreateFilterable(doc,
                f => f.CreateFromLines(),
                unUsedLabel: "Unplaced");

            var filterableMaterials = CreateFilterable(doc,
                f => f.CreateFromMaterials(),
                unUsedLabel: "Unused");

            var filterableTemplates = CreateFilterable(doc,
                f => f.CreateFromViewTemplates(),
                unUsedLabel: "Unused");

            var filterableFilters = CreateFilterable(doc,
                f => f.CreateFromFilters(),
                unUsedLabel: "Unused");

            var cleanVM = new CleanProjectViewModel<GarbageElementModel>(
                filterableFilters,
                filterableViews,
                filterableLines,
                filterableMaterials,
                filterableTemplates);

            CleanProjectWindow window = new(cleanVM);

            cleanVM.CancelAction += () =>
            {
                window.DialogResult = false;
                window.Close();
            };

            cleanVM.CleanAction += () =>
            {
                window.DialogResult = true;
                window.Close();
                ExecuteClean(doc,
                    cleanVM.FilterableViewsViewModel,
                    cleanVM.FilterableLinesViewModel,
                    cleanVM.FilterableMaterialsViewModel,
                    cleanVM.FilterableViewTemplatesViewModel,
                    cleanVM.FilterableParameterFiltersViewModel);
            };

            window.ShowDialog();
            return Result.Succeeded;
        }

        private static FilterableElementCollectionViewModel<GarbageElementModel>
            CreateFilterable(
                Document doc,
                Func<GarbageElementModelFactory, List<GarbageElementModel>> createFunc,
                string unUsedLabel)
        {
            var factory = new GarbageElementModelFactory(doc);
            var models = createFunc(factory);

            var selectable = models
                .Select(m => new SelectableElementViewModel<GarbageElementModel>(m))
                .ToList();

            var columns = CreateDefaultColumns();
            var combo = CreateFilterComboboxItems(unUsedLabel);

            return new FilterableElementCollectionViewModel<GarbageElementModel>(
                selectable, combo, columns);
        }

        private static List<DataGridTextColumnDefinition<GarbageElementModel>> CreateDefaultColumns() =>
        [
            new DataGridTextColumnDefinition<GarbageElementModel>
            {
                Header = "Name",
                BindingPath = vm => vm.Model.Name,
                Width = new DataGridLength(1, DataGridLengthUnitType.Star)
            },
            new DataGridTextColumnDefinition<GarbageElementModel>
            {
                Header = "Count",
                BindingPath = vm => vm.Model.Count.ToString(),
                Width = new DataGridLength(100)
            }
        ];

        private static List<ComboBoxItemDefinition<GarbageElementModel>> CreateFilterComboboxItems(string unusedLabel) =>
        [
            new ComboBoxItemDefinition<GarbageElementModel>
            {
                DisplayName = "All",
                FilterFunction = _ => true
            },
            new ComboBoxItemDefinition<GarbageElementModel>
            {
                DisplayName = unusedLabel,
                FilterFunction = vm => !vm.Model.IsUsed
            }
        ];

        private static void ExecuteClean(
            Document doc,
            params FilterableElementCollectionViewModel<GarbageElementModel>[] collections)
        {
            using Transaction t = new(doc, "Clean Project");
            t.Start();

            var ids = collections
                .SelectMany(c => c.Elements.Where(e => e.IsChecked))
                .Select(e => new ElementId(e.Model.Id))
                .ToList();

            if (ids.Count > 0)
                doc.Delete(ids);

            t.Commit();
        }
    }
}

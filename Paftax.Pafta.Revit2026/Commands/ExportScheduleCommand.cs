using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Paftax.Pafta.Revit2026.Factories;
using Paftax.Pafta.Shared.Models.Element;
using Paftax.Pafta.UI.Models;
using Paftax.Pafta.UI.ViewModels;
using Paftax.Pafta.UI.Views;
using System.Windows.Controls;

namespace Paftax.Pafta.Revit2026.Commands
{
    [Transaction(TransactionMode.Manual)]
    internal class ExportScheduleCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiApplication = commandData.Application;
            UIDocument uiDocument = uiApplication.ActiveUIDocument;
            Document document = uiDocument.Document;

            List<ViewSchedule> viewSchedules = GetViewSchedules(document);
            List<ScheduleModel> scheduleModels = ScheduleModelFactory.Create(viewSchedules);

            List<SelectableElementViewModel<ScheduleModel>> selectableElementViewModels = [.. scheduleModels.Select(s => new SelectableElementViewModel<ScheduleModel>(s))];
            List<DataGridTextColumnDefinition<ScheduleModel>> dataGridTextColumnDefinitions =
            [
                new DataGridTextColumnDefinition<ScheduleModel>
                {
                    Header = "Name",
                    BindingPath = s => s.Model.Name,
                    Width = new DataGridLength(1, DataGridLengthUnitType.Star),
                    CanResize = false
                }
            ];

            SearchableElementCollectionViewModel<ScheduleModel> searchableElementCollectionViewModel = new(selectableElementViewModels, dataGridTextColumnDefinitions);
            ExportScheduleViewModel<ScheduleModel> exportScheduleViewModel = new(searchableElementCollectionViewModel);

            ExportScheduleWindow exportScheduleWindow = new(exportScheduleViewModel);

            exportScheduleViewModel.ExportAction += () =>
            {
                exportScheduleWindow.Close();

                if (exportScheduleViewModel.IsMerged)
                {
                    List<ScheduleModel> selectedSchedules = [.. exportScheduleViewModel.ScheduleSelectionViewModel.Elements.Where(e => e.IsChecked).Select(e => e.Model)];

                    foreach (ScheduleModel scheduleModel in selectedSchedules)
                    {
                        // Export logic for merged schedules goes here
                    }
                }
                else if (exportScheduleViewModel.IsSeperated)
                {
                }
            };

            exportScheduleViewModel.CloseAction += () => exportScheduleWindow.Close();


            return Result.Succeeded;
        }

        private static List<ViewSchedule> GetViewSchedules(Document document)
        {
            return [.. new FilteredElementCollector(document)
                .OfClass(typeof(ViewSchedule))
                .Cast<ViewSchedule>()];
        }
    }
}
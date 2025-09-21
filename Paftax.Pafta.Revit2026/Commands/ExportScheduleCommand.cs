using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using DocumentFormat.OpenXml.Packaging;
using Paftax.Pafta.Revit2026.Models;
using Paftax.Pafta.Revit2026.Services.OpenXml;
using Paftax.Pafta.Revit2026.Services.OpenXml.Stylesheets;
using Paftax.Pafta.Revit2026.Services.Revit;
using Paftax.Pafta.Revit2026.Utilities;
using Paftax.Pafta.UI;
using Paftax.Pafta.UI.Models;
using Paftax.Pafta.UI.ViewModels;

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
            List<ScheduleModel> scheduleModels = [.. Mappers.MapToScheduleModels(viewSchedules)];

            ExportScheduleViewModel exportScheduleViewModel = new();
            exportScheduleViewModel.LoadSchedules(scheduleModels);

            MainWindow mainWindow = new(exportScheduleViewModel)
            {
                Title = "Export Schedules",
                Width = 400,
                Height = 700,       
            };

            mainWindow.ShowDialog();

            if (exportScheduleViewModel.CanExport == true)
            {
                List<ScheduleModel> selectedSchedules = exportScheduleViewModel.GetSelectedSchedules();
                List<ViewSchedule> selectedViewSchedules = [.. Mappers.MapToViewSchedules(selectedSchedules, document)];

                if (exportScheduleViewModel.IsMerged == true)
                {
                    ExportSchedulesMerged(selectedViewSchedules, exportScheduleViewModel.ExportFolderPath);
                }

                if (exportScheduleViewModel.IsSeperated == true)
                {
                    ExportSchedulesSeperate(selectedViewSchedules, exportScheduleViewModel.ExportFolderPath);
                }
                return Result.Succeeded;
            }
            return Result.Cancelled;
        }

        private static List<ViewSchedule> GetViewSchedules(Document document)
        {
            FilteredElementCollector collector = new(document);
            collector.OfClass(typeof(ViewSchedule));

            List<ViewSchedule> schedules = [.. collector.Cast<ViewSchedule>()];

            return schedules;
        }
        private static void ExportSchedulesSeperate(List<ViewSchedule> viewSchedules, string folderPath)
        {
            List<ScheduleTableDataModel> scheduleTableDatas = ScheduleTableDataService.CreateScheduleTableData(viewSchedules);

            foreach (ScheduleTableDataModel scheduleTableData in scheduleTableDatas)
            {
                string safeFileName = FileUtilities.MakeValidFileName(scheduleTableData.Name);
                string filePath = Path.Combine(folderPath, $"{safeFileName}.xlsx");

                if (FileUtilities.IsFileOpen(filePath))
                {
                    TaskDialog.Show("Export", $"One or more files are open\nPlease close the files and try again.");
                    break;
                }

                SpreadsheetDocument spreadsheetDocument = WorkbookService.CreateSpreadsheetWorkbook(filePath, scheduleTableData.Name)
                    ?? throw new InvalidOperationException("Failed to create spreadsheet document.");

                StyleService.AddStylesPart(spreadsheetDocument, ScheduleStylesheets.GenericStylesheet());
                SheetService.FillSheet(spreadsheetDocument, scheduleTableData.Name, scheduleTableData.TitlePart, 0, 1);
                SheetService.SetCustomRowHeight(spreadsheetDocument, scheduleTableData.Name, 1, 24);

                SheetService.FillSheet(spreadsheetDocument, scheduleTableData.Name, scheduleTableData.HeaderPart, 1, 2);
                SheetService.FillSheet(spreadsheetDocument, scheduleTableData.Name, scheduleTableData.BodyPart, 2, 2);

                SheetService.MergeCells(spreadsheetDocument, scheduleTableData.Name, scheduleTableData.MergedCells);

                SheetService.SetColumnWidthsFromData(spreadsheetDocument, scheduleTableData.Name, scheduleTableData.TableData);
                spreadsheetDocument.Dispose();
            }
        }

        private static void ExportSchedulesMerged(List<ViewSchedule> viewSchedules, string folderPath)
        {
            string filePath = Path.Combine(folderPath, "MergedSchedules.xlsx");
            List<ScheduleTableDataModel> scheduleTableDatas = ScheduleTableDataService.CreateScheduleTableData(viewSchedules);
            List<string> sheetNames = [.. scheduleTableDatas.Select(s => s.Name)];

            if (FileUtilities.IsFileOpen(filePath))
            {
                TaskDialog.Show("Export", $"The file is open\nPlease close the file and try again.");
                return;
            }

            SpreadsheetDocument spreadsheetDocument = WorkbookService.CreateSpreadsheetWorkbook(filePath, sheetNames)
                ?? throw new InvalidOperationException("Failed to create spreadsheet document.");

            StyleService.AddStylesPart(spreadsheetDocument, ScheduleStylesheets.GenericStylesheet());

            foreach (ScheduleTableDataModel scheduleTableData in scheduleTableDatas)
            {
                SheetService.FillSheet(spreadsheetDocument, scheduleTableData.Name, scheduleTableData.TitlePart, 0, 1);
                SheetService.SetCustomRowHeight(spreadsheetDocument, scheduleTableData.Name, 1, 24);

                SheetService.FillSheet(spreadsheetDocument, scheduleTableData.Name, scheduleTableData.HeaderPart, 1, 2);
                SheetService.FillSheet(spreadsheetDocument, scheduleTableData.Name, scheduleTableData.BodyPart, 2, 2);

                SheetService.MergeCells(spreadsheetDocument, scheduleTableData.Name, scheduleTableData.MergedCells);

                SheetService.SetColumnWidthsFromData(spreadsheetDocument, scheduleTableData.Name, scheduleTableData.TableData);
            }
            spreadsheetDocument.Dispose();
        }
    }
}

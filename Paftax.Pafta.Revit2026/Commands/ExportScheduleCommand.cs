using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using DocumentFormat.OpenXml.Packaging;
using Paftax.Pafta.Revit2026.Factories;
using Paftax.Pafta.Revit2026.Services;
using Paftax.Pafta.Revit2026.Utilities;
using Paftax.Pafta.Shared.Exporters.OpenXml;
using Paftax.Pafta.Shared.Exporters.OpenXml.Stylesheets;
using Paftax.Pafta.Shared.Models;
using Paftax.Pafta.UI.Services;
using System.Windows;

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

            List<ScheduleModel> scheduleModels = new ScheduleModelFactory(document).CreateModels();

            ExportScheduleDialogService exportScheduleDialogService = new();
            exportScheduleDialogService.LoadSchedules(scheduleModels);

            exportScheduleDialogService.ExportAction += () =>
            {
                List<ScheduleModel> selectedSchedules = exportScheduleDialogService.SelectedSchedules;
                List<ViewSchedule> selectedViewSchedules = new ElementCollectorService(document).GetElementsByIds<ViewSchedule>(selectedSchedules.Select(s => s.Id));


                if (exportScheduleDialogService.IsMerged == true)
                {
                    ExportSchedulesMerged(selectedViewSchedules, exportScheduleDialogService.ExportFolderPath);
                }

                if (exportScheduleDialogService.IsSeperated == true)
                {
                    ExportSchedulesSeperate(selectedViewSchedules, exportScheduleDialogService.ExportFolderPath);
                }
            };
            Window window = exportScheduleDialogService.ShowDialog();
            return Result.Succeeded;
        }

        private static void ExportSchedulesSeperate(List<ViewSchedule> viewSchedules, string folderPath)
        {
            List<ScheduleTableData> scheduleTableDatas = ScheduleTableDataFactory.FromViewSchedules(viewSchedules);

            foreach (ScheduleTableData scheduleTableData in scheduleTableDatas)
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
                SheetService sheetService = new(spreadsheetDocument, scheduleTableData.Name);

                sheetService.FillSheet(scheduleTableData.TitlePart, 0, 1);
                sheetService.SetCustomRowHeight(1, 24);

                sheetService.FillSheet(scheduleTableData.HeaderPart, 1, 2);
                sheetService.FillSheet(scheduleTableData.BodyPart, 2, scheduleTableData.HeaderRowCount + 1);

                sheetService.MergeCells(scheduleTableData.MergedCells);
                sheetService.SetColumnWidthsFromData(scheduleTableData.TableData);

                spreadsheetDocument.Dispose();
            }
        }

        private static void ExportSchedulesMerged(List<ViewSchedule> viewSchedules, string folderPath)
        {
            string filePath = Path.Combine(folderPath, "MergedSchedules.xlsx");
            List<ScheduleTableData> scheduleTableDatas = ScheduleTableDataFactory.FromViewSchedules(viewSchedules);
            List<string> sheetNames = [.. scheduleTableDatas.Select(s => s.Name)];

            if (FileUtilities.IsFileOpen(filePath))
            {
                TaskDialog.Show("Export", $"The file is open\nPlease close the file and try again.");
                return;
            }

            SpreadsheetDocument spreadsheetDocument = WorkbookService.CreateSpreadsheetWorkbook(filePath, sheetNames)
                ?? throw new InvalidOperationException("Failed to create spreadsheet document.");

            StyleService.AddStylesPart(spreadsheetDocument, ScheduleStylesheets.GenericStylesheet());

            foreach (ScheduleTableData scheduleTableData in scheduleTableDatas)
            {
                SheetService sheetService = new(spreadsheetDocument, scheduleTableData.Name);

                sheetService.FillSheet(scheduleTableData.TitlePart, 0, 1);
                sheetService.SetCustomRowHeight(1, 24);

                sheetService.FillSheet(scheduleTableData.HeaderPart, 1, 2);
                sheetService.FillSheet(scheduleTableData.BodyPart, 2, scheduleTableData.HeaderRowCount + 1);

                sheetService.MergeCells(scheduleTableData.MergedCells);
                sheetService.SetColumnWidthsFromData(scheduleTableData.TableData);
            }
            spreadsheetDocument.Dispose();
        }
    }
}

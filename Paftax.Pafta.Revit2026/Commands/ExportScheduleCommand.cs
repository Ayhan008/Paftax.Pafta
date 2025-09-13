using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using DocumentFormat.OpenXml.Packaging;
using Paftax.Pafta.Revit2026.Models;
using Paftax.Pafta.Revit2026.Services.OpenXml;
using Paftax.Pafta.Revit2026.Services.OpenXml.Stylesheets;
using Paftax.Pafta.Revit2026.Services.Revit;

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

            ViewSchedule viewSchedule = GetViewSchedule(document);
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            return Result.Succeeded;
        }

        private static ViewSchedule GetViewSchedule(Document document)
        {
            FilteredElementCollector collector = new(document);
            collector.OfClass(typeof(ViewSchedule));
            List<ViewSchedule> schedules = [.. collector.Cast<ViewSchedule>()];
            if (schedules.Count == 0)
            {
                throw new InvalidOperationException("No schedules found in the document.");
            }
            return schedules[19];
        }

        private static void ExportSchedulesSeperate(List<ViewSchedule> viewSchedules, string folderPath)
        {
            List<ScheduleTableDataModel> scheduleTableDatas = ScheduleTableDataService.CreateScheduleTableData(viewSchedules);

            foreach (ScheduleTableDataModel scheduleTableData in scheduleTableDatas)
            {
                string filePath = Path.Combine(folderPath, $"{scheduleTableData.Name}.xlsx");
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

            SpreadsheetDocument spreadsheetDocument = WorkbookService.CreateSpreadsheetWorkbook(filePath, sheetNames)
                ?? throw new InvalidOperationException("Failed to create spreadsheet document.");

            foreach (ScheduleTableDataModel scheduleTableData in scheduleTableDatas)
            {
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
    }
}

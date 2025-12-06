using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Paftax.Pafta.Exporters.Spreadsheet.Stylesheets;
using Paftax.Pafta.Shared.Enums;
using Paftax.Pafta.Shared.Models;

namespace Paftax.Pafta.Exporters.Spreadsheet
{
    public class SpreadsheetExporter(TableDataCollection tableDataCollection, SpreadsheetOptions spreadsheetOptions)
    {
        private readonly TableDataCollection _tableDataCollection = tableDataCollection;
        private readonly SpreadsheetOptions _spreadsheetOptions = spreadsheetOptions;

        public void Export(string folderPath, SpreadsheetExportMode exportMode)
        {
            if (exportMode == SpreadsheetExportMode.Merged)
            {
                ExportMerged(folderPath);
            }
            else if (exportMode == SpreadsheetExportMode.Separate)
            {
                ExportSeparate(folderPath);
            }
        }

        private void ExportMerged(string folderPath)
        {
            string filePath = Path.Combine(folderPath, "Merged_Schedules.xlsx");

            using SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Create(filePath, SpreadsheetDocumentType.Workbook);
            WorkbookPart workbookPart = spreadsheetDocument.AddWorkbookPart();
            workbookPart.Workbook = new Workbook();

            var stylesPart = workbookPart.AddNewPart<WorkbookStylesPart>();
            var stylesheet = new ScheduleStylesheet();
            stylesPart.Stylesheet = stylesheet;
            stylesPart.Stylesheet.Save();

            Sheets sheets = workbookPart.Workbook.AppendChild(new Sheets());

            uint sheetId = 1;

            foreach (WorksheetTableData table in _tableDataCollection.Tables)
            {
                WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                worksheetPart.Worksheet = new Worksheet(new SheetData());

                Sheet sheet = new()
                {
                    Id = workbookPart.GetIdOfPart(worksheetPart),
                    SheetId = sheetId++,
                    Name = SanitizeSheetName(table.Title)
                };
                sheets.Append(sheet);

                uint currentRow = 1;
                currentRow = WriteGroupedCells(worksheetPart, table.TitlePart, currentRow, stylesheet.TitleStyleIndex);
                currentRow = WriteGroupedCells(worksheetPart, table.HeaderPart, currentRow, stylesheet.HeaderStyleIndex);
                currentRow = WriteGroupedCells(worksheetPart, table.BodyPart, currentRow, stylesheet.BodyStyleIndex);

                if (table.MergedCellReferences.Count > 0)
                {
                    var mergeCells = new MergeCells();
                    foreach (string reference in table.MergedCellReferences)
                        mergeCells.Append(new MergeCell { Reference = reference });

                    worksheetPart.Worksheet.InsertAfter(mergeCells, worksheetPart.Worksheet.GetFirstChild<SheetData>());
                }
                
                SetRowHeight(worksheetPart, 1, 30);
                SetColumnWidths(worksheetPart);
                FreezeRows(worksheetPart, (uint)(table.HeaderRowCount + 1));
            }

            workbookPart.Workbook.Save();
        }

        private void ExportSeparate(string folderPath)
        {
            foreach (WorksheetTableData table in _tableDataCollection.Tables)
            {
                string filePath = Path.Combine(folderPath, $"{SanitizeSheetName(table.Title)}.xlsx");

                using SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Create(filePath, SpreadsheetDocumentType.Workbook);

                WorkbookPart workbookPart = spreadsheetDocument.AddWorkbookPart();
                workbookPart.Workbook = new Workbook();

                var stylesPart = workbookPart.AddNewPart<WorkbookStylesPart>();
                var stylesheet = new ScheduleStylesheet();
                stylesPart.Stylesheet = stylesheet;
                stylesPart.Stylesheet.Save();

                Sheets sheets = workbookPart.Workbook.AppendChild(new Sheets());

                WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                worksheetPart.Worksheet = new Worksheet(new SheetData());

                Sheet sheet = new()
                {
                    Id = workbookPart.GetIdOfPart(worksheetPart),
                    SheetId = 1,
                    Name = SanitizeSheetName(table.Title)
                };
                sheets.Append(sheet);

                uint currentRow = 1;
                currentRow = WriteGroupedCells(worksheetPart, table.TitlePart, currentRow, stylesheet.TitleStyleIndex);
                currentRow = WriteGroupedCells(worksheetPart, table.HeaderPart, currentRow, stylesheet.HeaderStyleIndex);
                currentRow = WriteGroupedCells(worksheetPart, table.BodyPart, currentRow, stylesheet.BodyStyleIndex);

                if (table.MergedCellReferences.Count > 0)
                {
                    var mergeCells = new MergeCells();
                    foreach (string reference in table.MergedCellReferences)
                        mergeCells.Append(new MergeCell { Reference = reference });

                    worksheetPart.Worksheet.InsertAfter(mergeCells, worksheetPart.Worksheet.GetFirstChild<SheetData>());
                }
   
                SetRowHeight(worksheetPart, 1, 30);
                SetColumnWidths(worksheetPart);
                FreezeRows(worksheetPart, (uint)(table.HeaderRowCount + 1));
                workbookPart.Workbook.Save();
            }
        }

        private static uint WriteGroupedCells(WorksheetPart worksheetPart, List<List<CellData>> groups, uint startRow, uint defaultStyleIndex)
        {
            SheetData sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>()!;
            uint rowIndex = startRow;

            foreach (var rowGroup in groups)
            {
                Row row = new() { RowIndex = rowIndex };
                foreach (var cellModel in rowGroup)
                {
                    string cellReference = GetCellReference(cellModel.ColumnIndex, rowIndex);
                    Cell cell = new()
                    {
                        CellReference = cellReference,
                        DataType = CellValues.String,
                        CellValue = new CellValue(cellModel.Value),
                        StyleIndex = cellModel.StyleIndex ?? defaultStyleIndex
                    };
                    row.Append(cell);
                }
                sheetData.Append(row);
                rowIndex++;
            }

            return rowIndex;
        }

        private static string GetCellReference(uint columnIndex, uint rowIndex)
        {
            return GetColumnName(columnIndex) + rowIndex.ToString();
        }

        private static string GetColumnName(uint index)
        {
            uint dividend = index;
            string columnName = string.Empty;
            while (dividend > 0)
            {
                uint modulo = (dividend - 1) % 26;
                columnName = (char)('A' + modulo) + columnName;
                dividend = (dividend - modulo - 1) / 26;
            }
            return columnName;
        }

        private static string SanitizeSheetName(string name)
        {
            char[] invalid = [':', '\\', '/', '?', '*', '[', ']'];
            string cleaned = new([.. name.Where(c => !invalid.Contains(c))]);
            if (cleaned.Length > 31) cleaned = cleaned[..31];
            return string.IsNullOrWhiteSpace(cleaned) ? "Sheet" : cleaned;
        }

        private static void SetColumnWidths(WorksheetPart worksheetPart)
        {
            var sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();
            if (sheetData == null) return;

            var sharedStrings = worksheetPart.GetParentParts()
                .OfType<WorkbookPart>()
                .FirstOrDefault()?
                .SharedStringTablePart?
                .SharedStringTable;

            Dictionary<uint, int> columnMaxChar = [];

            foreach (var row in sheetData.Elements<Row>())
            {
                foreach (var cell in row.Elements<Cell>())
                {
                    uint colIndex = GetColumnIndex(cell.CellReference!);

                    string text = GetCellText(cell, sharedStrings); // Fix below

                    int len = text.Length;

                    if (!columnMaxChar.TryGetValue(colIndex, out int value))
                        columnMaxChar[colIndex] = len;
                    else if (len > value)
                        columnMaxChar[colIndex] = len;
                }
            }

            Columns columns = worksheetPart.Worksheet.GetFirstChild<Columns>() ?? new Columns();

            foreach (var kvp in columnMaxChar)
            {
                uint colIndex = kvp.Key;
                int maxChars = kvp.Value;

                double pixelWidth = maxChars * 6;

                double excelWidth = (pixelWidth - 5) / 7.0;

                columns.Append(new Column
                {
                    Min = colIndex,
                    Max = colIndex,
                    Width = excelWidth,
                    CustomWidth = true
                });
            }

            worksheetPart.Worksheet.InsertAt(columns, 0);
        }

        private static string GetCellText(Cell cell, SharedStringTable? sharedStrings) // Accept nullable
        {
            if (cell.CellValue == null)
                return string.Empty;

            string value = cell.CellValue.InnerText ?? string.Empty;

            if (cell.DataType != null && cell.DataType == CellValues.SharedString)
            {
                if (sharedStrings != null && int.TryParse(value, out int sharedIndex))
                    return sharedStrings.ElementAt(sharedIndex).InnerText;
            }

            return value;
        }

        private static uint GetColumnIndex(string cellReference)
        {
            string col = new([.. cellReference.Where(char.IsLetter)]);
            uint index = 0;

            foreach (char c in col)
                index = (index * 26) + (uint)(c - 'A' + 1);

            return index;
        }

        private static void SetRowHeight(WorksheetPart worksheetPart, uint rowIndex, double heightPoints)
        {
            var sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();
            if (sheetData == null) return;

            var row = sheetData.Elements<Row>().FirstOrDefault(r => r.RowIndex != null && r.RowIndex.Value == rowIndex);
            if (row == null)
            {
                row = new Row { RowIndex = rowIndex };

                var next = sheetData.Elements<Row>().FirstOrDefault(r => r.RowIndex != null && r.RowIndex.Value > rowIndex);
                if (next != null)
                    sheetData.InsertBefore(row, next);
                else
                    sheetData.Append(row);
            }

            row.Height = heightPoints; 
            row.CustomHeight = true;
        }

        private static void FreezeRows(WorksheetPart worksheetPart, uint lastFrozenRowIndex)
        {
            if (lastFrozenRowIndex == 0) return;

            Worksheet worksheet = worksheetPart.Worksheet;

            SheetViews? sheetViews = worksheet.GetFirstChild<SheetViews>();
            if (sheetViews == null)
            {
                sheetViews = new SheetViews();
                worksheet.InsertAt(sheetViews, 0);
            }

            SheetView? sheetView = sheetViews.Elements<SheetView>().FirstOrDefault();
            if (sheetView == null)
            {
                sheetView = new SheetView { WorkbookViewId = 0U };
                sheetViews.Append(sheetView);
            }

            // Remove any existing pane
            Pane? existingPane = sheetView.GetFirstChild<Pane>();
            if (existingPane != null) 
                existingPane?.Remove();

            // Append new pane
            sheetView.Append(new Pane
            {
                VerticalSplit = lastFrozenRowIndex,
                TopLeftCell = "A" + (lastFrozenRowIndex + 1),
                ActivePane = PaneValues.BottomLeft,
                State = PaneStateValues.Frozen
            });
        }
    }
}

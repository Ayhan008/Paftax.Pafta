using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Office2016.Excel;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Diagnostics;
using System.Reflection.Metadata;

namespace Paftax.Pafta.Shared.Exporters.OpenXml
{
    public class SheetService
    {
        /// <summary>
        /// Fills the specified sheet in the spreadsheet document with the provided data, starting from the given row and applying the specified style index.
        /// </summary>
        /// <param name="document"></param>
        /// <param name="sheetName"></param>
        /// <param name="data"></param>
        /// <param name="styleIndex"></param>
        /// <param name="startRow"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static Workbook FillSheet(SpreadsheetDocument document, string sheetName, List<List<string>> data, uint styleIndex, uint startRow)
        {
            WorkbookPart workbookPart = document.WorkbookPart ?? document.AddWorkbookPart();
            Sheets? sheets = workbookPart.Workbook.GetFirstChild<Sheets>() ?? throw new Exception("No sheets found in the workbook.");
            Sheet? sheet = sheets.Elements<Sheet>().FirstOrDefault(s => s.Name == sheetName) ?? throw new Exception($"Sheet with name '{sheetName}' not found.");
            WorksheetPart? worksheetPart = (WorksheetPart?)workbookPart.GetPartById(sheet.Id!) ?? throw new Exception($"WorksheetPart for sheet '{sheetName}' not found.");
            SheetData sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>() ?? new SheetData();

            for (int i = 0; i < data.Count; i++)
            {
                Row row = new() { RowIndex = (uint)(i + startRow) };
                for (int j = 0; j < data[i].Count; j++)
                {
                    Cell cell = new()
                    {
                        DataType = CellValues.String,
                        CellValue = new CellValue(data[i][j]),
                        StyleIndex = styleIndex
                    };
                    row.Append(cell);
                }
                sheetData.Append(row);
            }
            worksheetPart.Worksheet.Save();
            return workbookPart.Workbook;
        }

        /// <summary>
        /// Merges the specified cell references in the given sheet of the spreadsheet document.
        /// </summary>
        /// <param name="document"></param>
        /// <param name="sheetName"></param>
        /// <param name="cellReferences"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static Workbook MergeCells(SpreadsheetDocument document, string sheetName, HashSet<string> cellReferences)
        {
            WorkbookPart workbookPart = document.WorkbookPart ?? document.AddWorkbookPart();
            Sheets? sheets = workbookPart.Workbook.GetFirstChild<Sheets>() ?? throw new Exception("No sheets found in the workbook.");
            Sheet? sheet = sheets.Elements<Sheet>().FirstOrDefault(s => s.Name == sheetName) ?? throw new Exception($"Sheet with name '{sheetName}' not found.");
            WorksheetPart? worksheetPart = (WorksheetPart?)workbookPart.GetPartById(sheet.Id!) ?? throw new Exception($"WorksheetPart for sheet '{sheetName}' not found.");
            MergeCells mergeCells;
            if (worksheetPart.Worksheet.Elements<MergeCells>().Any())
            {
                mergeCells = worksheetPart.Worksheet.Elements<MergeCells>().First();
            }
            else
            {
                mergeCells = new MergeCells();
                // Insert the MergeCells element after the SheetData element.
                if (worksheetPart.Worksheet.Elements<SheetData>().Any())
                {
                    worksheetPart.Worksheet.InsertAfter(mergeCells, worksheetPart.Worksheet.Elements<SheetData>().First());
                }
                else
                {
                    worksheetPart.Worksheet.Append(mergeCells);
                }
            }
            foreach (string cellReference in cellReferences)
            {
                MergeCell mergeCell = new() { Reference = new StringValue(cellReference) };
                mergeCells.Append(mergeCell);
            }
            worksheetPart.Worksheet.Save();
            return workbookPart.Workbook;
        }

        /// <summary>
        /// Sets a custom height for the specified row in a worksheet within the given spreadsheet document.
        /// </summary>
        /// <remarks>This method sets the row's height and marks it as having a custom height. The change
        /// is immediately saved to the worksheet.</remarks>
        /// <param name="spreadsheetDocument">The spreadsheet document containing the workbook and worksheets to modify.</param>
        /// <param name="sheetName">The name of the worksheet in which to set the row height. Must match an existing sheet name.</param>
        /// <param name="rowIndex">The one-based index of the row to update. The row must exist in the worksheet.</param>
        /// <param name="rowHeight">The height to assign to the specified row, in points.</param>
        /// <returns>The updated Workbook object reflecting the change to the row height.</returns>
        /// <exception cref="Exception">Thrown if the workbook part, sheet, or sheet data cannot be found in the spreadsheet document.</exception>
        /// <exception cref="ArgumentNullException">Thrown if the specified row does not exist in the worksheet.</exception>
        public static Workbook SetCustomRowHeight(SpreadsheetDocument spreadsheetDocument, string sheetName, uint rowIndex, double rowHeight)
        {
            WorkbookPart workbookPart = spreadsheetDocument.WorkbookPart
                ?? throw new Exception("No WorkbookPart found.");

            // Find the sheet
            Sheet? sheet = workbookPart.Workbook.Sheets?
                .Elements<Sheet>()
                .FirstOrDefault(s => s.Name == sheetName)
                ?? throw new Exception($"Sheet '{sheetName}' not found.");

            WorksheetPart worksheetPart = (WorksheetPart)workbookPart.GetPartById(sheet.Id!);
            SheetData sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>()
                ?? throw new Exception("SheetData not found.");

            // Find the row by index (create if missing)
            Row? row = sheetData.Elements<Row>().FirstOrDefault(r => r.RowIndex! == rowIndex) 
                ?? throw new ArgumentNullException($"Row with index {rowIndex} not found.");
            row.Height = rowHeight;
            row.CustomHeight = true;

            worksheetPart.Worksheet.Save();
            return workbookPart.Workbook;
        }

        /// <summary>
        /// Sets column widths in the specified sheet based on the maximum length of data in each column.
        /// </summary>
        /// <param name="spreadsheetDocument"></param>
        /// <param name="sheetName"></param>
        /// <param name="data"></param>
        /// <exception cref="InvalidOperationException"></exception>
        public static void SetColumnWidthsFromData(SpreadsheetDocument spreadsheetDocument, string sheetName, List<List<string>> data)
        {
            if (data == null || data.Count == 0) return;

            WorkbookPart? workbookPart = spreadsheetDocument.WorkbookPart;
            Sheets? sheets = workbookPart?.Workbook.Sheets
                ?? throw new InvalidOperationException("Workbook does not contain any sheets.");
            Sheet sheet = sheets
                 .OfType<Sheet>()
                 .FirstOrDefault(s => s.Name == sheetName)
                 ?? throw new InvalidOperationException("Sheet not found");

            WorksheetPart worksheetPart = (WorksheetPart)workbookPart.GetPartById(sheet.Id!);

            // Find max length per column
            int columnCount = data.Max(r => r.Count);
            var maxLengths = new int[columnCount];

            foreach (var row in data)
            {
                for (int i = 0; i < row.Count; i++)
                {
                    int length = row[i]?.Length ?? 0;
                    if (length > maxLengths[i])
                        maxLengths[i] = length;
                }
            }

            // Apply column widths
            var columns = new Columns();
            for (int i = 0; i < maxLengths.Length; i++)
            {
                columns.Append(new Column
                {
                    Min = (uint)(i + 1),
                    Max = (uint)(i + 1),
                    Width = maxLengths[i] + 2, // padding
                    CustomWidth = true
                });
            }

            worksheetPart.Worksheet.InsertAt(columns, 0);
            worksheetPart.Worksheet.Save();
        }
    }
}

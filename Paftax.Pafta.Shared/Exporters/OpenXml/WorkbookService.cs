using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace Paftax.Pafta.Shared.Exporters.OpenXml
{
    public class WorkbookService
    {
        /// <summary>
        /// Creates a new spreadsheet document with a single sheet.
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="sheetName"></param>
        /// <returns></returns>
        public static SpreadsheetDocument CreateSpreadsheetWorkbook(string filePath, string sheetName)
        {
            return CreateSpreadsheetWorkbook(filePath, [sheetName]);
        }

        /// <summary>
        /// Creates a new spreadsheet document with multiple sheets.
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="sheetNames"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static SpreadsheetDocument CreateSpreadsheetWorkbook(string filePath, List<string> sheetNames)
        {
            if (sheetNames == null || sheetNames.Count == 0)
                throw new ArgumentException("Sheet names list cannot be empty.");

            // Create the spreadsheet document
            SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Create(filePath, SpreadsheetDocumentType.Workbook);

            // Add a WorkbookPart
            WorkbookPart workbookPart = spreadsheetDocument.AddWorkbookPart();
            workbookPart.Workbook = new Workbook();

            // Add Sheets collection
            Sheets sheets = workbookPart.Workbook.AppendChild(new Sheets());

            uint sheetId = 1;
            foreach (string sheetName in sheetNames)
            {
                // Create a new WorksheetPart for each sheet
                WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                worksheetPart.Worksheet = new Worksheet(new SheetData());

                // Create and append the sheet
                Sheet sheet = new()
                {
                    Id = workbookPart.GetIdOfPart(worksheetPart),
                    SheetId = sheetId++,
                    Name = sheetName
                };
                sheets.Append(sheet);
            }

            workbookPart.Workbook.Save();
            return spreadsheetDocument;
        }
    }
}

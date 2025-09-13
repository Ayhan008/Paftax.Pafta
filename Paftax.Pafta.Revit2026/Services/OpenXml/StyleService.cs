using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace Paftax.Pafta.Revit2026.Services.OpenXml
{
    internal class StyleService
    {
        /// <summary>
        /// Adds a Styles part to the given SpreadsheetDocument.
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        public static WorkbookStylesPart AddStylesPart(SpreadsheetDocument document, Stylesheet stylesheet)
        {
            WorkbookPart workbookPart = document.WorkbookPart ?? document.AddWorkbookPart();
            WorkbookStylesPart stylesPart = workbookPart.AddNewPart<WorkbookStylesPart>();
            stylesPart.Stylesheet = stylesheet;
            stylesPart.Stylesheet.Save();
            return stylesPart;
        }
    }
}

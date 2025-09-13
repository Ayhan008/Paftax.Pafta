using Autodesk.Revit.DB;
using Paftax.Pafta.Revit2026.Models;

namespace Paftax.Pafta.Revit2026.Services.Revit
{
    internal class ScheduleTableDataService
    {
        /// <summary>
        /// Sets up a ScheduleTableDataModel from a given ViewSchedule.
        /// </summary>
        /// <param name="viewSchedule"></param>
        /// <returns></returns>
        public static ScheduleTableDataModel CreateScheduleTableData(ViewSchedule viewSchedule)
        {
            ScheduleTableDataModel scheduleTableDataModel = new()
            {
                Schedule = viewSchedule,
                Name = viewSchedule.Name,
                MergedCells = GetMergedCellData(viewSchedule),
                BodyPart = GetBodySectionData(viewSchedule),
                HeaderPart = GetHeaderSectionData(viewSchedule),
                TitlePart = GetTitleSectionData(viewSchedule),
                TableData = GetTableData(GetTitleSectionData(viewSchedule), GetHeaderSectionData(viewSchedule))
            };
            return scheduleTableDataModel;
        }

        /// <summary>
        /// Sets up a list of ScheduleTableDataModel from a collection of ViewSchedules.
        /// </summary>
        /// <param name="viewSchedules"></param>
        /// <returns></returns>
        public static List<ScheduleTableDataModel> CreateScheduleTableData(List<ViewSchedule> viewSchedules)
        {
            return [.. viewSchedules.Select(CreateScheduleTableData)];
        }

        /// <summary>
        /// Get Table Data.
        /// </summary>
        /// <param name="bodyPart"></param>
        /// <param name="headerPart"></param>
        /// <returns></returns>
        public static List<List<string>> GetTableData(List<List<string>> bodyPart, List<List<string>> headerPart)
        {
            List<List<string>> tableData = [.. bodyPart, .. headerPart];
            return tableData;
        }
        /// <summary>
        ///  Gets the Excel-style column letter for a given column number (0-indexed).
        /// </summary>
        /// <param name="columnNumber"></param>
        /// <returns></returns>
        private static string GetColumnLetter(int columnNumber)
        {
            string columnLetter = string.Empty;
            while (columnNumber >= 0)
            {
                columnLetter = (char)('A' + (columnNumber % 26)) + columnLetter;
                columnNumber = (columnNumber / 26) - 1;
            }
            return columnLetter;
        }

        /// <summary>
        /// Gets the title section data of a schedule as a list of lists of strings.
        /// </summary>
        /// <param name="viewSchedule"></param>
        /// <returns></returns>
        public static List<List<string>> GetTitleSectionData(ViewSchedule viewSchedule)
        {
            TableData tableData = viewSchedule.GetTableData();
            TableSectionData tableSectionData = tableData.GetSectionData(SectionType.Body);

            List<List<string>> titleSectionData = [];
            List<string> titleRow = [];

            for (int column = tableSectionData.FirstColumnNumber; column <= tableSectionData.LastColumnNumber; column++)
            {
                if (column == tableSectionData.FirstColumnNumber)
                    titleRow.Add(viewSchedule.Name);
                else
                    titleRow.Add(string.Empty);
            }

            titleSectionData.Add(titleRow);
            return titleSectionData;
        }

        /// <summary>
        /// Get the header section data of a schedule as a list of lists of strings.
        /// </summary>
        /// <param name="viewSchedule"></param>
        /// <returns></returns>
        public static List<List<string>> GetHeaderSectionData(ViewSchedule viewSchedule)
        {
            TableData tableData = viewSchedule.GetTableData();
            TableSectionData tableSectionData = tableData.GetSectionData(SectionType.Body);

            List<List<string>> headerSectionData = [];

            for (int row = 0; row < tableSectionData.NumberOfRows; row++)
            {
                List<string> rowData = [];
                bool isFullyUnmerged = true;

                for (int col = 0; col <= tableSectionData.LastColumnNumber; col++)
                {
                    TableMergedCell tableMergedCell = tableSectionData.GetMergedCell(row, col);
                    string cellValue = viewSchedule.GetCellText(SectionType.Body, row, col);

                    if (tableMergedCell != null)
                    {
                        if (tableMergedCell.Top != row || tableMergedCell.Left != col)
                        {
                            isFullyUnmerged = false;
                        }
                    }

                    rowData.Add(cellValue);
                }

                headerSectionData.Add(rowData);

                if (isFullyUnmerged)
                {
                    break;
                }
            }

            return headerSectionData;
        }

        /// <summary>
        /// Get the body section data of a schedule as a list of lists of strings.
        /// </summary>
        /// <param name="viewSchedule"></param>
        /// <returns></returns>
        public static List<List<string>> GetBodySectionData(ViewSchedule viewSchedule)
        {
            TableData tableData = viewSchedule.GetTableData();
            TableSectionData tableSectionData = tableData.GetSectionData(SectionType.Body);

            List<List<string>> headerSectionData = GetHeaderSectionData(viewSchedule);
            List<List<string>> bodySectionData = [];

            int headerRowCount = headerSectionData.Count;

            for (int row = headerRowCount; row < tableSectionData.NumberOfRows; row++)
            {
                List<string> rowData = [];

                for (int col = 0; col <= tableSectionData.LastColumnNumber; col++)
                {
                    string cellValue = viewSchedule.GetCellText(SectionType.Body, row, col);
                    rowData.Add(cellValue);
                }

                bodySectionData.Add(rowData);
            }

            return bodySectionData;
        }

        /// <summary>
        /// Gets the merged cell references in a schedule as a set of strings.
        /// </summary>
        /// <param name="viewSchedule"></param>
        /// <returns></returns>
        public static HashSet<string> GetMergedCellData(ViewSchedule viewSchedule)
        {
            TableData tableData = viewSchedule.GetTableData();
            TableSectionData tableSectionData = tableData.GetSectionData(SectionType.Body);

            HashSet<string> mergedCellData = [];

            for (int row = 0; row < tableSectionData.NumberOfRows; row++)
            {
                for (int col = 0; col < tableSectionData.NumberOfColumns; col++)
                {
                    TableMergedCell tableMergedCell = tableSectionData.GetMergedCell(row, col);

                    if (tableMergedCell != null)
                    {
                        string topLeft = GetColumnLetter(tableMergedCell.Left) + (tableMergedCell.Top + 2);
                        string bottomRight = GetColumnLetter(tableMergedCell.Right) + (tableMergedCell.Bottom + 2);

                        if (tableMergedCell.Top == row && tableMergedCell.Left == col)
                        {
                            string mergedCellReference = $"{topLeft}:{bottomRight}";
                            mergedCellData.Add(mergedCellReference);
                        }
                    }
                }
            }

            // Add title merged cell reference
            string firstColumnNumber = GetColumnLetter(tableSectionData.FirstColumnNumber);
            string lastColumnNumber = GetColumnLetter(tableSectionData.LastColumnNumber);
            string titleMergedCellReference = $"{firstColumnNumber}1:{lastColumnNumber}1";
            mergedCellData.Add(titleMergedCellReference);

            return mergedCellData;
        }
    }
}

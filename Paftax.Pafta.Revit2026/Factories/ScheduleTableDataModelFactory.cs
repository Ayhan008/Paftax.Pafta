using System.Diagnostics;
using Autodesk.Revit.DB;
using Paftax.Pafta.Shared.Models;

namespace Paftax.Pafta.Revit2026.Factories
{
    internal class ScheduleTableDataModelFactory(ViewSchedule viewSchedule)
    {
        private const int _rowOffset = 1;
        private const int _columnOffset = 1;
        private const int _titleOffset = 1;

        private int _headerRowCount;

        private int _firstRowIndex;
        private int _firstColumnIndex;
        private int _lastRowIndex;
        private int _lastColumnIndex;

        public WorksheetTableData CreateFromSchedule()
        {
            TableSectionData tableSectionData =
                viewSchedule.GetTableData().GetSectionData(SectionType.Body);

            _firstColumnIndex = tableSectionData.FirstColumnNumber;
            _firstRowIndex = tableSectionData.FirstRowNumber;
            _lastColumnIndex = tableSectionData.LastColumnNumber;
            _lastRowIndex = tableSectionData.LastRowNumber;

            var headerPart = GetHeaderSectionData(tableSectionData);
            var bodyPart = GetBodySectionData();

            WorksheetTableData scheduleModel = new()
            {
                Title = viewSchedule.Title,
                TitlePart = GetTitleSectionData(),
                HeaderPart = headerPart,
                BodyPart = bodyPart,
                TitleRowCount = 1,
                HeaderRowCount = headerPart.Count,
                BodyRowCount = bodyPart.Count,
                MergedCellReferences = GetMergedCellData(tableSectionData)
            };
            return scheduleModel;
        }

        public static List<WorksheetTableData> CreateFromSchedules(List<ViewSchedule> viewSchedules)
        {
            List<WorksheetTableData> scheduleModels = [];

            foreach (var viewSchedule in viewSchedules)
            {
                scheduleModels.Add(new ScheduleTableDataModelFactory(viewSchedule).CreateFromSchedule());
            }

            return scheduleModels;
        }

        private static string GetColumnLetter(int columnNumber)
        {
            string letter = string.Empty;

            while (columnNumber >= 0)
            {
                letter = (char)('A' + (columnNumber % 26)) + letter;
                columnNumber = (columnNumber / 26) - 1;
            }
            return letter;
        }

        private List<List<CellData>> GetTitleSectionData()
        {
            List<List<CellData>> section = [];
            List<CellData> row = [];

            int excelRow = _firstRowIndex + _rowOffset;

            for (int col = _firstColumnIndex; col <= _lastColumnIndex; col++)
            {
                string colLetter = GetColumnLetter(col + _columnOffset);
                string reference = $"{colLetter}{excelRow}";

                row.Add(new CellData
                {
                    ColumnIndex = (uint)(col + _columnOffset),
                    RowIndex = (uint)excelRow,
                    Value = viewSchedule.Title,
                    Reference = reference
                });
            }

            section.Add(row);
            return section;
        }

        private List<List<CellData>> GetHeaderSectionData(TableSectionData tableSectionData)
        {
            List<List<CellData>> header = [];

            for (int row = _firstRowIndex; row <= _lastRowIndex; row++)
            {
                List<CellData> rowCells = [];

                bool isFullyUnmerged = true;

                for (int col = _firstColumnIndex; col <= _lastColumnIndex; col++)
                {
                    TableMergedCell merge = tableSectionData.GetMergedCell(row, col);
                    string value = viewSchedule.GetCellText(SectionType.Body, row, col);

                    if (merge != null && (merge.Top != row || merge.Left != col))
                        isFullyUnmerged = false;

                    int excelRow = row + _rowOffset + _titleOffset;
                    string colLetter = GetColumnLetter(col + _columnOffset);
                    string reference = $"{colLetter}{excelRow}";

                    rowCells.Add(new CellData
                    {
                        ColumnIndex = (uint)(col + _columnOffset),
                        RowIndex = (uint)excelRow,
                        Value = value,
                        Reference = reference
                    });
                }

                if (isFullyUnmerged)
                {
                    if (row == 0)
                    {
                        header.Add(rowCells);
                        _headerRowCount = header.Count;
                    }
                    else
                    {
                        _headerRowCount = header.Count;
                    }
                    break;
                }
                header.Add(rowCells);
            }

            Debug.WriteLine(_headerRowCount);
            return header;
        }

        private List<List<CellData>> GetBodySectionData()
        {
            List<List<CellData>> body = [];
            for (int row = _firstRowIndex + _headerRowCount; row <= _lastRowIndex; row++)
            {
                List<CellData> rowCells = [];

                int excelRow = row + _rowOffset + _titleOffset - 1;

                for (int col = _firstColumnIndex; col <= _lastColumnIndex; col++)
                {
                    string value = viewSchedule.GetCellText(SectionType.Body, row, col);
                    string colLetter = GetColumnLetter(col + _columnOffset);
                    string reference = $"{colLetter}{excelRow}";

                    rowCells.Add(new CellData
                    {
                        ColumnIndex = (uint)(col + _columnOffset),
                        RowIndex = (uint)excelRow,
                        Value = value,
                        Reference = reference
                    });
                }

                body.Add(rowCells);
            }

            return body;
        }

        private HashSet<string> GetMergedCellData(TableSectionData tableSectionData)
        {

            HashSet<string> merges = [];

            for (int row = _firstRowIndex; row <= _lastRowIndex; row++)
            {
                for (int col = _firstColumnIndex; col <= _lastColumnIndex; col++)
                {
                    TableMergedCell m = tableSectionData.GetMergedCell(row, col);
                    if (m == null)
                        continue;

                    if (m.Top != row || m.Left != col)
                        continue;

                    if (m.Top == m.Bottom && m.Left == m.Right)
                        continue;

                    string leftCol = GetColumnLetter(m.Left);
                    string rightCol = GetColumnLetter(m.Right);

                    int topRow = m.Top + _rowOffset + _titleOffset;
                    int bottomRow = m.Bottom + _rowOffset + _titleOffset;

                    string mergeRef = $"{leftCol}{topRow}:{rightCol}{bottomRow}";
                    merges.Add(mergeRef);
                }
            }

            string tFirst = GetColumnLetter(_firstColumnIndex);
            string tLast = GetColumnLetter(_lastColumnIndex);
            int tRow = _firstRowIndex + _rowOffset;

            string titleMerge = $"{tFirst}{tRow}:{tLast}{tRow}";
            merges.Add(titleMerge);

            return merges;
        }
    }
}

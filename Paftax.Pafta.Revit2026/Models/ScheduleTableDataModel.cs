using Autodesk.Revit.DB;

namespace Paftax.Pafta.Revit2026.Models
{
    internal class ScheduleTableDataModel
    {
        public required ViewSchedule Schedule { get; set; }
        public string Name { get; set; } = string.Empty;
        public HashSet<string> MergedCells { get; set; } = [];
        public List<List<string>> BodyPart { get; set; } = [];
        public List<List<string>> HeaderPart { get; set; } = [];
        public List<List<string>> TitlePart { get; set; } = [];
        public List<List<string>> TableData { get; set; } = [];
    }
}

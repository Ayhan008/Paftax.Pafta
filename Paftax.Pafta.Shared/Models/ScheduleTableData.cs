using Paftax.Pafta.Shared.Enums;
using Paftax.Pafta.Shared.Interfaces;

namespace Paftax.Pafta.Shared.Models
{
    public class ScheduleTableData
    {
        public required long Id { get; set; }
        public required string Name { get; set; }
        public HashSet<string> MergedCells { get; set; } = [];

        public Dictionary<SchedulePart, List<List<string>>> TableParts { get; set; } = new()
        {
            { SchedulePart.Header, new List<List<string>>() },
            { SchedulePart.Body, new List<List<string>>() },
            { SchedulePart.Title, new List<List<string>>() }
        };

        public List<List<string>> HeaderPart => TableParts[SchedulePart.Header];
        public List<List<string>> BodyPart => TableParts[SchedulePart.Body];
        public List<List<string>> TitlePart => TableParts[SchedulePart.Title];

        public List<List<string>> TableData { get; set; } = [];

        public uint HeaderRowCount => (uint)HeaderPart.Count;
        public uint BodyRowCount => (uint)BodyPart.Count;
        public uint TitleRowCount => (uint)TitlePart.Count;
        public uint TotalRowCount => HeaderRowCount + BodyRowCount + TitleRowCount;
    }
}

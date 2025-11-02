using Paftax.Pafta.Shared.Enums;
using Paftax.Pafta.Shared.Interfaces;

namespace Paftax.Pafta.Shared.Models
{
    public class ScheduleTableData
    {
        public required long Id { get; set; }
        public required string Name { get; set; }
        public HashSet<string> MergedCells { get; set; } = [];

        public Dictionary<RevitSchedulePart, List<List<string>>> TableParts { get; set; } = new()
        {
            { RevitSchedulePart.Header, new List<List<string>>() },
            { RevitSchedulePart.Body, new List<List<string>>() },
            { RevitSchedulePart.Title, new List<List<string>>() }
        };

        public List<List<string>> HeaderPart => TableParts[RevitSchedulePart.Header];
        public List<List<string>> BodyPart => TableParts[RevitSchedulePart.Body];
        public List<List<string>> TitlePart => TableParts[RevitSchedulePart.Title];

        public List<List<string>> TableData { get; set; } = [];

        public uint HeaderRowCount => (uint)HeaderPart.Count;
        public uint BodyRowCount => (uint)BodyPart.Count;
        public uint TitleRowCount => (uint)TitlePart.Count;
        public uint TotalRowCount => HeaderRowCount + BodyRowCount + TitleRowCount;
    }
}

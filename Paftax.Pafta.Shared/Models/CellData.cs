namespace Paftax.Pafta.Shared.Models
{
    public class CellData
    {
        public required uint ColumnIndex { get; set; }
        public required uint RowIndex { get; set; }
        public required string Value { get; set; }
        public uint? StyleIndex { get; set; }
        public string? Reference { get; set; }
    }
}

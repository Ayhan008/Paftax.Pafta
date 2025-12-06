namespace Paftax.Pafta.Shared.Models
{
    public class WorksheetTableData 
    {
        public required string Title { get; set; }
        public List<List<CellData>> TitlePart { get; set; } = [];
        public List<List<CellData>> HeaderPart { get; set; } = [];
        public List<List<CellData>> BodyPart { get; set; } = [];    
        public int HeaderRowCount { get; set; }
        public int TitleRowCount { get; set; }
        public int BodyRowCount { get; set; }
        public HashSet<string> MergedCellReferences { get; set; } = [];
    }
}

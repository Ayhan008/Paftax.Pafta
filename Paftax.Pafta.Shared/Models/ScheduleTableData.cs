using Paftax.Pafta.Shared.Interfaces;

namespace Paftax.Pafta.Shared.Models
{
    public class ScheduleTableData: IElement
    {     
        public required long Id { get; set; }
        public required string Name { get; set; }
        public HashSet<string> MergedCells { get; set; } = [];
        public List<List<string>> BodyPart { get; set; } = [];
        public List<List<string>> HeaderPart { get; set; } = [];
        public List<List<string>> TitlePart { get; set; } = [];
        public List<List<string>> TableData { get; set; } = [];
        public uint HeaderRowCount 
        { 
            get 
            { 
                return (uint)HeaderPart.Count; 
            }
        }
    }
}

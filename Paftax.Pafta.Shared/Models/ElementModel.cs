using DocumentFormat.OpenXml.Drawing.Diagrams;

namespace Paftax.Pafta.Shared.Models
{
    public class ElementModel
    {
        public required long Id { get; set; }
        public required string Name { get; set; }
        public string TypeName { get; set; } = "Unknown";
        public string CategoryName { get; set; } = "Unknown";
        public long LevelId { get; set; }
        public long WorksetId { get; set; }
        
    }
}

namespace Paftax.Pafta.Shared.Models
{
    public class CreateViewModel
    {
        public string Name { get; set; } = string.Empty;
        public string ViewType { get; set; } = string.Empty;
        public int Scale { get; set; }
        public string TitleOnSheet { get; set; } = string.Empty;
        public string NumberOnSheet { get; set; } = string.Empty;
        public ViewTemplateModel? ViewTemplate { get; set; }
    }
}

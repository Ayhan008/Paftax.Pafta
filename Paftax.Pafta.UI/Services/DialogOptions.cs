namespace Paftax.Pafta.UI.Services
{
    public class DialogOptions
    {
        public int Width { get; set; } = 800;
        public int Height { get; set; } = 600;
        public string Title { get; set; } = "Dialog";
        public bool ShowCloseButton { get; set; } = true;
        public bool ShowMaximizeButton { get; set; } = false;
        public bool ShowMinimizeButton { get; set; } = false;
        public bool ShowHelpButton { get; set; } = false;
        public bool Async { get; set; } = false;
        public void Deconstruct(out int width, out int height, out string title)
        {
            width = Width;
            height = Height;
            title = Title;
        }
    }
}

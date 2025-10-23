using Paftax.Pafta.Shared.Enums;

namespace Paftax.Pafta.Shared.Interfaces
{
    public interface IInfoDialogViewModel
    {
        event Action? CloseAction;
        string Message { get; set; }
        string Glyph { get; set; }
        IconType IconType { get; set; }
    }
}

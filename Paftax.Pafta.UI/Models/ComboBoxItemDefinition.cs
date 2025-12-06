using Paftax.Pafta.Shared.Models.Element;
using Paftax.Pafta.UI.ViewModels;

namespace Paftax.Pafta.UI.Models;

public class ComboBoxItemDefinition<T> where T : ElementModel
{
    public string DisplayName { get; set; } = string.Empty;
    public Func<SelectableElementViewModel<T> ,bool>? FilterFunction { get; set; }

}

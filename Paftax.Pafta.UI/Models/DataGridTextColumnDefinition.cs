using Paftax.Pafta.Shared.Models.Element;
using Paftax.Pafta.UI.ViewModels;
using System.Windows.Controls;

namespace Paftax.Pafta.UI.Models
{
    public class DataGridTextColumnDefinition<T> where T : ElementModel
    {
        public string Header { get; set; } = string.Empty;
        public Func<SelectableElementViewModel<T>, string>? BindingPath { get; set; }
        public DataGridLength Width { get; set; }
        public bool CanResize { get; set; } = false;
    }
}

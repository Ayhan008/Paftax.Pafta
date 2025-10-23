using Paftax.Pafta.Shared.Models;
using System.Collections.ObjectModel;

namespace Paftax.Pafta.Shared.Interfaces
{
    public interface ICleanGarbageViewModel
    {
        ObservableCollection<FilterModel> Filters { get; }
        ObservableCollection<MaterialModel> Materials { get; }
        ObservableCollection<ViewModel> Views { get; }
        ObservableCollection<ViewTemplateModel> ViewTemplates { get; }
        ObservableCollection<LineModel> Lines { get; }

        Action? RequestLoadMaterials { get; set; }
        Action? RequestLoadFilters { get; set; }
        Action? RequestLoadLines { get; set; }

        event Action? CloseAction;
        event Action? CleanAction;
    }
}

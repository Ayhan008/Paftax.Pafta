using CommunityToolkit.Mvvm.ComponentModel;
using Paftax.Pafta.Shared.Interfaces;
using Paftax.Pafta.Shared.Models;
using System.Collections.ObjectModel;

namespace Paftax.Pafta.UI.ViewModels
{
    public partial class CleanGarbageViewModel : ObservableObject
    {
        public ObservableCollection<TagCategoryModel> TagCategories { get; } = [];
        public ObservableCollection<FilterModel> Filters { get; } = [];
        public ObservableCollection<ViewTemplateModel> ViewTemplates { get; } = [];

        public void LoadTagCategoryModels(IEnumerable<TagCategoryModel> models)
        {
            TagCategories.Clear();

            foreach (var model in models)
                TagCategories.Add(model);
        }
    }
}

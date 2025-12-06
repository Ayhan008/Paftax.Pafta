using CommunityToolkit.Mvvm.ComponentModel;
using Paftax.Pafta.Shared.Models.Element;
using Paftax.Pafta.UI.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;

namespace Paftax.Pafta.UI.ViewModels
{
    public partial class SearchableElementCollectionViewModel<T> : ObservableObject where T : ElementModel
    {
        public ObservableCollection<SelectableElementViewModel<T>> Elements { get; set; }
        public ObservableCollection<DataGridTextColumnDefinition<T>> DataGridTextColumns { get; }
        public string SearchBoxPlaceholderText { get; } = "Search...";

        private readonly ICollectionView _elementsView;

        [ObservableProperty] private string _searchBoxSearchText = string.Empty;
        [ObservableProperty] private bool _isAllChecked;

        partial void OnIsAllCheckedChanged(bool value)
        {
            foreach (SelectableElementViewModel<T> element in Elements)
            {
                element.IsChecked = value;
            }
        }

        public SearchableElementCollectionViewModel(
            IEnumerable<SelectableElementViewModel<T>> elements,
            IEnumerable<DataGridTextColumnDefinition<T>> columns)
        {
            Elements = new ObservableCollection<SelectableElementViewModel<T>>(elements);
            DataGridTextColumns = new ObservableCollection<DataGridTextColumnDefinition<T>>(columns);

            _elementsView = CollectionViewSource.GetDefaultView(Elements);
            _elementsView.Filter = FilterItem;
        }

        private bool FilterItem(object obj)
        {
            if (obj is not SelectableElementViewModel<T> selectableItem || selectableItem.Model is not T item)
                return false;

            if (!string.IsNullOrWhiteSpace(SearchBoxSearchText))
            {
                if (!item.Name.Contains(SearchBoxSearchText, StringComparison.OrdinalIgnoreCase))
                    return false;
            }
            return true;

        }

        partial void OnSearchBoxSearchTextChanged(string value)
        {
            _elementsView.Refresh();
        }
    }
}

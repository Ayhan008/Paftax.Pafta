using CommunityToolkit.Mvvm.ComponentModel;
using Paftax.Pafta.Shared.Models.Element;
using Paftax.Pafta.UI.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
namespace Paftax.Pafta.UI.ViewModels
{
    public partial class FilterableElementCollectionViewModel<T> : ObservableObject where T : ElementModel
    {
        public ObservableCollection<SelectableElementViewModel<T>> Elements { get; set; }
        public ObservableCollection<ComboBoxItemDefinition<T>> ComboBoxItems { get; }
        public ObservableCollection<DataGridTextColumnDefinition<T>> DataGridTextColumns { get; }

        public string SearchBoxPlaceholderText { get; } = "Search...";

        private readonly ICollectionView _elementsView;

        [ObservableProperty]
        private string _searchBoxSearchText = string.Empty;

        [ObservableProperty]
        private int _comboBoxSelectedIndex = 1;

        [ObservableProperty]
        private bool _isAllChecked;

        partial void OnIsAllCheckedChanged(bool value)
        {
            foreach (SelectableElementViewModel<T> element in Elements)
            {
                element.IsChecked = value;
            }
        }

        public FilterableElementCollectionViewModel(
            IEnumerable<SelectableElementViewModel<T>> elements,
            IEnumerable<ComboBoxItemDefinition<T>> comboBoxItems,
            IEnumerable<DataGridTextColumnDefinition<T>> columns)
        {
            Elements = new ObservableCollection<SelectableElementViewModel<T>>(elements);
            ComboBoxItems = new ObservableCollection<ComboBoxItemDefinition<T>>(comboBoxItems);
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

            if (ComboBoxSelectedIndex >= 0 &&
                ComboBoxSelectedIndex < ComboBoxItems.Count)
            {
                var filter = ComboBoxItems[ComboBoxSelectedIndex].FilterFunction;
                if (filter != null && !filter(selectableItem))
                    return false;
            }
            return true;

        }

        partial void OnSearchBoxSearchTextChanged(string value)
        {
            _elementsView.Refresh();
        }

        partial void OnComboBoxSelectedIndexChanged(int value)
        {
            _elementsView.Refresh();
        }
    }
}

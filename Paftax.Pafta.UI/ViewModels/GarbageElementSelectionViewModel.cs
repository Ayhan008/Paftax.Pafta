using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;

namespace Paftax.Pafta.UI.ViewModels
{
    public partial class GarbageElementSelectionViewModel : ObservableObject
    {
        public ObservableCollection<GarbageElementViewModel> GarbageElements { get; }
        public ICollectionView GarbageElementsView { get; }
        public ObservableCollection<string> ComboBoxCategories { get; }

        [ObservableProperty] private int selectedCategoryIndex = 1;
        [ObservableProperty] private bool isAllChecked = false;
        [ObservableProperty] private string searchText = string.Empty;

        public GarbageElementSelectionViewModel(ObservableCollection<GarbageElementViewModel> garbageElementViewModels, ObservableCollection<string> comboBoxCategories)
        {
            ComboBoxCategories = comboBoxCategories;
            GarbageElements = garbageElementViewModels;
            GarbageElementsView = CollectionViewSource.GetDefaultView(GarbageElements);
            GarbageElementsView.Filter = FilterGarbageElements;
        }
        partial void OnIsAllCheckedChanged(bool value)
        {
            foreach (GarbageElementViewModel element in GarbageElements)
            {
                element.IsChecked = value;
            }
        }
        partial void OnSearchTextChanged(string value)
        {
            GarbageElementsView.Refresh();
        }
        partial void OnSelectedCategoryIndexChanged(int value)
        {
            GarbageElementsView.Refresh();
        }
        private bool FilterGarbageElements(object obj)
        {
            if (obj is not GarbageElementViewModel element)
                return false;

            bool matchesSearch = string.IsNullOrWhiteSpace(SearchText) || element.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase);

            bool comboBoxFilter = SelectedCategoryIndex switch
            {
                1 => !element.IsUsed,
                _ => true
            };
            return matchesSearch && comboBoxFilter;
        }
    }
}

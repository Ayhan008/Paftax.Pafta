using CommunityToolkit.Mvvm.ComponentModel;
using Paftax.Pafta.Shared.Models;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Threading;

namespace Paftax.Pafta.UI.ViewModels
{
    public partial class CleanGarbageViewModel : ObservableObject
    {
        #region Collections
        private readonly ObservableCollection<TagCategoryModel> _allTagCategories = [];
        private readonly ObservableCollection<ViewTemplateModel> _allViewTemplates = [];
        private readonly ObservableCollection<FilterModel> _allFilters = [];
        private readonly ObservableCollection<MaterialModel> _allMaterials = [];
        private readonly ObservableCollection<LineModel> _allLines = [];

        public ObservableCollection<TagCategoryModel> TagCategories { get; } = [];
        public ObservableCollection<ViewTemplateModel> ViewTemplates { get; } = [];
        public ObservableCollection<FilterModel> Filters { get; } = [];
        public ObservableCollection<MaterialModel> Materials { get; } = [];
        public ObservableCollection<LineModel> Lines { get; } = [];
        #endregion

        public Action? RequestLoadMaterials { get; set; }
        public Action? RequestLoadFilters { get; set; }
        public Action? RequestLoadLines { get; set; }

        [ObservableProperty]
        private bool _isBusy = false;
        [ObservableProperty]
        private int _selectedTabIndex = 0;

        #region Search Texts
        [ObservableProperty]
        private string _tagSearchText = string.Empty;
        [ObservableProperty]
        private string _viewTemplateSearchText = string.Empty;
        [ObservableProperty]
        private string _materialSearchText = string.Empty;
        [ObservableProperty]
        private string _filterSearchText = string.Empty;
        [ObservableProperty]
        private string _lineSearchText = string.Empty;
        #endregion

        #region DataGrid SelectAll CheckBoxes
        [ObservableProperty]
        private bool _isAllTagsChecked = false;
        [ObservableProperty]
        private bool _isAllViewTemplatesChecked = false;
        [ObservableProperty]
        private bool _isAllFiltersChecked = false;
        [ObservableProperty]
        private bool _isAllMaterialsChecked = false;
        [ObservableProperty]
        private bool _isAllLinesChecked = false;
        #endregion

        private bool _linesLoaded = false;
        private bool _filtersLoaded = false;
        private bool _materialsLoaded = false;

        partial void OnSelectedTabIndexChanged(int value)
        {
            if (value == 4 && !_linesLoaded)
            {
                _linesLoaded = true;
                IsBusy = true;
                Application.Current.Dispatcher.BeginInvoke(
                    DispatcherPriority.ApplicationIdle,
                    new Action(() =>
                    {
                        RequestLoadLines?.Invoke();
                        IsBusy = false;
                    }));
            }
            else if (value == 2 && !_filtersLoaded)
            {
                _filtersLoaded = true;
                Application.Current.Dispatcher.BeginInvoke(
                    DispatcherPriority.ApplicationIdle,
                    new Action(() => RequestLoadFilters?.Invoke()));
            }
            else if (value == 3 && !_materialsLoaded)
            {
                _materialsLoaded = true;
                Application.Current.Dispatcher.BeginInvoke(
                    DispatcherPriority.ApplicationIdle,
                    new Action(() => RequestLoadMaterials?.Invoke()));
            }
        }

        partial void OnIsAllTagsCheckedChanged(bool value)
        {
            foreach (var item in TagCategories)
                if (item.IsChecked != value) item.IsChecked = value;
        }

        partial void OnIsAllViewTemplatesCheckedChanged(bool value)
        {
            foreach (var item in ViewTemplates)
                if (item.IsChecked != value) item.IsChecked = value;
        }

        partial void OnIsAllFiltersCheckedChanged(bool value)
        {
            foreach (var item in Filters)
                if (item.IsChecked != value) item.IsChecked = value;
        }

        partial void OnIsAllMaterialsCheckedChanged(bool value)
        {
            foreach (var item in Materials)
                if (item.IsChecked != value) item.IsChecked = value;
        }

        partial void OnIsAllLinesCheckedChanged(bool value)
        {
            foreach (var item in Lines)
                if (item.IsChecked != value) item.IsChecked = value;
        }

        partial void OnTagSearchTextChanged(string value) => FilterCollection(_allTagCategories, TagCategories, x => x.Category, value);
        partial void OnViewTemplateSearchTextChanged(string value) => FilterCollection(_allViewTemplates, ViewTemplates, x => x.Name, value);
        partial void OnFilterSearchTextChanged(string value) => FilterCollection(_allFilters, Filters, x => x.Name, value);
        partial void OnMaterialSearchTextChanged(string value) => FilterCollection(_allMaterials, Materials, x => x.Name, value);
        partial void OnLineSearchTextChanged(string value) => FilterCollection(_allLines, Lines, x => x.Name, value);

        private static void FilterCollection<T>(ObservableCollection<T> master, ObservableCollection<T> view, Func<T, string> selector, string filter)
        {
            view.Clear();
            foreach (var item in master.Where(x => selector(x).Contains(filter, System.StringComparison.OrdinalIgnoreCase)))
                view.Add(item);
        }

        public void LoadTagCategoryModels(IEnumerable<TagCategoryModel> models)
        {
            _allTagCategories.Clear();
            foreach (var model in models)
                _allTagCategories.Add(model);

            FilterCollection(_allTagCategories, TagCategories, x => x.Category, TagSearchText);
        }

        public void LoadViewTemplateModels(IEnumerable<ViewTemplateModel> models)
        {
            _allViewTemplates.Clear();
            foreach (var model in models)
                _allViewTemplates.Add(model);

            FilterCollection(_allViewTemplates, ViewTemplates, x => x.Name, ViewTemplateSearchText);
        }

        public void LoadFilterModels(IEnumerable<FilterModel> models)
        {
            _allFilters.Clear();
            foreach (var model in models)
                _allFilters.Add(model);

            FilterCollection(_allFilters, Filters, x => x.Name, FilterSearchText);
        }

        public void LoadLineModels(IEnumerable<LineModel> models)
        {
            _allLines.Clear();
            foreach (var model in models)
                _allLines.Add(model);

            FilterCollection(_allLines, Lines, x => x.Name, LineSearchText);          
        }

        public void LoadMaterialModels(IEnumerable<MaterialModel> models)
        {
            _allMaterials.Clear();
            foreach (var model in models)
                _allMaterials.Add(model);

            FilterCollection(_allMaterials, Materials, x => x.Name, MaterialSearchText);
        }
    }
}

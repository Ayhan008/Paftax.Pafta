using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Paftax.Pafta.Shared.Interfaces;
using Paftax.Pafta.Shared.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Threading;

namespace Paftax.Pafta.UI.ViewModels
{
    public partial class CleanGarbageViewModel : ObservableObject, ICleanGarbageViewModel
    {
        #region Collections
        public ObservableCollection<ViewModel> Views { get; } = [];
        public ObservableCollection<ViewTemplateModel> ViewTemplates { get; } = [];
        public ObservableCollection<FilterModel> Filters { get; } = [];
        public ObservableCollection<MaterialModel> Materials { get; } = [];
        public ObservableCollection<LineModel> Lines { get; } = [];

        public ICollectionView ViewsView { get; private set; }
        public ICollectionView ViewTemplatesView { get; private set; }
        public ICollectionView FiltersView { get; private set; }
        public ICollectionView MaterialsView { get; private set; }
        public ICollectionView LinesView { get; private set; }
        #endregion

        public Action? RequestLoadMaterials { get; set; }
        public Action? RequestLoadFilters { get; set; }
        public Action? RequestLoadLines { get; set; }

        public event Action? CloseAction;
        public event Action? CleanAction;
        public event Action? CancelAction;

        [ObservableProperty]
        private int _selectedTabIndex = 0;

        #region Search Texts
        [ObservableProperty]
        private string _viewSearchText = string.Empty;
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
        private bool _isAllViewsChecked = false;
        [ObservableProperty]
        private bool _isAllViewTemplatesChecked = false;
        [ObservableProperty]
        private bool _isAllFiltersChecked = false;
        [ObservableProperty]
        private bool _isAllMaterialsChecked = false;
        [ObservableProperty]
        private bool _isAllLinesChecked = false;
        #endregion

        [ObservableProperty]
        private Visibility _materialsProgressRing = Visibility.Collapsed;
        [ObservableProperty]
        private Visibility _linesProgressRing = Visibility.Collapsed;
        [ObservableProperty]
        private Visibility _filtersProgressRing = Visibility.Collapsed;

        [RelayCommand]
        private void Cancel()
        {
            CancelAction?.Invoke();
            CloseAction?.Invoke();
        }

        [RelayCommand]
        private void Clean()
        {
            CleanAction?.Invoke();
            CloseAction?.Invoke();
        }

        private bool _filtersLoaded = false;
        private bool _materialsLoaded = false;
        private bool _linesLoaded = false;

        public CleanGarbageViewModel()
        {
            ViewsView = CollectionViewSource.GetDefaultView(Views);
            ViewTemplatesView = CollectionViewSource.GetDefaultView(ViewTemplates);
            FiltersView = CollectionViewSource.GetDefaultView(Filters);
            MaterialsView = CollectionViewSource.GetDefaultView(Materials);
            LinesView = CollectionViewSource.GetDefaultView(Lines);
        }

        partial void OnSelectedTabIndexChanged(int value)
        {
            if (value == 4 && !_linesLoaded)
            {
                _linesLoaded = true;
                Application.Current.Dispatcher.BeginInvoke(
                    DispatcherPriority.ApplicationIdle,
                    new Action(() =>
                    {
                        LinesProgressRing = Visibility.Visible;
                        RequestLoadLines?.Invoke();
                    }));
            }
            else if (value == 2 && !_filtersLoaded)
            {
                _filtersLoaded = true;
                Application.Current.Dispatcher.BeginInvoke(
                    DispatcherPriority.ApplicationIdle,
                    new Action(() =>
                    {
                        FiltersProgressRing = Visibility.Visible;
                        RequestLoadFilters?.Invoke();
                    }));
            }
            else if (value == 3 && !_materialsLoaded)
            {
                _materialsLoaded = true;
                Application.Current.Dispatcher.BeginInvoke(
                    DispatcherPriority.ApplicationIdle,
                    new Action(() =>
                    {
                        MaterialsProgressRing = Visibility.Visible;
                        RequestLoadMaterials?.Invoke();
                    }));
            }
        }

        partial void OnIsAllViewsCheckedChanged(bool value)
        {
            foreach (var item in Views)
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

        partial void OnViewSearchTextChanged(string value)
        {
            ViewsView.Filter = item =>
                string.IsNullOrEmpty(value) || ((TagCategoryModel)item).Category.Contains(value, StringComparison.OrdinalIgnoreCase);
            ViewsView.Refresh();
        }

        partial void OnViewTemplateSearchTextChanged(string value)
        {
            ViewTemplatesView.Filter = item =>
                string.IsNullOrEmpty(value) || ((ViewTemplateModel)item).Name.Contains(value, StringComparison.OrdinalIgnoreCase);
            ViewTemplatesView.Refresh();
        }

        partial void OnFilterSearchTextChanged(string value)
        {
            FiltersView.Filter = item =>
                string.IsNullOrEmpty(value) || ((FilterModel)item).Name.Contains(value, StringComparison.OrdinalIgnoreCase);
            FiltersView.Refresh();
        }

        partial void OnMaterialSearchTextChanged(string value)
        {
            MaterialsView.Filter = item =>
                string.IsNullOrEmpty(value) || ((MaterialModel)item).Name.Contains(value, StringComparison.OrdinalIgnoreCase);
            MaterialsView.Refresh();
        }

        partial void OnLineSearchTextChanged(string value)
        {
            LinesView.Filter = item =>
                string.IsNullOrEmpty(value) || ((LineModel)item).Name.Contains(value, StringComparison.OrdinalIgnoreCase);
            LinesView.Refresh();
        }

        public void LoadViewModels(IEnumerable<ViewModel> models)
        {
            Views.Clear();
            foreach (var model in models)
                Views.Add(model);
        }

        public void LoadViewTemplateModels(IEnumerable<ViewTemplateModel> models)
        {
            ViewTemplates.Clear();
            foreach (var model in models)
                ViewTemplates.Add(model);
        }

        public void LoadFilterModels(IEnumerable<FilterModel> models)
        {
            Filters.Clear();
            foreach (var model in models)
                Filters.Add(model);

            FiltersProgressRing = Visibility.Collapsed;
        }

        public void LoadLineModels(IEnumerable<LineModel> models)
        {
            Lines.Clear();
            foreach (var model in models)
                Lines.Add(model);

            LinesProgressRing = Visibility.Collapsed;
        }

        public void LoadMaterialModels(IEnumerable<MaterialModel> models)
        {
            Materials.Clear();
            foreach (var model in models)
                Materials.Add(model);

            MaterialsProgressRing = Visibility.Collapsed;
        }
    }
}

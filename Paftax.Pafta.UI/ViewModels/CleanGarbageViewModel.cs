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
    public partial class CleanGarbageViewModel : ObservableObject
    {
        #region Collections
        public ObservableCollection<GarbageElementViewModel> Views { get; } = [];
        public ObservableCollection<GarbageElementViewModel> ViewTemplates { get; } = [];
        public ObservableCollection<GarbageElementViewModel> Filters { get; } = [];
        public ObservableCollection<GarbageElementViewModel> Materials { get; } = [];
        public ObservableCollection<GarbageElementViewModel> Lines { get; } = [];

        public ICollectionView ViewsView { get; private set; }
        public ICollectionView ViewTemplatesView { get; private set; }
        public ICollectionView FiltersView { get; private set; }
        public ICollectionView MaterialsView { get; private set; }
        public ICollectionView LinesView { get; private set; }
        #endregion

        #region ComboBox Fiters and Selected Indices
        public List<string> ViewFilters { get; } = ["All", "Unplaced"];
        public List<string> ViewTemplateFilters { get; } = ["All", "Unused"];
        public List<string> LineFilters { get; } = ["All", "Unused"];
        public List<string> MaterialFilters { get; } = ["All", "Unused"];
        public List<string> FilterFilters { get; } = ["All", "Unplaced"];

        [ObservableProperty] private int _selectedTabIndex = 0;
        [ObservableProperty] private int _selectedViewFilterIndex = 1;
        [ObservableProperty] private int _selectedViewTemplateFilterIndex = 1;
        [ObservableProperty] private int _selectedMaterialFilterIndex = 1;
        [ObservableProperty] private int _selectedFilterFilterIndex = 1;
        [ObservableProperty] private int _selectedLineFilterIndex = 1;
        #endregion

        #region Request Load Actions
        public Action? RequestLoadMaterials { get; set; }
        public Action? RequestLoadFilters { get; set; }
        public Action? RequestLoadLines { get; set; }
        #endregion

        #region Search Texts
        [ObservableProperty] private string _viewSearchText = string.Empty;
        [ObservableProperty] private string _viewTemplateSearchText = string.Empty;
        [ObservableProperty] private string _materialSearchText = string.Empty;
        [ObservableProperty] private string _filterSearchText = string.Empty;
        [ObservableProperty] private string _lineSearchText = string.Empty;

        #endregion

        #region DataGrid SelectAll CheckBoxes
        [ObservableProperty] private bool _isAllViewsChecked = false;
        [ObservableProperty] private bool _isAllViewTemplatesChecked = false;
        [ObservableProperty] private bool _isAllFiltersChecked = false;
        [ObservableProperty] private bool _isAllMaterialsChecked = false;
        [ObservableProperty] private bool _isAllLinesChecked = false;
        #endregion

        #region Button Actions   
        public event Action? CleanAction;
        public event Action? CancelAction;

        [RelayCommand]
        private void Cancel()
        {
            CancelAction?.Invoke();
        }

        [RelayCommand]
        private void Clean()
        {
            CleanAction?.Invoke();
        }
        #endregion

        [ObservableProperty] private bool _filtersLoaded = false;
        [ObservableProperty] private bool _materialsLoaded = false;
        [ObservableProperty] private bool _linesLoaded = false;
        [ObservableProperty] private bool _viewsLoaded = true;
        [ObservableProperty] private bool _viewTemplatesLoaded = true;

        [ObservableProperty] private string _linesLoadingStatusText = string.Empty;
        [ObservableProperty] private string _materialsLoadingStatusText = string.Empty;
        [ObservableProperty] private string _filtersLoadingStatusText = string.Empty;

        public CleanGarbageViewModel()
        {
            ViewsView = CollectionViewSource.GetDefaultView(Views);
            ViewsView.Filter = ViewsFilter;

            ViewTemplatesView = CollectionViewSource.GetDefaultView(ViewTemplates);
            ViewTemplatesView.Filter = ViewTemplateFilter;

            FiltersView = CollectionViewSource.GetDefaultView(Filters);
            FiltersView.Filter = FiltersFilter;

            MaterialsView = CollectionViewSource.GetDefaultView(Materials);
            MaterialsView.Filter = MaterialsFilter;

            LinesView = CollectionViewSource.GetDefaultView(Lines);
            LinesView.Filter = LinesFilter;
        }

        #region Tab Selection Changed Handler
        partial void OnSelectedTabIndexChanged(int value)
        {
            switch (value)
            {
                case 4:
                    if (!LinesLoaded)
                    {
                        StartLoadingTimer(ref _linesLoadingTimer, ref _linesLoadingElapsed, _linesLoadingMessages,
                            msg => LinesLoadingStatusText = msg, () => LinesLoaded);

                        Application.Current.Dispatcher.BeginInvoke(
                            DispatcherPriority.ApplicationIdle,
                            new Action(() => RequestLoadLines?.Invoke()));
                    }
                    break;

                case 2:
                    if (!FiltersLoaded)
                    {
                        StartLoadingTimer(ref _filtersLoadingTimer, ref _filtersLoadingElapsed, _filtersLoadingMessages,
                            msg => FiltersLoadingStatusText = msg, () => FiltersLoaded);

                        Application.Current.Dispatcher.BeginInvoke(
                            DispatcherPriority.ApplicationIdle,
                            new Action(() => RequestLoadFilters?.Invoke()));
                    }
                    break;

                case 3:
                    if (!MaterialsLoaded)
                    {
                        StartLoadingTimer(ref _materialsLoadingTimer, ref _materialsLoadingElapsed, _materialsLoadingMessages,
                            msg => MaterialsLoadingStatusText = msg, () => MaterialsLoaded);


                        Application.Current.Dispatcher.BeginInvoke(
                            DispatcherPriority.ApplicationIdle,
                            new Action(() => RequestLoadMaterials?.Invoke()));
                    }
                    break;
            }
        }
        #endregion

        #region Status Text and Timer
        private readonly List<(TimeSpan Threshold, string Message)> _linesLoadingMessages =
        [
            (TimeSpan.FromSeconds(0), "Loading lines..."),
            (TimeSpan.FromSeconds(5), "This may take a moment...\nDo not close the window."),
        ];

        private readonly List<(TimeSpan Threshold, string Message)> _materialsLoadingMessages =
        [
            (TimeSpan.FromSeconds(0), "Loading materials..."),
            (TimeSpan.FromSeconds(5), "This may take a moment...\nDo not close the window."),
        ];

        private readonly List<(TimeSpan Threshold, string Message)> _filtersLoadingMessages =
        [
            (TimeSpan.FromSeconds(0), "Loading filters..."),
            (TimeSpan.FromSeconds(5), "This may take a moment...\nDo not close the window."),
        ];

        private DispatcherTimer? _linesLoadingTimer;
        private DispatcherTimer? _materialsLoadingTimer;
        private DispatcherTimer? _filtersLoadingTimer;

        private TimeSpan _linesLoadingElapsed = TimeSpan.Zero;
        private TimeSpan _materialsLoadingElapsed = TimeSpan.Zero;
        private TimeSpan _filtersLoadingElapsed = TimeSpan.Zero;
        private static void StartLoadingTimer(
            ref DispatcherTimer? timer,
            ref TimeSpan elapsed,
            List<(TimeSpan Threshold, string Message)> messages,
            Action<string> setMessage,
            Func<bool> isLoaded)
        {
            if (timer != null && timer.IsEnabled)
                return;

            elapsed = TimeSpan.Zero;
            setMessage(messages[0].Message);

            TimeSpan localElapsed = TimeSpan.Zero;

            var localTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            timer = localTimer;
            localTimer.Tick += (s, e) =>
            {
                localElapsed += TimeSpan.FromSeconds(1);

                foreach (var (threshold, message) in messages.OrderBy(m => m.Threshold))
                {
                    if (localElapsed >= threshold)
                        setMessage(message);
                }

                if (isLoaded())
                {
                    localTimer.Stop();
                }
            };
            localTimer.Start();
        }

        #endregion

        #region Filtering Handlers
        partial void OnSelectedViewFilterIndexChanged(int value) => ViewsView.Refresh();
        partial void OnViewSearchTextChanged(string value) => ViewsView.Refresh();

        private bool ViewsFilter(object item)
        {
            if (item is not GarbageElementViewModel vm) return false;

            bool search = string.IsNullOrWhiteSpace(ViewSearchText)
                          || vm.Name.Contains(ViewSearchText, StringComparison.OrdinalIgnoreCase);

            bool filter = SelectedViewFilterIndex switch
            {
                1 => !vm.IsUsed,
                _ => true
            };

            return search && filter;
        }

        partial void OnSelectedViewTemplateFilterIndexChanged(int value) => ViewTemplatesView.Refresh();
        partial void OnViewTemplateSearchTextChanged(string value) => ViewTemplatesView.Refresh();

        private bool ViewTemplateFilter(object item)
        {
            if (item is not GarbageElementViewModel vm) return false;

            bool search = string.IsNullOrWhiteSpace(ViewTemplateSearchText)
                          || vm.Name.Contains(ViewTemplateSearchText, StringComparison.OrdinalIgnoreCase);

            bool filter = SelectedViewTemplateFilterIndex switch
            {
                1 => !vm.IsUsed,
                _ => true
            };

            return search && filter;
        }

        partial void OnSelectedFilterFilterIndexChanged(int value) => FiltersView.Refresh();
        partial void OnFilterSearchTextChanged(string value) => FiltersView.Refresh();

        private bool FiltersFilter(object item)
        {
            if (item is not GarbageElementViewModel vm) return false;

            bool search = string.IsNullOrWhiteSpace(FilterSearchText)
                          || vm.Name.Contains(FilterSearchText, StringComparison.OrdinalIgnoreCase);

            bool filter = SelectedFilterFilterIndex switch
            {
                1 => !vm.IsUsed,
                _ => true
            };

            return search && filter;
        }

        partial void OnSelectedMaterialFilterIndexChanged(int value) => MaterialsView.Refresh();
        partial void OnMaterialSearchTextChanged(string value) => MaterialsView.Refresh();

        private bool MaterialsFilter(object item)
        {
            if (item is not GarbageElementViewModel vm) return false;

            bool search = string.IsNullOrWhiteSpace(MaterialSearchText)
                          || vm.Name.Contains(MaterialSearchText, StringComparison.OrdinalIgnoreCase);

            bool filter = SelectedMaterialFilterIndex switch
            {
                1 => vm.Count == 0, // Unused
                _ => true
            };

            return search && filter;
        }

        partial void OnSelectedLineFilterIndexChanged(int value) => LinesView.Refresh();
        partial void OnLineSearchTextChanged(string value) => LinesView.Refresh();

        private bool LinesFilter(object item)
        {
            if (item is not GarbageElementViewModel vm) return false;

            bool search = string.IsNullOrWhiteSpace(LineSearchText)
                          || vm.Name.Contains(LineSearchText, StringComparison.OrdinalIgnoreCase);

            bool filter = SelectedLineFilterIndex switch
            {
                1 => vm.Count == 0, // Unused
                _ => true
            };

            return search && filter;
        }
        #endregion

        #region Select All Handlers
        partial void OnIsAllViewsCheckedChanged(bool value) => SetAllChecked(Views, ViewsView, value);
        partial void OnIsAllViewTemplatesCheckedChanged(bool value) => SetAllChecked(ViewTemplates, ViewTemplatesView, value);
        partial void OnIsAllFiltersCheckedChanged(bool value) => SetAllChecked(Filters, FiltersView, value);
        partial void OnIsAllMaterialsCheckedChanged(bool value) => SetAllChecked(Materials, MaterialsView, value);
        partial void OnIsAllLinesCheckedChanged(bool value) => SetAllChecked(Lines, LinesView, value);

        private static void SetAllChecked(ObservableCollection<GarbageElementViewModel> collection, ICollectionView view, bool value)
        {
            using (view.DeferRefresh())
            {
                foreach (var item in collection)
                    if (item.IsChecked != value)
                        item.IsChecked = value;
            }
        }
        #endregion

        #region Load Methods and Handlers
        public void LoadViewModels(IEnumerable<GarbageElementModel> models) 
        {
            LoadModels(models, Views); 
            ViewsLoaded = true; 
        }
        public void LoadViewTemplateModels(IEnumerable<GarbageElementModel> models) 
        {
            LoadModels(models, ViewTemplates); 
            ViewTemplatesLoaded = true; 
        }
        public void LoadFilterModels(IEnumerable<GarbageElementModel> models) 
        {
            LoadModels(models, Filters); 
            FiltersLoaded = true; 
        }
        public void LoadLineModels(IEnumerable<GarbageElementModel> models) 
        {
            LoadModels(models, Lines); 
            LinesLoaded = true; 
        }
        public void LoadMaterialModels(IEnumerable<GarbageElementModel> models) 
        {
            LoadModels(models, Materials); 
            MaterialsLoaded = true; 
        }

        private static void LoadModels(IEnumerable<GarbageElementModel> models, ObservableCollection<GarbageElementViewModel> collection)
        {
            collection.Clear();
            foreach (GarbageElementModel model in models)
                collection.Add(new GarbageElementViewModel(model));
        }
        #endregion
    }
}
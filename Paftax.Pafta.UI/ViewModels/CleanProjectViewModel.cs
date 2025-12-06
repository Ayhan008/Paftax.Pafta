using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Paftax.Pafta.Shared.Models;
using System.ComponentModel;

namespace Paftax.Pafta.UI.ViewModels
{
    public partial class CleanProjectViewModel<T> : ObservableObject where T : GarbageElementModel
    {
        public CleanProjectViewModel(
            FilterableElementCollectionViewModel<T> filterableViewsViewModel,
            FilterableElementCollectionViewModel<T> filterableParameterFiltersViewModel,
            FilterableElementCollectionViewModel<T> filterableLinesViewModel,
            FilterableElementCollectionViewModel<T> filterableMaterialsViewModel,
            FilterableElementCollectionViewModel<T> filterableViewTemplatesViewModel)
        {
            FilterableViewsViewModel = filterableViewsViewModel;
            FilterableParameterFiltersViewModel = filterableParameterFiltersViewModel;
            FilterableLinesViewModel = filterableLinesViewModel;
            FilterableMaterialsViewModel = filterableMaterialsViewModel;
            FilterableViewTemplatesViewModel = filterableViewTemplatesViewModel;

            Subscribe(FilterableViewsViewModel);
            Subscribe(FilterableParameterFiltersViewModel);
            Subscribe(FilterableLinesViewModel);
            Subscribe(FilterableMaterialsViewModel);
            Subscribe(FilterableViewTemplatesViewModel);

            CanClean = CheckAnySelection();
        }

        public FilterableElementCollectionViewModel<T> FilterableViewsViewModel { get; }
        public FilterableElementCollectionViewModel<T> FilterableParameterFiltersViewModel { get; }
        public FilterableElementCollectionViewModel<T> FilterableLinesViewModel { get; }
        public FilterableElementCollectionViewModel<T> FilterableMaterialsViewModel { get; }
        public FilterableElementCollectionViewModel<T> FilterableViewTemplatesViewModel { get; }

        [ObservableProperty]
        private int selectedTabIndex = 0;

        public event Action? CleanAction;
        public event Action? CancelAction;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(CleanCommand))]
        private bool canClean;

        [RelayCommand]
        private void Cancel() => CancelAction?.Invoke();

        [RelayCommand(CanExecute = nameof(CanClean))]
        private void Clean() => CleanAction?.Invoke();

        private bool CheckAnySelection()
        {
            return FilterableViewsViewModel.Elements.Any(e => e.IsChecked) ||
                   FilterableParameterFiltersViewModel.Elements.Any(e => e.IsChecked) ||
                   FilterableLinesViewModel.Elements.Any(e => e.IsChecked) ||
                   FilterableMaterialsViewModel.Elements.Any(e => e.IsChecked) ||
                   FilterableViewTemplatesViewModel.Elements.Any(e => e.IsChecked);
        }

        private void Subscribe(FilterableElementCollectionViewModel<T> vm)
        {
            foreach (var element in vm.Elements)
                element.PropertyChanged += ElementOnPropertyChanged;

            vm.Elements.CollectionChanged += (_, e) =>
            {
                if (e.NewItems != null)
                {
                    foreach (SelectableElementViewModel<T> item in e.NewItems)
                        item.PropertyChanged += ElementOnPropertyChanged;
                }

                if (e.OldItems != null)
                {
                    foreach (SelectableElementViewModel<T> item in e.OldItems)
                        item.PropertyChanged -= ElementOnPropertyChanged;
                }

                CanClean = CheckAnySelection();
            };
        }

        private void ElementOnPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(SelectableElementViewModel<T>.IsChecked))
                CanClean = CheckAnySelection();
        }
    }
}
using Paftax.Pafta.Shared.Models;
using Paftax.Pafta.UI.Dialogs;
using Paftax.Pafta.UI.ViewModels;
using System.Windows;

namespace Paftax.Pafta.UI.Services
{
    public class CleanGarbageDialogService
    {
        private readonly CleanGarbageViewModel _cleanGarbageViewModel = new();

        public Window Show()
        {
            DialogOptions dialogOptions = new()
            {
                Title = "Clean Project",
                Width = 500,
                Height = 800,
                Async = true
            };

            Window window = CommandDialog.Show(_cleanGarbageViewModel, dialogOptions);
            return window;
        }

        public event Action? CleanAction
        {
            add => _cleanGarbageViewModel.CleanAction += value;
            remove => _cleanGarbageViewModel.CleanAction -= value;
        }

        public event Action? CancelAction
        {
            add => _cleanGarbageViewModel.CancelAction += value;
            remove => _cleanGarbageViewModel.CancelAction -= value;
        }

        public Action? LoadFiltersRequested
        {
            get => _cleanGarbageViewModel.RequestLoadFilters;
            set => _cleanGarbageViewModel.RequestLoadFilters = value;
        }
        public Action? LoadMaterialsRequested
        {
            get => _cleanGarbageViewModel.RequestLoadMaterials;
            set => _cleanGarbageViewModel.RequestLoadMaterials = value;
        }
        public Action? LoadLinesRequested
        {
            get => _cleanGarbageViewModel.RequestLoadLines;
            set => _cleanGarbageViewModel.RequestLoadLines = value;
        }

        public void LoadFilterModels(IEnumerable<GarbageElementModel> models)
        {
            _cleanGarbageViewModel.LoadFilterModels(models);
        }

        public void LoadMaterialModels(IEnumerable<GarbageElementModel> models)
        {
            _cleanGarbageViewModel.LoadMaterialModels(models);
        }

        public void LoadViewModels(IEnumerable<GarbageElementModel> models)
        {
            _cleanGarbageViewModel.LoadViewModels(models);
        }

        public void LoadViewTemplateModels(IEnumerable<GarbageElementModel> models)
        {
            _cleanGarbageViewModel.LoadViewTemplateModels(models);
        }

        public void LoadLineModels(IEnumerable<GarbageElementModel> models)
        {
            _cleanGarbageViewModel.LoadLineModels(models);
        }

        public List<GarbageElementModel> GetSelectedMaterialModels()
        {
            return [.. _cleanGarbageViewModel.Materials
                .Where(vm => vm.IsChecked)
                .Select(vm => vm.Model)];
        }

        public List<GarbageElementModel> GetSelectedLineModels()
        {
            return [.. _cleanGarbageViewModel.Lines
                .Where(vm => vm.IsChecked)
                .Select(vm => vm.Model)];
        }

        public List<GarbageElementModel> GetSelectedFilterModels()
        {
            return [.. _cleanGarbageViewModel.Filters
                .Where(vm => vm.IsChecked)
                .Select(vm => vm.Model)];
        }

        public List<GarbageElementModel> GetSelectedViewModels()
        {
            return [.. _cleanGarbageViewModel.Views
                .Where(vm => vm.IsChecked)
                .Select(vm => vm.Model)];
        }

        public List<GarbageElementModel> GetSelectedViewTemplateModels()
        {
            return [.. _cleanGarbageViewModel.ViewTemplates
                .Where(vm => vm.IsChecked)
                .Select(vm => vm.Model)];
        }
    }
}

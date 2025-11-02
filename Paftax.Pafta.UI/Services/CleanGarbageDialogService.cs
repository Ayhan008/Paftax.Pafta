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

        public void LoadFilterModels(IEnumerable<FilterModel> models)
        {
            _cleanGarbageViewModel.LoadFilterModels(models);
        }

        public void LoadMaterialModels(IEnumerable<MaterialModel> models)
        {
            _cleanGarbageViewModel.LoadMaterialModels(models);
        }

        public void LoadViewModels(IEnumerable<ViewModel> models)
        {
            _cleanGarbageViewModel.LoadViewModels(models);
        }

        public void LoadViewTemplateModels(IEnumerable<ViewTemplateModel> models)
        {
            _cleanGarbageViewModel.LoadViewTemplateModels(models);
        }

        public void LoadLineModels(IEnumerable<LineModel> models)
        {
            _cleanGarbageViewModel.LoadLineModels(models);
        }

        public List<FilterModel> GetSelectedFilterModels()
        {
            return [.. _cleanGarbageViewModel.Filters.Where(model => model.IsChecked)];
        }

        public List<MaterialModel> GetSelectedMaterialModels()
        {
            return [.. _cleanGarbageViewModel.Materials.Where(model => model.IsChecked)];
        }

        public List<LineModel> GetSelectedLineModels()
        {
            return [.. _cleanGarbageViewModel.Lines.Where(model => model.IsChecked)];
        }

        public List<ViewModel> GetSelectedViewModels()
        {
            return [.. _cleanGarbageViewModel.Views.Where(model => model.IsChecked)];
        }

        public List<ViewTemplateModel> GetSelectedViewTemplateModels()
        {
            return [.. _cleanGarbageViewModel.ViewTemplates.Where(model => model.IsChecked)];
        }
    }
}

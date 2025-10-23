using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Paftax.Pafta.Revit2026.Events;
using Paftax.Pafta.Revit2026.Factories;
using Paftax.Pafta.Revit2026.Services;
using Paftax.Pafta.Shared.Models;
using Paftax.Pafta.UI.Services;
using System.Windows;
using System.Windows.Threading;

namespace Paftax.Pafta.Revit2026.Commands
{
    [Transaction(TransactionMode.Manual)]
    internal class CleanProjectCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            // Get Revit document
            UIApplication uiApplication = commandData.Application;
            UIDocument uiDocument = uiApplication.ActiveUIDocument;
            Document doc = uiDocument.Document;

            // Prepare data models
            List<ViewModel> viewModels = new ViewModelFactory(doc).CreateModels();
            List<ViewTemplateModel> viewTemplateModels = new ViewTemplateModelFactory(doc).CreateModels();

            // Set up External Event and Handler
            CleanGarbageHandler handler = new();
            ExternalEvent externalEvent = ExternalEvent.Create(handler);

            // Start UI thread
            Thread uiThread = new(() =>
            {
                // Create Dispatcher for the new thread
                Dispatcher threadDispatcher = Dispatcher.CurrentDispatcher;

                // Dialog Service
                CleanGarbageDialogService dialogService = new();
                dialogService.LoadViewModels(viewModels);
                dialogService.LoadViewTemplateModels(viewTemplateModels);
                Window window = dialogService.Show();

                CancellationTokenSource cancellationTokenSource = new();

                // Load Materials when requested
                dialogService.LoadMaterialsRequested = () =>
                {
                    handler.SetAction(app =>
                    {
                        List<MaterialModel> materialModels = new MaterialModelFactory(doc).Cancellable(cancellationTokenSource.Token).CreateModels();
                        threadDispatcher.Invoke(() => dialogService.LoadMaterialModels(materialModels));
                    });
                    externalEvent.Raise();
                };

                // Load Filters when requested
                dialogService.LoadFiltersRequested = () =>
                {
                    handler.SetAction(app =>
                    {
                        List<FilterModel> filterModels = new FilterModelFactory(doc).Cancellable(cancellationTokenSource.Token).CreateModels();
                        threadDispatcher.Invoke(() => dialogService.LoadFilterModels(filterModels));
                    });
                    externalEvent.Raise();
                };

                // Load Lines when requested
                dialogService.LoadLinesRequested = () =>
                {
                    handler.SetAction(app =>
                    {
                        List<LineModel> lineModels = new LineModelFactory(doc).Cancellable(cancellationTokenSource.Token).CreateModels();
                        threadDispatcher.Invoke(() => dialogService.LoadLineModels(lineModels));
                    });
                    externalEvent.Raise();
                };

                dialogService.CleanAction += () =>
                {
                    window.Close();

                    List<FilterModel> selectedFilters = dialogService.GetSelectedFilterModels();
                    List<MaterialModel> selectedMaterials = dialogService.GetSelectedMaterialModels();
                    List<LineModel> selectedLines = dialogService.GetSelectedLineModels();
                    List<ViewModel> selectedViewModels = dialogService.GetSelectedViewModels();
                    List<ViewTemplateModel> selectedViewTemplates = dialogService.GetSelectedViewTemplateModels();

                    List<ParameterFilterElement> filtersToDelete = new ElementCollectorService(doc).GetElementsByIds<ParameterFilterElement>(selectedFilters.Select(f => f.Id));
                    List<Material> materialsToDelete = new ElementCollectorService(doc).GetElementsByIds<Material>(selectedMaterials.Select(m => m.Id));
                    List<LinePatternElement> linesToDelete = new ElementCollectorService(doc).GetElementsByIds<LinePatternElement>(selectedLines.Select(l => l.Id));
                    List<View> viewTemplatesToDelete = new ElementCollectorService(doc).GetElementsByIds<View>(selectedViewTemplates.Select(vt => vt.Id));

                    handler.SetAction(uiApp =>
                    {
                        Document doc = uiApp.ActiveUIDocument.Document;

                        using Transaction t = new(doc, "Clean Project");
                        t.Start();

                        List<ElementId> idsToDelete =
                        [
                            .. selectedFilters.Select(f => new ElementId(f.Id)),
                            .. selectedMaterials.Select(m => new ElementId(m.Id)),
                            .. selectedViewModels.Select(v => new ElementId(v.Id)),
                            .. selectedLines.Select(l => new ElementId(l.Id)),
                            .. selectedViewTemplates.Select(vt => new ElementId(vt.Id)),
                        ];
                        doc.Delete(idsToDelete);

                        t.Commit();
                    });
                    externalEvent.Raise();
                };

                // Handle window closing and cancel operations
                window.Closed += (s, e) => { cancellationTokenSource.Cancel(); };
                dialogService.CancelAction += () => { window.Close(); };
                dialogService.CloseAction += () => { window.Close(); };
                Dispatcher.Run();
            });

            uiThread.SetApartmentState(ApartmentState.STA);
            uiThread.IsBackground = true;
            uiThread.Start();

            return Result.Succeeded;
        }
    }
}

using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Paftax.Pafta.Revit2026.Events;
using Paftax.Pafta.Revit2026.Factories;
using Paftax.Pafta.Revit2026.Services;
using Paftax.Pafta.Shared.Models;
using Paftax.Pafta.UI.Dialogs;
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
            UIApplication uiApplication = commandData.Application;
            UIDocument uiDocument = uiApplication.ActiveUIDocument;
            Document doc = uiDocument.Document;

            // Prepare Revit Window Service
            RevitWindowService windowService = new(uiApplication.MainWindowHandle);
            Dispatcher revitDispatcher = windowService.GetDispatcher();

            List<ViewModel> viewModels = new ViewModelFactory(doc).CreateModels();
            List<ViewTemplateModel> viewTemplateModels = new ViewTemplateModelFactory(doc).CreateModels();

            CleanGarbageHandler handler = new();
            ExternalEvent externalEvent = ExternalEvent.Create(handler);

            Thread uiThread = new(() =>
            {
                Dispatcher threadDispatcher = Dispatcher.CurrentDispatcher;

                CleanGarbageDialogService dialogService = new();
                dialogService.LoadViewModels(viewModels);
                dialogService.LoadViewTemplateModels(viewTemplateModels);

                CancellationTokenSource cancellationTokenSource = new();
                Window window = dialogService.Show();
                windowService.BlockRevitWhile(window);

                // Load Materials
                dialogService.LoadMaterialsRequested = () =>
                {
                    handler.SetAction(app =>
                    {
                        var materialModels = new MaterialModelFactory(doc)
                            .Cancellable(cancellationTokenSource.Token)
                            .CreateModels();
                        threadDispatcher.Invoke(() => dialogService.LoadMaterialModels(materialModels));
                    });
                    externalEvent.Raise();
                };

                // Load Filters
                dialogService.LoadFiltersRequested = () =>
                {
                    handler.SetAction(app =>
                    {
                        var filterModels = new FilterModelFactory(doc)
                            .Cancellable(cancellationTokenSource.Token)
                            .CreateModels();
                        threadDispatcher.Invoke(() => dialogService.LoadFilterModels(filterModels));
                    });
                    externalEvent.Raise();
                };

                // Load Lines
                dialogService.LoadLinesRequested = () =>
                {
                    handler.SetAction(app =>
                    {
                        var lineModels = new LineModelFactory(doc)
                            .Cancellable(cancellationTokenSource.Token)
                            .CreateModels();
                        threadDispatcher.Invoke(() => dialogService.LoadLineModels(lineModels));
                    });
                    externalEvent.Raise();
                };

                window.Closing += (s, e) =>
                {
                    cancellationTokenSource.Cancel();
                };

                dialogService.CancelAction += () =>
                {
                    revitDispatcher.BeginInvoke(DispatcherPriority.Background,
                        () => InfoDialog.Show("Clean Result", "Operation cancelled by user.", Shared.Enums.IconType.Error));

                    cancellationTokenSource.Cancel();
                    window.Close();      
                };

                dialogService.CleanAction += () =>
                {
                    window.Close();      

                    List<FilterModel> selectedFilters = dialogService.GetSelectedFilterModels();
                    List<MaterialModel> selectedMaterials = dialogService.GetSelectedMaterialModels();
                    List<LineModel> selectedLines = dialogService.GetSelectedLineModels();
                    List<ViewModel> selectedViewModels = dialogService.GetSelectedViewModels();
                    List<ViewTemplateModel> selectedViewTemplates = dialogService.GetSelectedViewTemplateModels();

                    int elementCount = selectedFilters.Count + selectedMaterials.Count +
                        selectedLines.Count + selectedViewModels.Count + selectedViewTemplates.Count;

                    revitDispatcher.BeginInvoke(DispatcherPriority.Background, 
                        () => InfoDialog.Show("Clean Result", $"{elementCount} elements deleted from project.", Shared.Enums.IconType.Info));

                    handler.SetAction(uiApp =>
                    {
                        var docInner = uiApp.ActiveUIDocument.Document;

                        using Transaction t = new(docInner, "Clean Project");
                        t.Start();

                        List<ElementId> idsToDelete =
                        [
                            .. selectedFilters.Select(f => new ElementId(f.Id)),
                            .. selectedMaterials.Select(m => new ElementId(m.Id)),
                            .. selectedViewModels.Select(v => new ElementId(v.Id)),
                            .. selectedLines.Select(l => new ElementId(l.Id)),
                            .. selectedViewTemplates.Select(vt => new ElementId(vt.Id)),
                        ];

                        docInner.Delete(idsToDelete);

                        t.Commit();
                    });

                    externalEvent.Raise();
                };
                Dispatcher.Run();
            });

            uiThread.SetApartmentState(ApartmentState.STA);
            uiThread.IsBackground = true;
            uiThread.Start();

            return Result.Succeeded;
        }
    }
}

using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Paftax.Pafta.Revit2026.Events;
using Paftax.Pafta.Revit2026.Factories;
using Paftax.Pafta.Shared.Models;
using Paftax.Pafta.UI.Dialogs;
using Paftax.Pafta.UI.Services;
using Paftax.Pafta.UI.ViewModels;
using System.Windows;
using System.Windows.Threading;

namespace Paftax.Pafta.Revit2026.Commands
{
    [Transaction(TransactionMode.Manual)]
    internal class CleanProjectCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Document doc = commandData.Application.ActiveUIDocument.Document;

            List<TagCategoryModel> tagCategoryModels = TagCategoryModelFactory.CreateModels(doc);
            List<ViewTemplateModel> viewTemplateModels = ViewTemplateModelFactory.CreateModels(doc);

            CleanGarbageHandler handler = new();
            ExternalEvent externalEvent = ExternalEvent.Create(handler);

            MaterialModelFactory.CancelRequested = false;
            FilterModelFactory.CancelRequested = false;
            LineModelFactory.CancelRequested = false;

            Thread uiThread = new(() =>
            {
                Dispatcher threadDispatcher = Dispatcher.CurrentDispatcher;

                CleanGarbageViewModel cleanGarbageViewModel = new();
                cleanGarbageViewModel.LoadTagCategoryModels(tagCategoryModels);
                cleanGarbageViewModel.LoadViewTemplateModels(viewTemplateModels);

                DialogOptions dialogOptions = new()
                {
                    Title = "Clean Project",
                    Width = 400,
                    Height = 700,
                    Async = true
                };

                Window window = CommandDialog.Show(cleanGarbageViewModel, dialogOptions);
                window.Closing += (s, e) =>
                {
                    MaterialModelFactory.CancelRequested = true;
                    FilterModelFactory.CancelRequested = true;
                    LineModelFactory.CancelRequested = true;
                };          

                cleanGarbageViewModel.RequestLoadMaterials = () =>
                {
                    handler.SetAction(app =>
                    {
                        if (!MaterialModelFactory.CancelRequested)
                        {
                            List<MaterialModel> materialModels = MaterialModelFactory.CreateModels(doc);
                            threadDispatcher.Invoke(() =>
                                cleanGarbageViewModel.LoadMaterialModels(materialModels));
                        }
                    });
                    externalEvent.Raise();
                };

                cleanGarbageViewModel.RequestLoadFilters = () =>
                {
                    handler.SetAction(app =>
                    {
                        if (!FilterModelFactory.CancelRequested)
                        {
                            List<FilterModel> filterModels = FilterModelFactory.CreateModels(doc);
                            threadDispatcher.Invoke(() =>
                                cleanGarbageViewModel.LoadFilterModels(filterModels));
                        }
                    });
                    externalEvent.Raise();
                };

                cleanGarbageViewModel.RequestLoadLines = () =>
                {
                    handler.SetAction(app =>
                    {
                        if (!LineModelFactory.CancelRequested)
                        {
                            List<LineModel> lineModels = LineModelFactory.CreateModels(doc);
                            threadDispatcher.Invoke(() =>
                                cleanGarbageViewModel.LoadLineModels(lineModels));
                        }
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

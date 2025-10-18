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

namespace Paftax.Pafta.Revit2026.Commands
{
    [Transaction(TransactionMode.Manual)]
    internal class CleanProjectCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiApp = commandData.Application;
            UIDocument uiDoc = uiApp.ActiveUIDocument;
            Document doc = uiDoc.Document;

            List<TagCategoryModel> tagCategoryModels = TagCategoryModelFactory.CreateModels(doc);
            List<ViewTemplateModel> viewTemplateModels = ViewTemplateModelFactory.CreateModels(doc);

            CleanGarbageViewModel cleanGarbageViewModel = new();     
            cleanGarbageViewModel.LoadTagCategoryModels(tagCategoryModels);
            cleanGarbageViewModel.LoadViewTemplateModels(viewTemplateModels);

            CleanGarbageHandler handler = new();
            ExternalEvent externalEvent = ExternalEvent.Create(handler);

            cleanGarbageViewModel.RequestLoadMaterials = () =>
            {
                handler.SetAction(app =>
                {
                    List<MaterialModel> materialModels = MaterialModelFactory.CreateModels(app.ActiveUIDocument.Document);
                    Application.Current.Dispatcher.BeginInvoke(() =>
                        cleanGarbageViewModel.LoadMaterialModels(materialModels));
                });
                externalEvent.Raise();
            };

            cleanGarbageViewModel.RequestLoadFilters = () =>
            {
                handler.SetAction(app =>
                {
                    List<FilterModel> filterModels = FilterModelFactory.CreateModels(app.ActiveUIDocument.Document);
                    Application.Current.Dispatcher.BeginInvoke(() =>
                        cleanGarbageViewModel.LoadFilterModels(filterModels));
                });
                externalEvent.Raise();
            };

            cleanGarbageViewModel.RequestLoadLines = () =>
            {
                handler.SetAction(app =>
                {
                    List<LineModel> lineModels = LineModelFactory.CreateModels(app.ActiveUIDocument.Document);
                    Application.Current.Dispatcher.BeginInvoke(() =>
                        cleanGarbageViewModel.LoadLineModels(lineModels));
                });
                externalEvent.Raise();
            };

            DialogOptions dialogOptions = new()
            {
                Title = "Clean Project",
                Width = 400,
                Height = 700,
                Async = true
            };

            CommandDialog.Show(cleanGarbageViewModel, dialogOptions);
            return Result.Succeeded;
        }
    }
}

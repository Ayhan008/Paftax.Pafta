using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Paftax.Pafta.Revit2026.Factories;
using Paftax.Pafta.Shared.Models;
using Paftax.Pafta.UI.Dialogs;
using Paftax.Pafta.UI.ViewModels;

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

            CleanGarbageViewModel cleanGarbageViewModel = new();
            cleanGarbageViewModel.LoadTagCategoryModels(tagCategoryModels);

            CommandDialog<CleanGarbageViewModel>.Show("Clean Project", 400, 700);
            InfoDialog.Show("Info", "Operation completed successfully.", Shared.Enums.IconType.Success);

            return Result.Succeeded;
        }
    }
}

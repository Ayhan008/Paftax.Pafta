using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Paftax.Pafta.UI.Dialogs;

namespace Paftax.Pafta.Revit2026.Commands
{
    [Transaction(TransactionMode.Manual)]
    internal class QuickExportCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;
            View activeView = doc.ActiveView;

            if (activeView == null)
            {
                TaskDialog.Show("Error", "No active view found!");
                return Result.Failed;
            }

            string projectName = Path.GetFileNameWithoutExtension(doc.PathName);
            if (string.IsNullOrEmpty(projectName))
                projectName = "CurrentFamily";

            string desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            string safeViewName = activeView.Name
                .Replace(":", "_")
                .Replace("/", "_")
                .Replace("\\", "_");

            string fileBaseName = $"{projectName}_{safeViewName}";
            string fullPath = Path.Combine(desktop, fileBaseName);

            ImageExportOptions options = new()
            {
                ExportRange = ExportRange.CurrentView,
                FilePath = fullPath,
                HLRandWFViewsFileType = ImageFileType.PNG,
                ImageResolution = ImageResolution.DPI_300,
                FitDirection = FitDirectionType.Horizontal,
                ZoomType = ZoomFitType.Zoom,
                Zoom = 100
            };

            if (!doc.IsFamilyDocument)
                options.ViewName = activeView.Name;

            try
            {
                doc.ExportImage(options);
            }
            catch (Exception ex)
            {
                InfoDialog.Show("Export Error", $"Export failed:\n{ex.Message}");
                return Result.Failed;
            }

            string outputPath = fullPath + ".png";
            if (File.Exists(outputPath))
            {
                InfoDialog.Show("Success", $"PNG created at:\n{outputPath}");
                return Result.Succeeded;
            }
            else
            {
                InfoDialog.Show("Error", $"PNG file not found at:\n{outputPath}");
                return Result.Failed;
            }
        }
    }
}

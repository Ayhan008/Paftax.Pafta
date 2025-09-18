using Autodesk.Revit.UI;
using Paftax.Pafta.Revit2026.Services.Revit;
using System.Reflection;

namespace Paftax.Pafta.Revit2026
{
    public class App : IExternalApplication
    {
        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }

        public Result OnStartup(UIControlledApplication application)
        {
            string tabName = "Pafta";
            application.CreateRibbonTab(tabName);

            #region Sheet Creation Panel
            RibbonPanel sheetCreationPanel = application.CreateRibbonPanel(tabName, "Sheet Creation");

            // Room to Sheet Button
            PushButtonData roomToSheetPushButtonData = new(
                "RoomToSheetPushButton",
                "Room to\nSheet",
                typeof(App).Assembly.Location,
                "Paftax.Pafta.Revit2026.Commands.RoomToSheetCommand")
            {
                ToolTip = "Select room to create sheet and populate it with views of it.",
                LongDescription = "Select room to create sheet and populate it with views of it. " +
                                  "You can choose titleblock family, view types to be created, " +
                                  "and set naming conventions for views and sheets."
            };
            sheetCreationPanel.AddItem(roomToSheetPushButtonData);

            // Create Sheets Button
            PushButtonData createSheetsPushButtonData = new(
                "CreateSheetsPushButton",
                "Create\nSheets",
                typeof(App).Assembly.Location,
                "Paftax.Pafta.Revit2026.Commands.CreateSheetsCommand")
            {
                ToolTip = "Create multiple sheets at once based on desired parameters",
                LongDescription = "Create multiple sheets at once based on a desired parameters. " +
                                  "You can import sheet list from Excel or CSV file, " +
                                  "or use parameters now."
            };
            sheetCreationPanel.AddItem(createSheetsPushButtonData);

            // View to Sheet Button
            PushButtonData viewToSheetPushButtonData = new(
                "ViewToSheetPushButton",
                "View to\nSheet",
                typeof(App).Assembly.Location,
                "Paftax.Pafta.Revit2026.Commands.ViewToSheetCommand")
            {
                ToolTip = "Create sheet for active view",
                LongDescription = "Create sheet for active view. " +
                                  "You can choose titleblock family, " +
                                  "and set naming conventions for views and sheets."
            };
            sheetCreationPanel.AddItem(viewToSheetPushButtonData);
            #endregion

            #region Sheet Editing Panel
            RibbonPanel sheetEditingPanel = application.CreateRibbonPanel(tabName, "Sheet Editing");

            // Copy Schedules Button
            PushButtonData copySchedulesPushButtonData = new(
                "CopySchedulesPushButton",
                "Copy\nSchedules",
                typeof(App).Assembly.Location,
                "Paftax.Pafta.Revit2026.Commands.CopySchedulesCommand")
            {
                ToolTip = "Copy schedules from one sheet to multiple sheets",
                LongDescription = "Select schedule in the active view. " +
                                  "Then you can select target sheets." +
                                  "and the schedules will be copied to the same position on the target sheets."
            };
            sheetEditingPanel.AddItem(copySchedulesPushButtonData);

            // Move Viewports Button
            PushButtonData moveViewportsPushButtonData = new(
                "MoveViewportsPushButton",
                "Move\nViewports",
                typeof(App).Assembly.Location,
                "Paftax.Pafta.Revit2026.Commands.MoveViewportsCommand")
            {
                ToolTip = "Move viewports on multiple sheets",
                LongDescription = "Select viewports in the active view. " +
                                  "Then you can select target sheets." +
                                  "and the viewports will be moved to the same position on the target sheets."
            };
            sheetEditingPanel.AddItem(moveViewportsPushButtonData);

            // Edit Sheet Properties Button
            PushButtonData editSheetPropertiesPushButtonData = new(
                "EditSheetPropertiesPushButtonData",
                "Edit Sheet\nProperties",
                typeof(App).Assembly.Location,
                "Paftax.Pafta.Revit2026.Commands.EditSheetPropertiesCommand")
            {
                ToolTip = "Edit sheet properties in multiple sheets",
                LongDescription = "Select sheets in the list." +
                                  "You can Revision numbers etc."
            };
            sheetEditingPanel.AddItem(editSheetPropertiesPushButtonData);
            #endregion

            #region Export Panel
            RibbonPanel exportPanel = application.CreateRibbonPanel(tabName, "Export");

            // Export Sheet Button
            PushButtonData exportSheetsPushButtonData = new(
                "ExportSheetsPushButton",
                "Export\nSheets",
                typeof(App).Assembly.Location,
                "Paftax.Pafta.Revit2026.Commands.ExportSheetsCommand")
            {
                ToolTip = "Export sheets to PDF, DWG, DXF or PNG",
                LongDescription = "Export sheets to PDF, DWG, DXF or PNG. " +
                                  "You can choose export format, " +
                                  "naming conventions and destination folder."
            };
            exportPanel.AddItem(exportSheetsPushButtonData);

            // Export Schedule Button
            PushButtonData exportSchedulePushButtonData = new(
                "ExportSchedulePushButton",
                "Export\nSchedule",
                Assembly.GetExecutingAssembly().Location,
                "Paftax.Pafta.Revit2026.Commands.ExportScheduleCommand")
            {
                ToolTip = "Export schedules to Excel or CSV",
                LongDescription = "Export schedules to Excel or CSV. " +
                                  "You can export schedule in seperate Spreadsheets or Merged Workbooks"
            };
            exportPanel.AddItem(exportSchedulePushButtonData);

            // Quick Export Button
            PushButtonData quickExportPushButtonData = new(
                "QuickExportPushButton",
                "Quick\nExport",
                Assembly.GetExecutingAssembly().Location,
                "Paftax.Pafta.Revit2026.Commands.QuickExportCommand")
            {
                ToolTip = "Export active view as DWG, PDF or PNG",
                LongDescription = "Export active view as DWG, PDF or PNG. " +
                                  "You can choose export format, " +
                                  "naming conventions and destination folder."
            };
            exportPanel.AddItem(quickExportPushButtonData);
            #endregion

            #region Settings Panel
            RibbonPanel settingsPanel = application.CreateRibbonPanel(tabName, "Settings");

            // Settings Button
            PushButtonData settingsPushButtonData = new(
                "SettingsPushButton",
                "Settings",
                typeof(App).Assembly.Location,
                "Paftax.Pafta.Revit2026.Commands.SettingsCommand")
            {
                ToolTip = "Open Pafta settings",
                LongDescription = "Open Pafta settings. " +
                                  "You can set default parameters for various tools."
            };
            settingsPanel.AddItem(settingsPushButtonData);
            #endregion

            #region Update Image by Theme
            ApplicationThemeService themeService = new(application, tabName);
            themeService.Initialize();
            application.ThemeChanged += themeService.OnThemeChanged;
            #endregion

            return Result.Succeeded;
        }
    }
}

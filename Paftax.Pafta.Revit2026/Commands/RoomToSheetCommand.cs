using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Paftax.Pafta.Shared.Enums;
using Paftax.Pafta.Shared.Models;
using Paftax.Pafta.UI.Dialogs;
using Paftax.Pafta.UI.Services;
using Paftax.Pafta.UI.ViewModels;

namespace Paftax.Pafta.Revit2026.Commands
{
    [Transaction(TransactionMode.Manual)]
    internal class RoomToSheetCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiApplication = commandData.Application;
            UIDocument uiDocument = uiApplication.ActiveUIDocument;
            Document document = uiDocument.Document;
            View view = document.ActiveView;

            if (view.ViewType != Autodesk.Revit.DB.ViewType.FloorPlan)
            {
                InfoDialog.Show("Error", "Please switch to a floor plan view to use this command.", FluentIcon.Error);
                return Result.Cancelled;
            }

            List<Room> selectedRooms = SelectRooms(uiDocument, document);

            if (selectedRooms.Count == 0)
            {
                return Result.Cancelled;
            }

            List<RoomModel> richRoomDataModels = [];
            foreach (Room room in selectedRooms)
            {

            }

            RoomToSheetViewModel roomToSheetViewModel = new();
            roomToSheetViewModel.LoadRoomModels(richRoomDataModels);

            DialogOptions dialogOptions = new()
            {
                Title = "Room To Sheet",
                Width = 900,
                Height = 700
            };

            CommandDialog.Show(roomToSheetViewModel, dialogOptions);
            return Result.Succeeded;
        }

        private static List<Room> SelectRooms(UIDocument uiDocument, Document document)
        {
            RoomSelectionFilter filter = new(document);
            List<Room> selectedRooms = [];

            try
            {
                IList<Reference> selectedReferences = uiDocument.Selection
                    .PickObjects(ObjectType.LinkedElement, filter, "Select Rooms");

                foreach (Reference reference in selectedReferences)
                {
                    Element element = document.GetElement(reference);

                    if (element is Room room)
                    {
                        selectedRooms.Add(room);
                    }
                    else if (element is RevitLinkInstance linkInstance)
                    {
                        Document linkDoc = linkInstance.GetLinkDocument();
                        Element linkedElement = linkDoc.GetElement(reference.LinkedElementId);
                        if (linkedElement is Room linkedRoom)
                        {
                            selectedRooms.Add(linkedRoom);
                        }
                    }
                }
            }
            catch (Autodesk.Revit.Exceptions.OperationCanceledException)
            {
                InfoDialog.Show("Cancelled", "Operation cancelled by user.", FluentIcon.Info);
                return selectedRooms;
            }

            return selectedRooms;
        }


        private class RoomSelectionFilter(Document document) : ISelectionFilter
        {
            public bool AllowElement(Element elem)
            {
                if (elem is Room)
                    return true;

                else if (elem is RevitLinkInstance)
                    return true;

                return false;
            }
            public bool AllowReference(Reference reference, XYZ position)
            {
                Element linkElement = document.GetElement(reference);
                if (linkElement is RevitLinkInstance linkInstance)
                {
                    Element element = linkInstance.GetLinkDocument().GetElement(reference.LinkedElementId);

                    if (element is Room) return true;
                }
                return false;
            }
        }
    }
}

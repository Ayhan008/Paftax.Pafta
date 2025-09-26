using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Paftax.Pafta.Revit2026.Factories;
using Paftax.Pafta.Shared.Models;
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

            List<RichRoomDataModel> richRoomDataModels = [];
            List<Room> selectedRooms = SelectRooms(uiDocument, document);
            foreach (Room room in selectedRooms)
            {
                RichRoomDataModel richRoomDataModel = RichRoomDataModelFactory.Create(room);
                richRoomDataModels.Add(richRoomDataModel);
            }
            
            RoomToSheetViewModel roomToSheetViewModel = new();
            roomToSheetViewModel.LoadData(richRoomDataModels);

            DialogService<RoomToSheetViewModel> dialogService = new(roomToSheetViewModel);
            dialogService.ShowDialog("Room To Sheet", 1200, 900);

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

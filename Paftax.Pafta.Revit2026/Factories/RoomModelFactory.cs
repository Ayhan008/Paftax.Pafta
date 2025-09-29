using Autodesk.Revit.DB.Architecture;
using Paftax.Pafta.Shared.Models;

namespace Paftax.Pafta.Revit2026.Factories
{
    internal class RoomModelFactory
    {
        public static RoomModel CreateRoomModelFromRevit(Room room)
        {
            RoomModel roomModel = new()
            {
                RoomGeometry = RoomGeometryFactory.CreateRoomFromRevit(room),
                Name = room.Name,
                Number = room.Number
            };
            return roomModel;
        }
    }
}

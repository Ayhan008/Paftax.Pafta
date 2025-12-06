using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Paftax.Pafta.Shared.Models;
using Paftax.Pafta.Shared.Models.Element;
using Paftax.Pafta.Shared.Models.Geometry;

namespace Paftax.Pafta.Revit2026.Factories
{
    internal static class RoomModelFactory
    {
        public static RoomModel Create(Room room)
        {
            SpatialElementBoundaryOptions options = new()
            {
                SpatialElementBoundaryLocation = SpatialElementBoundaryLocation.CoreCenter
            };

            IList<IList<Autodesk.Revit.DB.BoundarySegment>> boundarySegments = room.GetBoundarySegments(options);

            RoomModel roomModel = new()
            {
                Id = room.Id.Value,
                Name = room.Name,
                Area = room.Area,
                Volume = room.Volume,
                LevelId = room.LevelId.Value,
                Number = room.Number,
                BoundarySegments = CreateSegments(boundarySegments)
            };
            return roomModel;
        }

        private static List<List<Shared.Models.BoundarySegment>> CreateSegments(IList<IList<Autodesk.Revit.DB.BoundarySegment>> boundarySegments)
        {
            List<List<Shared.Models.BoundarySegment>> result = [];
            foreach (IList<Autodesk.Revit.DB.BoundarySegment> segmentList in boundarySegments)
            {
                List<Shared.Models.BoundarySegment> segmentModels = [];
                foreach (Autodesk.Revit.DB.BoundarySegment segment in segmentList)
                {
                    Curve curve = segment.GetCurve();
                    CurveModel curveModel = CurveModelFactory.Create(curve);

                    Shared.Models.BoundarySegment segmentModel = new()
                    {
                        Curves = [curveModel]
                    };
                    segmentModels.Add(segmentModel);
                }
                result.Add(segmentModels);
            }
            return result;
        }
    }
}

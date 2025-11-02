using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Paftax.Pafta.Drawing.Elements;
using Paftax.Pafta.Drawing.Geometries;
using Paftax.Pafta.Drawing.Structs;
using Paftax.Pafta.Shared.Models;

namespace Paftax.Pafta.Revit2026.Factories
{
    internal class RoomModelFactory
    {
        public static RoomModel CreateRoomModelFromRevit(Room room)
        {
            RoomModel roomModel = new()
            {
                Boundary = CreateSpatialBoundaryFromRevit(room),
                Name = room.Name,
                Number = room.Number
            };
            return roomModel;
        }

        private static SpatialBoundary CreateSpatialBoundaryFromRevit(Room room)
        {
            SpatialBoundary spatialBoundary = new();

            List<Paftax.Pafta.Drawing.Geometries.Curve2> roomBoundarySegments = [];

            SpatialElementBoundaryOptions spatialElementBoundaryOptions = new();

            IList<IList<BoundarySegment>> boundarySegments = room.GetBoundarySegments(spatialElementBoundaryOptions);

            foreach (IList<BoundarySegment> segmentList in boundarySegments)
            {
                foreach (BoundarySegment segment in segmentList)
                {
                    Autodesk.Revit.DB.Curve curve = segment.GetCurve();

                    if (curve is Autodesk.Revit.DB.Line line)
                    {
                        Point2 startPoint = new(line.GetEndPoint(0).X, line.GetEndPoint(0).Y);
                        Point2 endPoint = new(line.GetEndPoint(1).X, line.GetEndPoint(1).Y);

                        roomBoundarySegments.Add(new Paftax.Pafta.Drawing.Geometries.Line2(startPoint, endPoint));
                    }

                    else if (curve is Autodesk.Revit.DB.Arc arc)
                    {
                        Point2 startPoint = new(arc.GetEndPoint(0).X, arc.GetEndPoint(0).Y);
                        Point2 endPoint = new(arc.GetEndPoint(1).X, arc.GetEndPoint(1).Y);
                        Point2 centerPoint = new(arc.Center.X, arc.Center.Y);
                        double radius = arc.Radius;

                        roomBoundarySegments.Add(new Paftax.Pafta.Drawing.Geometries.Arc2(startPoint, endPoint, centerPoint, radius));
                    }
                }
            }
            spatialBoundary.BoundarySegments = roomBoundarySegments;
            return spatialBoundary;
        }
    }
}

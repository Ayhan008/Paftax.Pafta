using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Paftax.Pafta.Drawing.Entities;
using Paftax.Pafta.Drawing.Structs;
using Paftax.Pafta.Revit2026.Utilities;

namespace Paftax.Pafta.Revit2026.Factories
{
    internal class RoomGeometryFactory
    {
        public static RoomGeometry CreateRoomFromRevit(Room revitRoom)
        {
            ArgumentNullException.ThrowIfNull(revitRoom);

            Autodesk.Revit.DB.BoundingBoxXYZ bbox = revitRoom.get_BoundingBox(null);

            Drawing.Structs.BoundingBoxXYZ bounding = new()
            {
                Min = new PointXYZ(bbox.Min.X, bbox.Min.Y, bbox.Min.Z),
                Max = new PointXYZ(bbox.Max.X, bbox.Max.Y, bbox.Max.Z)
            };

            RoomGeometry room = new()
            {
                Id = new Drawing.Structs.ElementId(revitRoom.Id.ToLong()),
                Name = revitRoom.Name,
                Number = revitRoom.Number,
                Area = revitRoom.Area,
                Bounding = bounding
            };

            var options = new SpatialElementBoundaryOptions();
            IList<IList<BoundarySegment>> loops = revitRoom.GetBoundarySegments(options);

            if (loops != null)
            {
                foreach (var loop in loops)
                {
                    foreach (var segment in loop)
                    {
                        Curve curve = segment.GetCurve();

                        if (curve is Line line)
                        {
                            room.BoundarySegments.Add(
                                new Drawing.Geometries.Line(
                                    new PointXY(line.GetEndPoint(0).X, line.GetEndPoint(0).Y),
                                    new PointXY(line.GetEndPoint(1).X, line.GetEndPoint(1).Y)
                                )
                            );
                        }

                        else if (curve is Arc arc)
                        {
                            PointXY startPoint = new(arc.GetEndPoint(0).X, arc.GetEndPoint(0).Y);
                            PointXY endPoint = new(arc.GetEndPoint(1).X, arc.GetEndPoint(1).Y);
                            PointXY centerPoint = new(arc.Center.X, arc.Center.Y);
                            double radius = arc.Radius;

                            Drawing.Geometries.Arc arc2 = new(startPoint, endPoint, centerPoint, radius);

                            room.BoundarySegments.Add(arc2);
                        }
                    }
                }
            }

            return room;
        }
    }
}

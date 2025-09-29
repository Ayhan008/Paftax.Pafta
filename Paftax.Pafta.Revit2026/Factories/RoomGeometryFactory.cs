using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Paftax.Pafta.Drawing.Geometries;
using Paftax.Pafta.Drawing.Geometries.Primitives;
using Paftax.Pafta.Revit2026.Utilities;

namespace Paftax.Pafta.Revit2026.Factories
{
    internal class RoomGeometryFactory
    {
        public static Drawing.Elements.Room CreateRoomFromRevit(Room revitRoom)
        {
            ArgumentNullException.ThrowIfNull(revitRoom);

            BoundingBoxXYZ bbox = revitRoom.get_BoundingBox(null);

            BoundingXY bounding = new();
            bounding.IncludePoint(new PointXY(bbox.Min.X, bbox.Min.Y));
            bounding.IncludePoint(new PointXY(bbox.Max.X, bbox.Max.Y));

            Drawing.Elements.Room room = new()
            {
                ElementId = revitRoom.Id.ToLong(),
                Name = revitRoom.Name,
                Number = revitRoom.Number,
                Area = revitRoom.Area
            };
            room.BoundingXY.IncludeBounding(bounding);

            var options = new SpatialElementBoundaryOptions();
            IList<IList<BoundarySegment>> loops = revitRoom.GetBoundarySegments(options);

            if (loops != null)
            {
                foreach (var loop in loops)
                {
                    foreach (var segment in loop)
                    {
                        Autodesk.Revit.DB.Curve curve = segment.GetCurve();

                        if (curve is Autodesk.Revit.DB.Line line)
                        {
                            room.BoundarySegments.Add(
                                new Drawing.Geometries.Primitives.Line(
                                    new PointXY(line.GetEndPoint(0).X, line.GetEndPoint(0).Y),
                                    new PointXY(line.GetEndPoint(1).X, line.GetEndPoint(1).Y)
                                )
                            );
                        }                  
                        else if (curve is Autodesk.Revit.DB.Arc arc)
                        {
                            PointXY startPoint = new(arc.GetEndPoint(0).X, arc.GetEndPoint(0).Y);
                            PointXY endPoint = new(arc.GetEndPoint(1).X, arc.GetEndPoint(1).Y);
                            PointXY centerPoint = new(arc.Center.X, arc.Center.Y);
                            double radius = arc.Radius;

                            Drawing.Geometries.Primitives.Arc arc2 = new(startPoint, endPoint, centerPoint, radius);

                            room.BoundarySegments.Add(arc2);
                        }
                    }
                }
            }

            return room;
        }
    }
}

using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Paftax.Pafta.Drawing.Structs;
using Paftax.Pafta.Revit2026.Utilities;

namespace Paftax.Pafta.Revit2026.Factories
{
    internal class RoomGeometryFactory
    {
        public static Drawing.Elements.Room CreateRoomFromRevit(Room revitRoom)
        {
            ArgumentNullException.ThrowIfNull(revitRoom);

            BoundingBoxXYZ bbox = revitRoom.get_BoundingBox(null);

            Bounding bounding = new(new XY(bbox.Min.X, bbox.Min.Y), new XY(bbox.Max.X, bbox.Max.Y));

            Drawing.Elements.Room room = new()
            {
                ElementId = revitRoom.Id.ToLong(),
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
                                    new XY(line.GetEndPoint(0).X, line.GetEndPoint(0).Y),
                                    new XY(line.GetEndPoint(1).X, line.GetEndPoint(1).Y)
                                )
                            );
                        }

                        else if (curve is Arc arc)
                        {
                            XY startPoint = new(arc.GetEndPoint(0).X, arc.GetEndPoint(0).Y);
                            XY endPoint = new(arc.GetEndPoint(1).X, arc.GetEndPoint(1).Y);
                            XY centerPoint = new(arc.Center.X, arc.Center.Y);
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

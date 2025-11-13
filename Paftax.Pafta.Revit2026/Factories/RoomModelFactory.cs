using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Paftax.Pafta.Drawings;
using Paftax.Pafta.Drawings.Elements;
using Paftax.Pafta.Shared.Geometries;
using Paftax.Pafta.Shared.Models;

namespace Paftax.Pafta.Revit2026.Factories
{
    internal class RoomModelFactory
    {
        public static RoomModel Create(Room room)
        {
            RoomModel roomModel = new()
            {
                Name = room.Name,
                Number = room.Number,
                Area = room.Area,
                Volume = room.Volume,
                Boundaries = ExtractBoundaries(room)
            };

            return roomModel;
        }

        private static List<CurveModel> ExtractBoundaries(Room room)
        {
            List<CurveModel> boundaries = [];

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

                        boundaries.Add(new LineModel
                        {
                            Start = startPoint,
                            End = endPoint
                        });
                    }
                    else if (curve is Autodesk.Revit.DB.Arc arc)
                    {
                        Point2 startPoint = new(arc.GetEndPoint(0).X, arc.GetEndPoint(0).Y);
                        Point2 endPoint = new(arc.GetEndPoint(1).X, arc.GetEndPoint(1).Y);
                        Point2 centerPoint = new(arc.Center.X, arc.Center.Y);
                        double radius = arc.Radius;

                        boundaries.Add(new ArcModel
                        {
                            Start = startPoint,
                            End = endPoint,
                            Center = centerPoint,
                            Radius = radius
                        });
                    }
                }
            }

            return boundaries;
        }

        private static SpatialBoundary CreateSpatialBoundaryFromRevit(Room room)
        {
            SpatialBoundary spatialBoundary = new();

            List<Drawings.Curve> roomBoundarySegments = [];

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

                        roomBoundarySegments.Add(new Drawings.Line(startPoint, endPoint));
                    }

                    else if (curve is Autodesk.Revit.DB.Arc arc)
                    {
                        Point2 startPoint = new(arc.GetEndPoint(0).X, arc.GetEndPoint(0).Y);
                        Point2 endPoint = new(arc.GetEndPoint(1).X, arc.GetEndPoint(1).Y);
                        Point2 centerPoint = new(arc.Center.X, arc.Center.Y);
                        double radius = arc.Radius;

                        roomBoundarySegments.Add(new Drawings.Arc(startPoint, endPoint, centerPoint, radius));
                    }
                }
            }
            spatialBoundary.Segments = roomBoundarySegments;
            return spatialBoundary;
        }
    }
}

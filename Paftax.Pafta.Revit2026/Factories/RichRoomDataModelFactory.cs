using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Paftax.Pafta.Revit2026.Utilities;
using Paftax.Pafta.Shared.Models;
using System.Numerics;

namespace Paftax.Pafta.Revit2026.Factories
{
    internal class RichRoomDataModelFactory
    {
        public static RichRoomDataModel Create(Room room)
        {
            RichRoomDataModel richRoomDataModel = new()
            {
                Id = room.Id.ToLong(),
                Name = room.Name,
                Number = room.Number,
                Area = room.Area,
                HostWalls = GetRoomHostWalls(room),
                RoomGeometryData = GetRoomGeometryData(room),
                BoundingBoxData = GetBoundingBoxData(room)
            };
            return richRoomDataModel;
        }

        private static List<RoomHostWallModel> GetRoomHostWalls(Room room)
        {
            List<RoomHostWallModel> hostWalls = [];

            SpatialElementBoundaryOptions options = new()
            {
                StoreFreeBoundaryFaces = true,
                SpatialElementBoundaryLocation = SpatialElementBoundaryLocation.Finish
            };

            var boundaries = room.GetBoundarySegments(options);
            if (boundaries == null) return hostWalls;

            foreach (var boundaryList in boundaries)
            {
                foreach (var boundary in boundaryList)
                {
                    Element element = room.Document.GetElement(boundary.ElementId);

                    Wall? wall = null;

                    if (element is Wall directWall)
                        wall = directWall;
                    else if (element is FamilyInstance fi && fi.Host is Wall hostWall)
                        wall = hostWall;

                    if (wall != null)
                    {
                        Curve boundaryCurve = boundary.GetCurve();
                        XYZ start = boundaryCurve.GetEndPoint(0);
                        XYZ end = boundaryCurve.GetEndPoint(1);

                        RoomHostWallModel model = new()
                        {
                            HostBoundarySegmentId = boundary.ElementId.ToLong(),
                            HostRoomId = room.Id.ToLong(),
                            Id = wall.Id.ToLong(),
                            Length = wall.Location is LocationCurve lc ? lc.Curve.Length : 0,
                            Height = wall.get_Parameter(BuiltInParameter.WALL_USER_HEIGHT_PARAM)?.AsDouble() ?? 0,
                            IntersectedLength = new Vector2(
                                (float)(end.X - start.X),
                                (float)(end.Y - start.Y)
                            ),
                            StartPointOnInsersection = new System.Drawing.Point((int)start.X, (int)start.Y),
                            EndPointOnInsersection = new System.Drawing.Point((int)end.X, (int)end.Y)
                        };

                        if (!hostWalls.Any(w => w.Id == model.Id &&
                                                w.StartPointOnInsersection == model.StartPointOnInsersection &&
                                                w.EndPointOnInsersection == model.EndPointOnInsersection))
                        {
                            hostWalls.Add(model);
                        }
                    }
                }
            }

            return hostWalls;
        }

        private static RoomGeometryData GetRoomGeometryData(Room room)
        {
            RoomGeometryData data = new()
            {
                Area = (float)room.Area,
                Volume = (float)room.Volume
            };

            SpatialElementBoundaryOptions options = new()
            {
                StoreFreeBoundaryFaces = true,
                SpatialElementBoundaryLocation = SpatialElementBoundaryLocation.Finish
            };

            var boundaries = room.GetBoundarySegments(options);
            if (boundaries == null) return data;

            HashSet<System.Drawing.Point> pointsSet = [];

            foreach (var boundaryList in boundaries)
            {
                foreach (var boundary in boundaryList)
                {
                    Curve curve = boundary.GetCurve();
                    XYZ start = curve.GetEndPoint(0);
                    XYZ end = curve.GetEndPoint(1);

                    pointsSet.Add(new System.Drawing.Point((int)start.X, (int)start.Y));
                    pointsSet.Add(new System.Drawing.Point((int)end.X, (int)end.Y));
                }
            }

            // HashSet zaten tekrar edenleri otomatik kaldırır
            data.GeomertyPoints = [.. pointsSet];

            return data;
        }

        private static BoundingBoxData GetBoundingBoxData(Room room)
        {
            BoundingBoxXYZ? boundingBox = room.get_BoundingBox(null);
            if (boundingBox == null)
                return new BoundingBoxData();

            XYZ min = boundingBox.Min;
            XYZ max = boundingBox.Max;
            return new BoundingBoxData
            {
                Min = new Vector3((float)min.X, (float)min.Y, (float)min.Z),
                Max = new Vector3((float)max.X, (float)max.Y, (float)max.Z)
            };
        }
    }
}

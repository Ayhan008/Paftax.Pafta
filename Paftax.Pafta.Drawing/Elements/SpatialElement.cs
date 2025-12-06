using System.Windows;
using System.Windows.Media;
using Paftax.Pafta.Drawings.Visuals;
using Paftax.Pafta.Shared.Models.Element;
using Paftax.Pafta.Shared.Models.Geometry;

namespace Paftax.Pafta.Drawings.Elements
{
    public class SpatialElement : DrawingElement
    {
        private readonly SpatialElementVisual _visual = new();
        public override GeometryVisual Visual => _visual;

        public override void Create(ElementModel elementModel)
        {
            if (elementModel is not RoomModel roomModel)
                return;

            var path = new PathGeometry();

            foreach (var boundary in roomModel.BoundarySegments)
            {
                var figure = new PathFigure { IsClosed = true, IsFilled = false };
                bool hasStart = false;

                foreach (var segment in boundary)
                {
                    foreach (var curve in segment.Curves)
                    {
                        if (!hasStart)
                        {
                            figure.StartPoint = new Point(curve.Start.X, curve.Start.Y);
                            hasStart = true;
                        }

                        var endPoint = new Point(curve.End.X, curve.End.Y);

                        switch (curve)
                        {
                            case LineModel:
                                figure.Segments.Add(new LineSegment(endPoint, true));
                                break;

                            case ArcModel arc:
                                var size = new Size(Math.Abs(arc.Radius), Math.Abs(arc.Radius));
                                var sweep = arc.IsClockwise ? SweepDirection.Clockwise : SweepDirection.Counterclockwise;
                                figure.Segments.Add(new ArcSegment(endPoint, size, 0, false, sweep, true));
                                break;

                            default:
                                figure.Segments.Add(new LineSegment(endPoint, true));
                                break;
                        }
                    }
                }

                if (hasStart)
                    path.Figures.Add(figure);
            }

            _visual.SourceGeometry = path;
            _visual.UpdateGeometry();
            Bounding = _visual.Bounding;
            InvalidateVisual();
        }
    }
}
using System.Windows;
using System.Windows.Media;

namespace Paftax.Pafta.Drawings.Visuals
{
    public class SpatialBoundaryVisual : GeometryVisual
    {
        public List<Curve> BoundarySegments { get; set; } = [];
        public new double StrokeThickness { get; set; } = UnitConverter.MmToFeet(200);

        public override void Draw(DrawingContext dc)
        {
            var geom = BuildPathGeometry();
            if (geom == null) return;

            Pen pen = new(Stroke, StrokeThickness * Scale);
            dc.DrawGeometry(null, pen, geom);
        }

        public override Geometry GetGeometry()
        {
            throw new NotImplementedException();
        }

        private PathGeometry? BuildPathGeometry()
        {
            if (BoundarySegments.Count == 0) return null;

            PathFigure pathFigure = new()
            {
                StartPoint = new Point(BoundarySegments[0].GetEndPoint(0).X, BoundarySegments[0].GetEndPoint(0).Y),
                IsClosed = true
            };

            foreach (Curve segment in BoundarySegments)
            {
                Point endPoint = segment.GetEndPoint(1);

                if (segment is Line)
                {
                    pathFigure.Segments.Add(new LineSegment(new Point(endPoint.X, endPoint.Y), true));
                }
                else if (segment is Arc arc)
                {
                    Vector startVec = new(arc.Start.X - arc.Center.X, arc.Start.Y - arc.Center.Y);
                    Vector endVec = new(arc.End.X - arc.Center.X, arc.End.Y - arc.Center.Y);

                    double cross = startVec.X * endVec.Y - startVec.Y * endVec.X;
                    bool isClockwise = cross < 0;

                    double sweepAngle = Vector.AngleBetween(startVec, endVec);
                    if (sweepAngle < 0) sweepAngle += 360;
                    bool isLargeArc = sweepAngle > 180;

                    ArcSegment arcSegment = new(
                        new Point(arc.End.X, arc.End.Y),
                        new Size(arc.Radius, arc.Radius),
                        0,
                        isLargeArc,
                        isClockwise ? SweepDirection.Counterclockwise : SweepDirection.Clockwise,
                        true);

                    pathFigure.Segments.Add(arcSegment);
                }
            }

            return new PathGeometry([pathFigure]);
        }
    }
}
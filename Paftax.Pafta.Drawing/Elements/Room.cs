using Paftax.Pafta.Drawing.Geometries;
using Paftax.Pafta.Drawing.Structs;
using System.Windows;
using System.Windows.Media;

namespace Paftax.Pafta.Drawing.Elements
{
    public class Room : Element
    {
        public string Name { get; set; } = string.Empty;
        public string Number { get; set; } = string.Empty;
        public double Area { get; set; }
        public new Bounding Bounding { get; set; } = new(new XY(0, 0), new XY(0, 0));
        public List<Curve> BoundarySegments { get; } = [];
        public override void Draw(DrawingContext dc, Brush brush)
        {
            Pen pen = new(brush, 1) { LineJoin = PenLineJoin.Miter };

            if (BoundarySegments.Count == 0) return;

            var pf = new PathFigure
            {
                StartPoint = new Point(BoundarySegments[0].GetEndPoint(0).X, BoundarySegments[0].GetEndPoint(0).Y),
                IsClosed = true
            };

            foreach (var segment in BoundarySegments)
            {
                var end = segment.GetEndPoint(1);

                if (segment is Line)
                {
                    pf.Segments.Add(new LineSegment(new Point(end.X, end.Y), true));
                }
                else if (segment is Arc arc)
                {
                    var startVec = new Vector(arc.Start.X - arc.Center.X, arc.Start.Y - arc.Center.Y);
                    var endVec = new Vector(arc.End.X - arc.Center.X, arc.End.Y - arc.Center.Y);

                    double cross = startVec.X * endVec.Y - startVec.Y * endVec.X;
                    bool isClockwise = cross < 0;

                    double sweepAngle = Vector.AngleBetween(startVec, endVec);
                    if (sweepAngle < 0) sweepAngle += 360;
                    bool isLargeArc = sweepAngle > 180;

                    var arcSegment = new ArcSegment(
                        new Point(arc.End.X, arc.End.Y),
                        new Size(arc.Radius, arc.Radius),
                        0,
                        isLargeArc,
                        isClockwise ? SweepDirection.Counterclockwise : SweepDirection.Clockwise,
                        true
                    );

                    pf.Segments.Add(arcSegment);
                }
            }
            var pathGeometry = new PathGeometry([pf]);
            dc.DrawGeometry(null, pen, pathGeometry);
        }
    }
}

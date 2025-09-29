using Paftax.Pafta.Drawing.Elements;
using Paftax.Pafta.Drawing.Geometries.Primitives;
using System.Windows;
using System.Windows.Media;

namespace Paftax.Pafta.Drawing
{
    public partial class DrawingCanvas : FrameworkElement
    {
        private readonly VisualCollection _visuals;
        private Matrix _transformMatrix = Matrix.Identity;

        public DrawingCanvas()
        {
            _visuals = new VisualCollection(this);
            Background = Brushes.White;
        }

        public static readonly DependencyProperty BackgroundProperty =
            DependencyProperty.Register(nameof(Background), typeof(Brush), typeof(DrawingCanvas),
                new FrameworkPropertyMetadata(Brushes.White, FrameworkPropertyMetadataOptions.AffectsRender));

        public Brush Background
        {
            get => (Brush)GetValue(BackgroundProperty);
            set => SetValue(BackgroundProperty, value);
        }

        protected override int VisualChildrenCount => _visuals.Count;
        protected override Visual GetVisualChild(int index) => _visuals[index];
        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            UpdateTransformMatrix();

            drawingContext.DrawRectangle(Background, null, new Rect(0, 0, ActualWidth, ActualHeight));
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            UpdateTransformMatrix();
            ApplyMatrixToVisuals();
        }

        private void UpdateTransformMatrix()
        {
            var translate = new TranslateTransform(ActualWidth / 2, ActualHeight / 2).Value;
            var scale = new ScaleTransform(1, -1).Value;
            _transformMatrix = scale;
            _transformMatrix.Append(translate);
        }

        private void ApplyMatrixToVisuals()
        {
            var mt = new MatrixTransform(_transformMatrix);
            foreach (DrawingVisual v in _visuals.Cast<DrawingVisual>())
            {
                v.Transform = mt;
            }
        }


        private static System.Windows.Point TransformPoint(PointXY p)
        {
            return new(p.X, p.Y);
        }

        public void AddLine(PointXY start, PointXY end)
        {
            var visual = new DrawingVisual { Transform = new MatrixTransform(_transformMatrix) };
            using (DrawingContext dc = visual.RenderOpen())
            {
                Pen pen = new(Brushes.Blue, 2);
                dc.DrawLine(pen, TransformPoint(start), TransformPoint(end));
            }
            _visuals.Add(visual);
        }

        public void AddArc(PointXY start, PointXY end, PointXY center, double radius)
        {
            var visual = new DrawingVisual { Transform = new MatrixTransform(_transformMatrix) };
            using (DrawingContext dc = visual.RenderOpen())
            {
                Pen pen = new(Brushes.Red, 2);
                // Calculate angles
                Vector startVector = new(start.X - center.X, start.Y - center.Y);
                Vector endVector = new(end.X - center.X, end.Y - center.Y);
                double startAngle = Math.Atan2(startVector.Y, startVector.X) * (180 / Math.PI);
                double endAngle = Math.Atan2(endVector.Y, endVector.X) * (180 / Math.PI);
                // Ensure the arc is drawn in the correct direction
                bool isLargeArc = Math.Abs(endAngle - startAngle) > 180;
                SweepDirection sweepDirection = SweepDirection.Clockwise;
                if (Vector.CrossProduct(startVector, endVector) < 0)
                {
                    sweepDirection = SweepDirection.Counterclockwise;
                }
                ArcSegment arcSegment = new(
                    TransformPoint(end),
                    new Size(radius, radius),
                    0,
                    isLargeArc,
                    sweepDirection,
                    true);
                PathFigure pathFigure = new(TransformPoint(start), new List<ArcSegment> { arcSegment }, false);
                PathGeometry pathGeometry = new([pathFigure]);
                dc.DrawGeometry(null, pen, pathGeometry);
            }
            _visuals.Add(visual);
        }

        public void AddPolygon(List<PointXY> points)
        {
            if (points == null || points.Count < 3)
                return;

            // Remove last point if it's the same as the first
            if (points[0].Equals(points[^1]))
                points.RemoveAt(points.Count - 1);

            // Convert Structs.Point list to System.Windows.Point list
            List<System.Windows.Point> wpPoints = [.. points.Select(p => TransformPoint(p))];

            var visual = new DrawingVisual { Transform = new MatrixTransform(_transformMatrix) };
            using (DrawingContext dc = visual.RenderOpen())
            {
                Pen pen = new(Brushes.Green, 2);
                Brush brush = Brushes.LightGreen;

                StreamGeometry geometry = new();
                using (StreamGeometryContext ctx = geometry.Open())
                {
                    ctx.BeginFigure(wpPoints[0], true, true);
                    if (wpPoints.Count > 1)
                        ctx.PolyLineTo(wpPoints.GetRange(1, wpPoints.Count - 1), true, true);
                }

                geometry.Freeze();
                dc.DrawGeometry(brush, pen, geometry);
            }

            _visuals.Add(visual);
            InvalidateVisual();
        }

        public void ClearDrawings() => _visuals.Clear();

        public void ZoomToFit(List<Element> elements)
        {
            if (elements == null || elements.Count == 0)
                return;
            double minX = double.MaxValue;
            double minY = double.MaxValue;
            double maxX = double.MinValue;
            double maxY = double.MinValue;
            foreach (var element in elements)
            {
                var bbox = element.BoundingXY;
                if (bbox.Min.X < minX) minX = bbox.Min.X;
                if (bbox.Min.Y < minY) minY = bbox.Min.Y;
                if (bbox.Max.X > maxX) maxX = bbox.Max.X;
                if (bbox.Max.Y > maxY) maxY = bbox.Max.Y;
            }
            double elementWidth = maxX - minX;
            double elementHeight = maxY - minY;
            if (elementWidth <= 0 || elementHeight <= 0)
                return;
            double scaleX = ActualWidth / elementWidth;
            double scaleY = ActualHeight / elementHeight;
            double scale = Math.Min(scaleX, scaleY) * 0.9;
            var translate = new TranslateTransform(ActualWidth / 2, ActualHeight / 2).Value;
            var scaleTransform = new ScaleTransform(scale, -scale).Value;
            var moveToOrigin = new TranslateTransform(-(minX + elementWidth / 2), -(minY + elementHeight / 2)).Value;
            _transformMatrix = moveToOrigin;
            _transformMatrix.Append(scaleTransform);
            _transformMatrix.Append(translate);
            ApplyMatrixToVisuals();
        }
    }
}

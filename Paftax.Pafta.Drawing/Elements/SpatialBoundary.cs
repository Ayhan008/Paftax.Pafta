using Paftax.Pafta.Drawing.Elements.Abstracts;
using Paftax.Pafta.Drawing.Geometries;
using Paftax.Pafta.Drawing.Structs;
using Paftax.Pafta.Drawing.Utilities;
using Paftax.Pafta.Drawing.Visuals;
using Paftax.Pafta.Drawing.Visuals.Abstracts;
using System.Windows.Media;

namespace Paftax.Pafta.Drawing.Elements
{
    public class SpatialBoundary : DrawingElement
    {
        private readonly SpatialBoundaryVisual _visual;
        public override GeometryVisual Visual => _visual;

        private List<Curve2> _boundarySegments = [];
        public List<Curve2> BoundarySegments
        {
            get => _boundarySegments;
            set
            {
                _boundarySegments = value ?? [];
                _visual.BoundarySegments = _boundarySegments;
                UpdateBounds();
                InvalidateVisual();
            }
        }

        public SpatialBoundary()
        {
            _visual = new SpatialBoundaryVisual()
            {
                Stroke = Brushes.White,
                StrokeThickness = UnitConverter.MmToPoint(200),
                BoundarySegments = _boundarySegments,
            };
            IsAnnotation = false;
        }

        private void UpdateBounds()
        {
            if (_boundarySegments.Count == 0) return;

            double minX = double.PositiveInfinity, minY = double.PositiveInfinity;
            double maxX = double.NegativeInfinity, maxY = double.NegativeInfinity;

            foreach (var seg in _boundarySegments)
            {
                var p0 = seg.GetEndPoint(0);
                var p1 = seg.GetEndPoint(1);
                minX = Math.Min(minX, Math.Min(p0.X, p1.X));
                minY = Math.Min(minY, Math.Min(p0.Y, p1.Y));
                maxX = Math.Max(maxX, Math.Max(p0.X, p1.X));
                maxY = Math.Max(maxY, Math.Max(p0.Y, p1.Y));
            }

            BoundingXY = new(new Point2(minX, minY), new Point2(maxX, maxY));
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            _visual.Draw(drawingContext);
        }
    }
}
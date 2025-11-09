using Paftax.Pafta.Drawings.Visuals;
using Paftax.Pafta.Shared.Geometries;
using System.Windows.Media;

namespace Paftax.Pafta.Drawings.Elements
{
    public class SpatialBoundaryElement : DrawingElement
    {
        private readonly SpatialBoundaryVisual _visual;
        public override GeometryVisual Visual => _visual;

        private List<Curve> _boundarySegments = [];
        public List<Curve> BoundarySegments
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

        public SpatialBoundaryElement()
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

            Bounding = new(new Point2(minX, minY), new Point2(maxX, maxY));
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            _visual.Draw(drawingContext);
        }
    }
}
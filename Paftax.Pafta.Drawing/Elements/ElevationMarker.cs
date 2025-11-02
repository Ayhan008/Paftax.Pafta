using Paftax.Pafta.Drawing.Elements.Abstracts;
using Paftax.Pafta.Drawing.Structs;
using Paftax.Pafta.Drawing.Utilities;
using Paftax.Pafta.Drawing.Visuals;
using Paftax.Pafta.Drawing.Visuals.Abstracts;
using System.Windows.Input;
using System.Windows.Media;

namespace Paftax.Pafta.Drawing.Elements
{
    public class ElevationMarker : DrawingElement
    {
        private readonly ElevationMarkerVisual _visual;
        public override GeometryVisual Visual { get; }
        public ElevationMarker()
        {
            _visual = new ElevationMarkerVisual()
            {
                Angle = 0,
                TriangleVisible = [true, false, false, false],
                SheetNumber = "A101",
                DetailNumber = "1",
                Scale = Scale,
                Stroke = Brushes.Black,
                Center = Origin
            };

            BoundingXY = new(
                    new Point2(_visual.ContentBounds.Left, _visual.ContentBounds.Top),
                    new Point2(_visual.ContentBounds.Right, _visual.ContentBounds.Bottom));

            Visual = _visual;
            IsAnnotation = true;
            MouseLeftButtonDown += OnMouseLeftButtonDown;
        }

        private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;

            _visual.Stroke = IsSelected ? Brushes.DeepSkyBlue : Brushes.Black;
            InvalidateVisual();
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            _visual.Draw(drawingContext);
        }
    }
}
using Paftax.Pafta.Drawing.Test.Elements.Abstracts;
using Paftax.Pafta.Drawing.Test.Visuals;
using Paftax.Pafta.Drawing.Test.Visuals.Abstracts;
using System.Windows.Input;
using System.Windows.Media;

namespace Paftax.Pafta.Drawing.Test.Elements
{
    public class ElevationMarkerElement : DrawingElement
    {
        public string SheetNumber { get; set; } = "A101";
        public string DetailNumber { get; set; } = "1";

        private bool _isHovered = false;
        public ElevationMarkerElement()
        {
            ElevationTriangleVisual elevationTriangleVisual = new()
            {
                Center = Center,
                Scale = Scale,
            };
            Visuals.Add(elevationTriangleVisual);

            CalloutHeadVisual calloutHeadVisual = new()
            {
                Center = Center,
                SheetNumber = SheetNumber,
                DetailNumber = DetailNumber,
                Scale = Scale,
            };
            Visuals.Add(calloutHeadVisual);
        }
        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            foreach (var visual in Visuals)
            {
                if (visual is CalloutHeadVisual gv)
                {
                    if (_isHovered)
                        gv.Stroke = Brushes.Blue;
                    else
                        gv.Stroke = Brushes.Black;
                }

                visual.Draw(drawingContext);
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            var pos = e.GetPosition(this);
            var callout = Visuals.OfType<CalloutHeadVisual>().FirstOrDefault();

            bool hovered = callout != null && callout.Geometry.FillContains(pos);

            if (hovered != _isHovered)
            {
                _isHovered = hovered;
                InvalidateVisual();
            }
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);

            if (_isHovered)
            {
                _isHovered = false;
                InvalidateVisual();
            }
        }
    }
}
using Paftax.Pafta.Drawing.Test.Visuals.Abstracts;
using System.Windows;
using System.Windows.Media;

namespace Paftax.Pafta.Drawing.Test.Elements.Abstracts
{
    public abstract class DrawingElement : UIElement
    {
        public Point Center { get; set; } = new(0, 0);
        public double Scale { get; set; } = 1.0;
        public bool IsDraggable { get; set; } = false;
        public bool IsScaleable { get; set; } = false;
        public List<GeometryVisual> Visuals { get; }
        public DrawingElement()
        {
            Visuals = [];
        }
        protected override int VisualChildrenCount => Visuals.Count;
        protected override Visual GetVisualChild(int index) => Visuals[index];

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            foreach (var visual in Visuals)
            {
                visual.Draw(drawingContext);
            }
        }
    }
}

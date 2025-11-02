using Paftax.Pafta.Drawing.Structs;
using Paftax.Pafta.Drawing.Visuals.Abstracts;
using System.Windows;
using System.Windows.Media;

namespace Paftax.Pafta.Drawing.Elements.Abstracts
{
    public abstract class DrawingElement : UIElement
    {
        public Bounding2 BoundingXY { get; set; }
        public Point Origin { get; set; }
        public bool IsSelected { get; set; }
        public bool IsAnnotation { get; set; }
        public double Scale { get; set; }
        public abstract GeometryVisual Visual { get; }
        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            Visual?.Draw(drawingContext);
        }
    }
}

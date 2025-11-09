using Paftax.Pafta.Drawings.Visuals;
using Paftax.Pafta.Shared.Geometries;
using System.Windows;
using System.Windows.Media;

namespace Paftax.Pafta.Drawings.Elements
{
    public abstract class DrawingElement : UIElement
    {
        public Bounding2 Bounding { get; set; }
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

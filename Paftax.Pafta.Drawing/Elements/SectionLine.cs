using Paftax.Pafta.Drawings.Visuals;
using Paftax.Pafta.Shared.Geometries;
using Paftax.Pafta.Shared.Models.Element;
using System.Windows.Input;
using System.Windows.Media;

namespace Paftax.Pafta.Drawings.Elements
{
    public class SectionLine : DrawingElement
    {
        public Point2 Start { get; set; }
        public Point2 End { get; set; }
        public Bounding3 ViewBox { get; set; }
        public string SheetNumber { get; set; } = "A101";
        public string DetailNumber { get; set; } = "2";


        private readonly SectionLineVisual _visual;
        public override GeometryVisual Visual { get; }

        public SectionLine()
        {
            _visual = new SectionLineVisual()
            {
                Stroke = Brushes.Black,
                Scale = Scale,
            };

            Visual = _visual;
            IsAnnotation = true;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            _visual.Draw(drawingContext);
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            using var dc = _visual.RenderOpen();

        }

        public override void Create(ElementModel elementModel)
        {
            throw new NotImplementedException();
        }
    }
}
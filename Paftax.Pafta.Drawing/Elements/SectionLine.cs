// Paftax.Pafta.Drawing\Elements\SectionLine.cs
using Paftax.Pafta.Drawing.Elements.Abstracts;
using Paftax.Pafta.Drawing.Structs;
using Paftax.Pafta.Drawing.Utilities;
using Paftax.Pafta.Drawing.Visuals;
using Paftax.Pafta.Drawing.Visuals.Abstracts;
using System.Windows.Media;

namespace Paftax.Pafta.Drawing.Elements
{
    public class SectionLine : DrawingElement
    {
        private Point2 _start;
        private Point2 _end;

        public Point2 Start
        {
            get => _start;
            set
            {
                _start = value;
                if (_visual != null) _visual.StartPoint = _start;
                UpdateState();
            }
        }

        public Point2 End
        {
            get => _end;
            set
            {
                _end = value;
                if (_visual != null) _visual.EndPoint = _end;
                UpdateState();
            }
        }

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

            // initialize with defaults (in case properties are not set by caller)
            UpdateState();
        }

        private void UpdateState()
        {
            _visual.StartPoint = _start;
            _visual.EndPoint = _end;

            var minX = Math.Min(_start.X, _end.X);
            var minY = Math.Min(_start.Y, _end.Y);
            var maxX = Math.Max(_start.X, _end.X);
            var maxY = Math.Max(_start.Y, _end.Y);

            BoundingXY = new Bounding2(new Point2(minX, minY), new Point2(maxX, maxY));
            Origin = new Point2((_start.X + _end.X) / 2, (_start.Y + _end.Y) / 2);

            InvalidateVisual();
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            _visual.Draw(drawingContext);
        }
    }
}
using Paftax.Pafta.Drawings;
using Paftax.Pafta.Drawings.Elements;
using Paftax.Pafta.Shared.Geometries;
using System.Windows.Controls;

namespace Paftax.Pafta.UI.Views
{
    public partial class GraphicDesignerUserControl : UserControl
    {
        public GraphicDesignerUserControl()
        {
            InitializeComponent();
            DataContext = new ViewModels.GraphicDesignerViewModel();
            Canvas.AddElement(new ElevationMarker());

            double half = UnitConverter.MToPoint(5);

            SpatialBoundaryElement spatialBoundary = new()
            {
                BoundarySegments =
                [
                    new Line(
                    new Point2(-half, -half),
                    new Point2(half, -half)
                ),
                new Line(
                    new Point2(half, -half),
                    new Point2(half, half)
                ),
                new Line(
                    new Point2(half, half),
                    new Point2(-half, half)
                ),
                new Line(
                    new Point2(-half, half),
                    new Point2(-half, -half)
                )
                ],
            };
            Canvas.AddElement(spatialBoundary);

            SectionLine sectionLine = new()
            {
                Start = new Point2(0, half + 700),
                End = new Point2(0, -half - 700),
            };
            Canvas.AddElement(sectionLine);

            SectionLine sectionLine2 = new()
            {
                Start = new Point2(-half - 700, 0),
                End = new Point2(half + 700, 0),
            };
            Canvas.AddElement(sectionLine2);
        }
    }
}
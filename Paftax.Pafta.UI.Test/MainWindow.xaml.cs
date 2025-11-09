using Paftax.Pafta.Drawings;
using Paftax.Pafta.Drawings.Elements;
using Paftax.Pafta.UI.Test.Visuals;
using Paftax.Pafta.UI.ViewModels;
using System.Windows;
using System.Windows.Media;

namespace Paftax.Pafta.UI.Test
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new GraphicDesignerViewModel();
            Gc.AddElement(new ElevationMarker());

            double half = UnitConverter.MToPoint(5);

            SpatialBoundaryElement spatialBoundary = new()
            {
                BoundarySegments =
                [
                    new Paftax.Pafta.Drawing.Geometries.Line(
                    new Paftax.Pafta.Drawing.Structs.Point2(-half, -half),
                    new Paftax.Pafta.Drawing.Structs.Point2(half, -half)
                ),
                new Paftax.Pafta.Drawing.Geometries.Line(
                    new Paftax.Pafta.Drawing.Structs.Point2(half, -half),
                    new Paftax.Pafta.Drawing.Structs.Point2(half, half)
                ),
                new Paftax.Pafta.Drawing.Geometries.Line(
                    new Paftax.Pafta.Drawing.Structs.Point2(half, half),
                    new Paftax.Pafta.Drawing.Structs.Point2(-half, half)
                ),
                new Paftax.Pafta.Drawing.Geometries.Line(
                    new Paftax.Pafta.Drawing.Structs.Point2(-half, half),
                    new Paftax.Pafta.Drawing.Structs.Point2(-half, -half)
                )
                ],
            };
            Gc.AddElement(spatialBoundary);

            SectionLine sectionLine = new()
            {
                Start = new Paftax.Pafta.Drawing.Structs.Point2(0, half + 700),
                End = new Paftax.Pafta.Drawing.Structs.Point2(0, -half - 700),
            };
            Gc.AddElement(sectionLine);

            SectionLine sectionLine2 = new()
            {
                Start = new Paftax.Pafta.Drawing.Structs.Point2(-half - 700, 0),
                End = new Paftax.Pafta.Drawing.Structs.Point2(half + 700, 0),
            };
            Gc.AddElement(sectionLine2);
        }
    }
}

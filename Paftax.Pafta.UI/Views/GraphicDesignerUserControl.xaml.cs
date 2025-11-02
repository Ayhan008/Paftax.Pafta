using Paftax.Pafta.Drawing.Elements;
using Paftax.Pafta.Drawing.Utilities;
using Paftax.Pafta.Shared.Utilities;
using System.Windows;
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

            double half = UnitConverter.MToPoint(5); // 10 metre yarıçap

            SpatialBoundary spatialBoundary = new()
            {
                BoundarySegments =
                [
                    new Paftax.Pafta.Drawing.Geometries.Line2(
                    new Paftax.Pafta.Drawing.Structs.Point2(-half, -half),
                    new Paftax.Pafta.Drawing.Structs.Point2(half, -half)
                ),
                new Paftax.Pafta.Drawing.Geometries.Line2(
                    new Paftax.Pafta.Drawing.Structs.Point2(half, -half),
                    new Paftax.Pafta.Drawing.Structs.Point2(half, half)
                ),
                new Paftax.Pafta.Drawing.Geometries.Line2(
                    new Paftax.Pafta.Drawing.Structs.Point2(half, half),
                    new Paftax.Pafta.Drawing.Structs.Point2(-half, half)
                ),
                new Paftax.Pafta.Drawing.Geometries.Line2(
                    new Paftax.Pafta.Drawing.Structs.Point2(-half, half),
                    new Paftax.Pafta.Drawing.Structs.Point2(-half, -half)
                )
                ],
            };
            Canvas.AddElement(spatialBoundary);

            SectionLine sectionLine = new()
            {
                Start = new Paftax.Pafta.Drawing.Structs.Point2(0, half + 700),
                End = new Paftax.Pafta.Drawing.Structs.Point2(0, -half - 700),
            };
            Canvas.AddElement(sectionLine);

            SectionLine sectionLine2 = new()
            {
                Start = new Paftax.Pafta.Drawing.Structs.Point2(-half - 700, 0),
                End = new Paftax.Pafta.Drawing.Structs.Point2(half + 700, 0),
            };
            Canvas.AddElement(sectionLine2);

            Loaded += OnLoaded;
            Unloaded += OnUnloaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Canvas.MouseMovedInContent += OnCanvasMouseMoved;
        }

        private void OnUnloaded(object? sender, RoutedEventArgs e)
        {
            Canvas.MouseMovedInContent -= OnCanvasMouseMoved;
        }

        private void OnCanvasMouseMoved(Point screenOnContent, Point model)
        {
            if (DataContext is ViewModels.GraphicDesignerViewModel vm)
                vm.MouseCoordinates = $"X: {model.X:F2}, Y: {model.Y:F2}";
        }
    }
}
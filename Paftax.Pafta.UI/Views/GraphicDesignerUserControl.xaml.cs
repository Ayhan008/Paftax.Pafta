using DocumentFormat.OpenXml.Wordprocessing;
using Paftax.Pafta.Drawing.Elements;
using Paftax.Pafta.Drawing.Geometries.Primitives;
using Paftax.Pafta.Shared.Models;
using System.Windows.Controls;

namespace Paftax.Pafta.UI.Views
{
    public partial class GraphicDesignerUserControl : UserControl
    {
        public GraphicDesignerUserControl()
        {
            InitializeComponent();

            Loaded += (s, e) =>
            {
                if (DataContext is ViewModels.GraphicDesignerViewModel vm)
                {
                    List<Element> elementsToFit = [];

                    foreach (RoomModel room in vm.Rooms)
                    {
                        List<Curve> curves = room.RoomGeometry.BoundarySegments;
                        elementsToFit.Add(room.RoomGeometry);

                        foreach (Curve curve in curves)
                        {
                            if (curve is Line line)
                            {
                                DrawingCanvas.AddLine(line.Start, line.End);
                            }
                            else if (curve is Arc arc)
                            {
                                DrawingCanvas.AddArc(arc.Start, arc.End, arc.Center, arc.Radius);
                            }
                        }        
                    }
                    DrawingCanvas.ZoomToFit(elementsToFit);
                }
            };
        }
    }
}
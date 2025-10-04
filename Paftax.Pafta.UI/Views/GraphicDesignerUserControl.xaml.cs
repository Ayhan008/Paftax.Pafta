using Paftax.Pafta.Drawing.Structs;
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
                    foreach (RoomModel room in vm.Rooms)
                    {
                        DrawingCanvas.AddElement(room.RoomGeometry);
                        DrawingCanvas.AddElevation(new XY(0, 0));
                    }
                    DrawingCanvas.ZoomToFitAll();
                }
            };
        }
    }
}
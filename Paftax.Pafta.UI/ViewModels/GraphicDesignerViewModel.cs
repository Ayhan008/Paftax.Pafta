using CommunityToolkit.Mvvm.ComponentModel;
using Paftax.Pafta.Drawing.Geometries;
using Paftax.Pafta.Shared.Models;
using System.Collections.ObjectModel;

namespace Paftax.Pafta.UI.ViewModels
{
    public partial class GraphicDesignerViewModel : ObservableObject
    {
        public ObservableCollection<RoomModel> Rooms { get; } = [];

        [ObservableProperty]
        private List<Curve> curves = [];

        [ObservableProperty]
        private double scale = 0.05;

        public void LoadData(List<RoomModel> rooms)
        {
            Rooms.Clear();
            foreach (var room in rooms)
            {
                Rooms.Add(room);
                Curves.AddRange(room.RoomGeometry.BoundarySegments);
            }
        }
    }
}
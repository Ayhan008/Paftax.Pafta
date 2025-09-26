using CommunityToolkit.Mvvm.ComponentModel;
using Paftax.Pafta.Shared.Models;
using System.Collections.ObjectModel;

namespace Paftax.Pafta.UI.ViewModels
{
    public partial class GraphicDesignerViewModel : ObservableObject
    {
        public ObservableCollection<RichRoomDataModel> Rooms { get; } = [];

        [ObservableProperty]
        private int canvasWidth;

        [ObservableProperty]
        private int canvasHeight;

        public void LoadData(List<RichRoomDataModel> rooms)
        {
            Rooms.Clear();
            foreach (var room in rooms)
                Rooms.Add(room);

            UpdateCanvasFromRooms();
        }

        private void UpdateCanvasFromRooms()
        {
            if (Rooms.Count == 0)
            {
                CanvasWidth = 800;
                CanvasHeight = 450;
                return;
            }

            float minX = Rooms.Min(r => r.BoundingBoxData.Min.X);
            float minY = Rooms.Min(r => r.BoundingBoxData.Min.Y);
            float maxX = Rooms.Max(r => r.BoundingBoxData.Max.X);
            float maxY = Rooms.Max(r => r.BoundingBoxData.Max.Y);

            var width = Math.Max(1.0, maxX - minX);
            var height = Math.Max(1.0, maxY - minY);

            CanvasWidth = (int)Math.Ceiling(width);
            CanvasHeight = (int)Math.Ceiling(height);
        }
    }
}
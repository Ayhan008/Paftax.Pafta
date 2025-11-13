using CommunityToolkit.Mvvm.ComponentModel;
using Paftax.Pafta.Shared.Enums;
using Paftax.Pafta.Shared.Interfaces;
using Paftax.Pafta.Shared.Models;
using System.Collections.ObjectModel;

namespace Paftax.Pafta.UI.ViewModels
{
    public partial class RoomToSheetViewModel : ObservableObject
    {
        public ObservableCollection<RoomModel> Rooms { get; } = [];
        public List<ViewType> ViewTypes { get; } =
        [
            ViewType.FloorPlan,
            ViewType.CeilingPlan,
            ViewType.Elevation,
            ViewType.Section,
            ViewType.ThreeD,
            ViewType.Schedule,
            ViewType.Legend
        ];

        [ObservableProperty] private string? _selectedViewType;
        public GraphicDesignerViewModel GraphicDesignerViewModel { get; set; } = new();

        public void LoadRoomModels(List<RoomModel> roomModels)
        {
            Rooms.Clear();
            foreach (var roomModel in roomModels)
            {
                Rooms.Add(roomModel);
                
                // Add room geometry to the graphic designer
                var spatialBoundaryElement = GraphicDesignerViewModel.CreateSpatialBoundaryFromRoomModel(roomModel);
                GraphicDesignerViewModel.Elements.Add(spatialBoundaryElement);
            }
        }
    }
}

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
        public ObservableCollection<CreateViewModel> CreatedViews { get; } = [];
        public List<RevitViewType> ViewTypes { get; } =
        [
            RevitViewType.FloorPlan,
            RevitViewType.CeilingPlan,
            RevitViewType.Elevation,
            RevitViewType.Section,
            RevitViewType.ThreeD,
            RevitViewType.Schedule,
            RevitViewType.Legend
        ];

        [ObservableProperty] private string? _selectedViewType;
        public GraphicDesignerViewModel GraphicDesignerViewModel { get; set; } = new();

        public void LoadRoomModels(List<RoomModel> roomModels)
        {
            Rooms.Clear();
            foreach (var roomModel in roomModels)
            {
                Rooms.Add(roomModel);

                if (roomModel.Boundary != null)
                    GraphicDesignerViewModel.AddDrawingElement(roomModel.Boundary);
            }
        }
    }
}

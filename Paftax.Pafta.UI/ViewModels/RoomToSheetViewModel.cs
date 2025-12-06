using CommunityToolkit.Mvvm.ComponentModel;
using Paftax.Pafta.Drawings.Elements;
using Paftax.Pafta.Shared.Enums;
using Paftax.Pafta.Shared.Models.Element;
using System.Collections.ObjectModel;

namespace Paftax.Pafta.UI.ViewModels
{
    public partial class RoomToSheetViewModel : ObservableObject
    {
        public GraphicDesignerViewModel GraphicDesignerViewModel { get; set; } = new();
        public ObservableCollection<RoomModel> Rooms { get; } = [];

        public RoomToSheetViewModel() { }

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

        public void LoadRoomModels(List<RoomModel> roomModels)
        {
            Rooms.Clear();
            GraphicDesignerViewModel.Elements.Clear();

            foreach (var roomModel in roomModels)
            {
                Rooms.Add(roomModel);

                var spatialElement = new SpatialElement();
                spatialElement.Create(roomModel);

                GraphicDesignerViewModel.Elements.Add(spatialElement);
            }
        }
    }
}
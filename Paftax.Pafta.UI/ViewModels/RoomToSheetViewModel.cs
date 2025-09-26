using CommunityToolkit.Mvvm.ComponentModel;
using Paftax.Pafta.Shared.Models;
using System.Collections.ObjectModel;

namespace Paftax.Pafta.UI.ViewModels
{
    public partial class RoomToSheetViewModel : ObservableObject
    {
        public ObservableCollection<RichRoomDataModel> Rooms { get; } = [];
        public GraphicDesignerViewModel GraphicDesignerViewModel { get; set; } = new();

        public void LoadData(List<RichRoomDataModel> rooms)
        {
            Rooms.Clear();
            foreach (var room in rooms)
            {
                Rooms.Add(room);
                GraphicDesignerViewModel.LoadData(rooms);
            }
        }
    }
}

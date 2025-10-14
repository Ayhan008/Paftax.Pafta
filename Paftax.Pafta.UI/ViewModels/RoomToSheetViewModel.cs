using CommunityToolkit.Mvvm.ComponentModel;
using Paftax.Pafta.Shared.Interfaces;
using Paftax.Pafta.Shared.Models;
using System.Collections.ObjectModel;

namespace Paftax.Pafta.UI.ViewModels
{
    public partial class RoomToSheetViewModel : ObservableObject, ICloseable
    {
        public ObservableCollection<RoomModel> Rooms { get; } = [];
        public GraphicDesignerViewModel GraphicDesignerViewModel { get; set; } = new();

        public event Action? CloseAction;

        public void LoadData(List<RoomModel> rooms)
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

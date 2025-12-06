using CommunityToolkit.Mvvm.ComponentModel;
using Paftax.Pafta.Drawings.Elements;
using Paftax.Pafta.Shared.Models;
using System.Collections.ObjectModel;

namespace Paftax.Pafta.UI.ViewModels
{
    public partial class GraphicDesignerViewModel : ObservableObject
    {
        public ObservableCollection<DrawingElement> Elements { get; } = [];
        public ObservableCollection<ScaleItem> Scales { get; set; } =
        [
            new ScaleItem { Name = "1 : 10", Scale = 0.1 },
            new ScaleItem { Name = "1 : 20", Scale = 0.05 },
            new ScaleItem { Name = "1 : 50", Scale = 0.02 },
            new ScaleItem { Name = "1 : 100", Scale = 0.01 },
            new ScaleItem { Name = "1 : 200", Scale = 0.005 },
            new ScaleItem { Name = "1 : 500", Scale = 0.002 },
            new ScaleItem { Name = "1 : 1000", Scale = 0.001 },
            new ScaleItem { Name = "1 : 2000", Scale = 0.0005 },
            new ScaleItem { Name = "1 : 5000", Scale = 0.0002 },
        ];

        [ObservableProperty] private int _selectedScaleIndex = 2;
        [ObservableProperty] private double _canvasScale = 1.0;
        [ObservableProperty] private string _mouseCoordinates = string.Empty;

        public GraphicDesignerViewModel()
        {
            CanvasScale = 1 / Scales[SelectedScaleIndex].Scale;
        }
        partial void OnSelectedScaleIndexChanged(int value)
        {
            if (value >= 0 && value < Scales.Count)
            {
                CanvasScale = 1 / Scales[value].Scale;
            }
        }
    }
}
using CommunityToolkit.Mvvm.ComponentModel;
using Paftax.Pafta.Drawings;
using Paftax.Pafta.Drawings.Elements;
using Paftax.Pafta.Shared.Geometries;
using Paftax.Pafta.Shared.Models;
using System.Collections.ObjectModel;

namespace Paftax.Pafta.UI.ViewModels
{
    public partial class GraphicDesignerViewModel : ObservableObject
    {
        public ObservableCollection<DrawingElement> Elements { get; } = [];
        public ObservableCollection<ScaleModel> Scales { get; set; } =
        [
            new ScaleModel { Name = "1 : 10", Scale = 0.1 },
            new ScaleModel { Name = "1 : 20", Scale = 0.05 },
            new ScaleModel { Name = "1 : 50", Scale = 0.02 },
            new ScaleModel { Name = "1 : 100", Scale = 0.01 },
            new ScaleModel { Name = "1 : 200", Scale = 0.005 },
            new ScaleModel { Name = "1 : 500", Scale = 0.002 },
            new ScaleModel { Name = "1 : 1000", Scale = 0.001 },
            new ScaleModel { Name = "1 : 2000", Scale = 0.0005 },
            new ScaleModel { Name = "1 : 5000", Scale = 0.0002 },
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
                CanvasScale = 1/ Scales[value].Scale;
            }
        }
        public void AddDrawingElement(DrawingElement element)
        {
            Elements.Add(element);
            Elements.Add(DrawingElement());
        }   

        public static DrawingElement DrawingElement()
        {
            return new SpatialBoundaryElement 
            {
                BoundarySegments =
                [ 
                    new Line (new Point2(0, 0), new Point2(100, 0)),
                    new Line (new Point2(100, 0), new Point2(100, 100)),
                    new Line (new Point2(100, 100), new Point2(0, 100)),
                    new Line (new Point2(0, 100), new Point2(0, 0))
                ],               
            };
        }

        public static SpatialBoundaryElement CreateSpatialBoundaryFromRoomModel(RoomModel roomModel)
        {
            List<Curve> boundarySegments = [];

            foreach (CurveModel curveModel in roomModel.Boundaries)
            {
                if (curveModel is LineModel lineModel)
                {
                    boundarySegments.Add(new Line(lineModel.Start, lineModel.End));
                }
                else if (curveModel is ArcModel arcModel)
                {
                    boundarySegments.Add(new Arc(arcModel.Start, arcModel.End, arcModel.Center, arcModel.Radius));
                }
            }

            return new SpatialBoundaryElement
            {
                BoundarySegments = boundarySegments
            };
        }
    }
}
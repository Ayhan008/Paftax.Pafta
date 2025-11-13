using Paftax.Pafta.Drawings;
using Paftax.Pafta.Drawings.Elements;
using Paftax.Pafta.Shared.Geometries;
using Paftax.Pafta.UI.ViewModels;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;

namespace Paftax.Pafta.UI.Views
{
    public partial class GraphicDesignerUserControl : UserControl
    {
        public GraphicDesignerUserControl()
        {
            InitializeComponent();
            
            // If no DataContext is set from parent, create a default one with test data
            if (DataContext == null)
            {
                DataContext = new GraphicDesignerViewModel();
                Canvas.AddElement(new ElevationMarker());

                double half = UnitConverter.MToPoint(5);

                SpatialBoundaryElement spatialBoundary = new()
                {
                    BoundarySegments =
                    [
                        new Line(
                        new Point2(-half, -half),
                        new Point2(half, -half)
                    ),
                    new Line(
                        new Point2(half, -half),
                        new Point2(half, half)
                    ),
                    new Line(
                        new Point2(half, half),
                        new Point2(-half, half)
                    ),
                    new Line(
                        new Point2(-half, half),
                        new Point2(-half, -half)
                    )
                    ],
                };
                Canvas.AddElement(spatialBoundary);

                SectionLine sectionLine = new()
                {
                    Start = new Point2(0, half + 700),
                    End = new Point2(0, -half - 700),
                };
                Canvas.AddElement(sectionLine);

                SectionLine sectionLine2 = new()
                {
                    Start = new Point2(-half - 700, 0),
                    End = new Point2(half + 700, 0),
                };
                Canvas.AddElement(sectionLine2);
            }

            Loaded += GraphicDesignerUserControl_Loaded;
            DataContextChanged += GraphicDesignerUserControl_DataContextChanged;
        }

        private void GraphicDesignerUserControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue is GraphicDesignerViewModel oldViewModel)
            {
                oldViewModel.Elements.CollectionChanged -= Elements_CollectionChanged;
            }

            if (e.NewValue is GraphicDesignerViewModel newViewModel)
            {
                newViewModel.Elements.CollectionChanged += Elements_CollectionChanged;
                SyncElementsToCanvas(newViewModel);
            }
        }

        private void GraphicDesignerUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is GraphicDesignerViewModel viewModel)
            {
                viewModel.Elements.CollectionChanged += Elements_CollectionChanged;
                SyncElementsToCanvas(viewModel);
            }
        }

        private void Elements_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add && e.NewItems != null)
            {
                foreach (DrawingElement element in e.NewItems)
                {
                    Canvas.AddElement(element);
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove && e.OldItems != null)
            {
                foreach (DrawingElement element in e.OldItems)
                {
                    Canvas.RemoveElement(element);
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                // Clear canvas when collection is reset
                Canvas.Children.Clear();
            }
        }

        private void SyncElementsToCanvas(GraphicDesignerViewModel viewModel)
        {
            // Add all existing elements to canvas
            foreach (var element in viewModel.Elements)
            {
                Canvas.AddElement(element);
            }
        }
    }
}
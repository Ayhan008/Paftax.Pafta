using System.Collections.Specialized;
using System.Windows.Controls;
using Paftax.Pafta.Drawings.Elements;
using Paftax.Pafta.UI.ViewModels;

namespace Paftax.Pafta.UI.Views
{
    public partial class GraphicDesignerUserControl : UserControl
    {
        public GraphicDesignerUserControl()
        {
            InitializeComponent();
            Loaded += (_, _) => AttachToViewModel();
            DataContextChanged += (_, _) => AttachToViewModel();
        }

        private void AttachToViewModel()
        {
            if (DataContext is not GraphicDesignerViewModel vm) return;

            vm.Elements.CollectionChanged -= OnElementsChanged;

            Canvas.Children.Clear();
            foreach (var el in vm.Elements)
                Canvas.AddElement(el);

            vm.Elements.CollectionChanged += OnElementsChanged;
        }

        private void OnElementsChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (DataContext is GraphicDesignerViewModel)
            {
                if (e.NewItems != null)
                    foreach (DrawingElement el in e.NewItems)
                        Canvas.AddElement(el);

                if (e.OldItems != null)
                    foreach (DrawingElement el in e.OldItems)
                        Canvas.RemoveElement(el);
            }
        }
    }
}
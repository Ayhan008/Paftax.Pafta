using Paftax.Pafta.UI.Test.FrameworkElements;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Paftax.Pafta.UI.Test
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DrawingCanvas.AddPolygon(
            [
                new Point(50, 50),
                new Point(150, 50),
                new Point(100, 150)
            ]);
        }
    }
}

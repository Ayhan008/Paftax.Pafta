using System.Windows.Media;

namespace Paftax.Pafta.UI.Test.Visuals
{
    abstract class GeometryVisual : DrawingVisual
    {
        public Matrix WorldMatrix { get; set; } = Matrix.Identity;

        public abstract void DrawGeometry(DrawingContext dc);
        public void Draw()
        {
            using DrawingContext dc = RenderOpen();

            dc.PushTransform(new MatrixTransform(WorldMatrix));

            DrawGeometry(dc);

            dc.Pop();
        }
    }
}

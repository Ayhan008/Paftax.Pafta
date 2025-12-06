using System.Windows.Media;

namespace Paftax.Pafta.Drawings.Visuals
{
    public class SpatialElementVisual : GeometryVisual
    {
        public Geometry? SourceGeometry { get; set; }

        protected override Geometry CreateGeometry()
        {
            return SourceGeometry ?? Geometry.Empty;
        }
    }
}
using Autodesk.Revit.DB;
using Paftax.Pafta.Shared.Models;
using Paftax.Pafta.Shared.Models.Geometry;

namespace Paftax.Pafta.Revit2026.Factories
{
    internal static class BoundarySegmentModelFactory
    {
        public static Shared.Models.BoundarySegment Create(Autodesk.Revit.DB.BoundarySegment boundarySegment)
        {
            var curves = new List<CurveModel>();

            if (boundarySegment != null)
            {
                Curve curve = boundarySegment.GetCurve();

                if (curve != null)
                {
                    CurveModel curveModel = CurveModelFactory.Create(curve);
                    if (curveModel != null)
                        curves.Add(curveModel);
                }
            }
            return new Shared.Models.BoundarySegment { Curves = curves };
        }
    }
}

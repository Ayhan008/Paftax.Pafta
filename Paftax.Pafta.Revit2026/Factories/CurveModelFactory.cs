using Autodesk.Revit.DB;
using Paftax.Pafta.Shared.Models;
using Paftax.Pafta.Shared.Models.Geometry;
using System;

namespace Paftax.Pafta.Revit2026.Factories
{
    internal static class CurveModelFactory
    {
        public static CurveModel Create(Curve curve)
        {
            return curve switch
            {
                Line line => new LineModel
                {
                    Length = line.Length,
                    Start = XYZModelFactory.Create(line.GetEndPoint(0)),
                    End = XYZModelFactory.Create(line.GetEndPoint(1))
                },
                Arc arc => new ArcModel
                {
                    Length = arc.Length,
                    Start = XYZModelFactory.Create(arc.GetEndPoint(0)),
                    End = XYZModelFactory.Create(arc.GetEndPoint(1)),
                    Radius = arc.Radius,
                    Center = XYZModelFactory.Create(arc.Center),
                    Normal = XYZModelFactory.Create(arc.Normal),
                    IsClockwise = IsClockwise(arc)
                },
                _ => throw new NotSupportedException(),
            };
        }

        private static bool IsClockwise(Arc arc)
        {
            if (arc.Normal.Z > 0)
            {
                return true;
            }
            return false;
        }
    }
}
           

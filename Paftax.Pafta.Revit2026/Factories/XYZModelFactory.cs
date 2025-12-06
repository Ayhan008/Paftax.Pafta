using Autodesk.Revit.DB;
using Paftax.Pafta.Shared.Models.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paftax.Pafta.Revit2026.Factories
{
    internal static class XYZModelFactory
    {
        public static XYZModel Create(XYZ xyz)
        {
            if (xyz == null)
                return new XYZModel();

            return new XYZModel
            {
                X = xyz.X,
                Y = xyz.Y,
                Z = xyz.Z
            };
        }
    }
}

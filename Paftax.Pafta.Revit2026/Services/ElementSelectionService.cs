using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Paftax.Pafta.Revit2026.Services
{
    internal class ElementSelectionService(Document document)
    {
        private enum SelectionType
        {
            ElementInRevitLink,
            ElementInActiveDocument,
            ElementInDocumentAndRevitLink
        }

        private class SelectionFilter(Document document, List<RevitLinkInstance> revitLinkInstances) : ISelectionFilter
        {
            public bool AllowElement(Element elem)
            {
                foreach (RevitLinkInstance linkInstance in revitLinkInstances)
                {
                    if (elem.Id.Value == linkInstance.Id.Value)
                    {
                        return true;
                    }
                }
                return false;
            }
            public bool AllowReference(Reference reference, XYZ position)
            {
                Element linkElement = document.GetElement(reference);
                if (linkElement is RevitLinkInstance linkInstance)
                {
                    Element element = linkInstance.GetLinkDocument().GetElement(reference.LinkedElementId);

                    return true;
                }
                return false;
            }
        }
    }
}

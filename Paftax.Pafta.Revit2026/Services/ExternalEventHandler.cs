using Autodesk.Revit.UI;

namespace Paftax.Pafta.Revit2026.Services
{
    internal class ExternalEventHandler : IExternalEventHandler
    {
        public Action<UIApplication>? ExecuteAction { get; set; }
        public void Execute(UIApplication app)
        {
            ExecuteAction?.Invoke(app);
        }

        public string GetName()
        {
            return "External Event Handler";
        }
    }
}

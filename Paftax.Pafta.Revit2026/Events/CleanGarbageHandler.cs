using Autodesk.Revit.UI;

namespace Paftax.Pafta.Revit2026.Events
{
    internal class CleanGarbageHandler : IExternalEventHandler
    {
        private Action<UIApplication>? _action;
        public void Execute(UIApplication app)
        {
            _action?.Invoke(app);
        }
        public string GetName()
        {
            return nameof(CleanGarbageHandler);
        }
        public void SetAction(Action<UIApplication> action)
        {
            _action = action;
        }
    }
}

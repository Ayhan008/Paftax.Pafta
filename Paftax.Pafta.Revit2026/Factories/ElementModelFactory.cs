using Autodesk.Revit.DB;
using Paftax.Pafta.Shared.Models;

namespace Paftax.Pafta.Revit2026.Factories
{
    internal class ElementModelFactory(Document document, bool isCancellable = false)
    {
        private readonly Document _document = document;
        private readonly CancellationToken _token = isCancellable ? new CancellationToken() : CancellationToken.None;

        public List<ElementModel> CreateFromSchedules()
        {
            List<ElementModel> schedules = [];

            IEnumerable<ViewSchedule> collector = new FilteredElementCollector(_document)
                .OfClass(typeof(ViewSchedule))
                .Cast<ViewSchedule>();

            foreach (ViewSchedule viewSchedule in collector)
            {
                _token.ThrowIfCancellationRequested();

                ElementModel scheduleModel = new()
                {
                    Id = viewSchedule.Id.Value,
                    Name = viewSchedule.Name,
                };
                schedules.Add(scheduleModel);
            }
            return schedules;
        }
    }
}

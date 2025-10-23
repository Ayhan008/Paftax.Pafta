using Autodesk.Revit.DB;
using Paftax.Pafta.Shared.Models;

namespace Paftax.Pafta.Revit2026.Factories
{
    internal class ScheduleModelFactory(Document document)
    {
        private readonly Document _document = document;
        public List<ScheduleModel> CreateModels()
        {
            List<ScheduleModel> scheduleModels = [];

            IEnumerable<ViewSchedule> viewSchedules = new FilteredElementCollector(_document)
                .OfClass(typeof(ViewSchedule))
                .Cast<ViewSchedule>();

            foreach (ViewSchedule viewSchedule in viewSchedules)
            {
                ScheduleModel scheduleModel = new()
                {
                    Id = viewSchedule.Id.Value,
                    Name = viewSchedule.Name,
                };
                scheduleModels.Add(scheduleModel);
            }
            return scheduleModels;
        }
    }
}

using Autodesk.Revit.DB;
using Paftax.Pafta.Shared.Models;

namespace Paftax.Pafta.Revit2026.Factories
{
    internal static class ScheduleModelFactory
    {
        public static List<ScheduleModel> CreateScheduleModels(List<ViewSchedule> viewSchedules)
        {
            List<ScheduleModel> scheduleModels = [];

            foreach (var viewSchedule in viewSchedules)
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

using Autodesk.Revit.DB;
using Paftax.Pafta.Shared.Models.Element;

namespace Paftax.Pafta.Revit2026.Factories
{
    internal static class ScheduleModelFactory
    {
        public static ScheduleModel Create(ViewSchedule viewSchedule)
        {
            return new ScheduleModel
            {
                Id = viewSchedule.Id.Value,
                Name = viewSchedule.Name,
                CategoryName = viewSchedule.Category?.Name ?? "No Category",
                LevelId = viewSchedule.LevelId.Value,
                WorksetId = viewSchedule.WorksetId.IntegerValue
            };
        }

        public static List<ScheduleModel> Create(List<ViewSchedule> viewSchedules)
        {
            if (viewSchedules == null)
                return [];

            return [.. viewSchedules.Select(Create)];
        }
    }
}

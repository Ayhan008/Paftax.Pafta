using Autodesk.Revit.DB;
using Paftax.Pafta.Revit2026.Utilities;
using Paftax.Pafta.Shared.Models;

namespace Paftax.Pafta.Revit2026.Mappers
{
    internal static class ScheduleMapper
    {
        public static ScheduleModel MapToScheduleModel(ViewSchedule viewSchedule)
        {
            return new ScheduleModel
            {
                Id = viewSchedule.Id.ToLong(),
                Name = viewSchedule.Name,
            };
        }

        public static IEnumerable<ScheduleModel> MapToScheduleModels(IEnumerable<ViewSchedule> viewSchedules)
        {
            return viewSchedules.Select(vs => MapToScheduleModel(vs));
        }
    }
}

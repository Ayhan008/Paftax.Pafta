using Autodesk.Revit.DB;
using Paftax.Pafta.UI.Models;

namespace Paftax.Pafta.Revit2026.Utilities
{
    internal static class Mappers
    {
        #region ViewSchedule -> ScheduleModel

        public static ScheduleModel MapToScheduleModel(this ViewSchedule viewSchedule) => new()
        {
            Id = viewSchedule.Id.ToLong(),
            Name = viewSchedule.Name,
            IsChecked = false
        };

        public static IEnumerable<ScheduleModel> MapToScheduleModels(this IEnumerable<ViewSchedule> viewSchedules) =>
            viewSchedules.Select(vs => vs.MapToScheduleModel());

        public static IEnumerable<ViewSchedule> MapToViewSchedules(this IEnumerable<ScheduleModel> scheduleModels, Document doc) =>
            scheduleModels
                .Select(sm => doc.GetElement(sm.Id.ToElementId()))
                .OfType<ViewSchedule>();

        #endregion
    }
}

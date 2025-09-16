using Autodesk.Revit.DB;
using Paftax.Pafta.UI.Models;

namespace Paftax.Pafta.Revit2026.Utilities
{
    internal class Converters
    {
        public static long ElementIdToLong(ElementId elementId)
        {
            return elementId.Value;
        }

        public static ElementId LongToElementId(long id)
        {
            return new ElementId(id);
        }

        public static ScheduleModel ViewScheduleToScheduleModel(ViewSchedule viewSchedule)
        {
            return new ScheduleModel
            {
                Id = ElementIdToLong(viewSchedule.Id),
                IsChecked = false,
                Name = viewSchedule.Name,
            };
        }

        public static List<ScheduleModel> ViewSchedulesToScheduleModels(IEnumerable<ViewSchedule> viewSchedules)
        {
            return [.. viewSchedules.Select(ViewScheduleToScheduleModel)];
        }
    }
}

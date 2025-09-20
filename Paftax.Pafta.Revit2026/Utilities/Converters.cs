using Autodesk.Revit.DB;
using Paftax.Pafta.UI.Models;

namespace Paftax.Pafta.Revit2026.Utilities
{
    internal class Converters
    {
        public static long ToLong(ElementId elementId)
        {
            return elementId.Value;
        }

        public static ElementId ToElementId(long id)
        {
            return new ElementId(id);
        }

        public static ScheduleModel ViewScheduleToScheduleModel(ViewSchedule viewSchedule)
        {
            return new ScheduleModel
            {
                Id = ToLong(viewSchedule.Id),
                IsChecked = false,
                Name = viewSchedule.Name,
            };
        }

        public static List<ScheduleModel> ViewSchedulesToScheduleModels(IEnumerable<ViewSchedule> viewSchedules)
        {
            return [.. viewSchedules.Select(ViewScheduleToScheduleModel)];
        }

        public static List<ViewSchedule> ToViewSchedules(IEnumerable<ScheduleModel> scheduleModels, Document document)
        {
            List<ViewSchedule> viewSchedules = [];

            foreach (ScheduleModel scheduleModel in scheduleModels)
            {
                ElementId elementId = ToElementId(scheduleModel.Id);

                Element element = document.GetElement(elementId);
                if (element is ViewSchedule viewSchedule)
                    viewSchedules.Add(viewSchedule);              
            }
            return viewSchedules;
        }
    }
}

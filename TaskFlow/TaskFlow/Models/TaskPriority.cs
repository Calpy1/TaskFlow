using System.Windows.Media;
using static TaskFlow.Models.TaskStatus;

namespace TaskFlow.Models
{
    public class TaskPriority
    {
        public enum Priority
        {
            Low,
            Medium,
            High
        }

        public static SolidColorBrush ToBrush(Priority priority)
        {
            string color = "#F19CBB";

            switch (priority)
            {
                case Priority.High:
                    color = "#df2a2a";
                    break;
                case Priority.Medium:
                    color = "#df2a2a";
                    break;
                case Priority.Low:
                    color = "#df2a2a";
                    break;
            }
            return new((Color)ColorConverter.ConvertFromString(color));
        }
    }
}

using System.Windows.Media;

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

        public static SolidColorBrush ToBrush(Priority priorityStatus)
        {
            if (priorityStatus == Priority.High)
            {
                return new((Color)ColorConverter.ConvertFromString("#df2a2a"));
            }

            if (priorityStatus == Priority.Medium)
            {
                return new((Color)ColorConverter.ConvertFromString("#deb12f"));
            }

            if (priorityStatus == Priority.Low)
            {
                return new((Color)ColorConverter.ConvertFromString("#FF41A45D"));
            }

            return new SolidColorBrush(Colors.Pink);
        }
    }
}

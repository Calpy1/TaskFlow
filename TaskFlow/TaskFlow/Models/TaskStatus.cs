using System.Windows.Media;

namespace TaskFlow.Models
{
    public class TaskStatus
    {
        public enum Status
        {
            Completed,
            In_Progress,
            Delayed
        }

        public static SolidColorBrush ToBrush(Status status)
        {
            if (status == Status.Delayed)
            {
                return new((Color)ColorConverter.ConvertFromString("#df2a2a"));
            }

            if (status == Status.In_Progress)
            {
                return new((Color)ColorConverter.ConvertFromString("#deb12f"));
            }

            if (status == Status.Completed)
            {
                return new((Color)ColorConverter.ConvertFromString("#FF41A45D"));
            }

            return new SolidColorBrush(Colors.Pink);
        }
    }
}

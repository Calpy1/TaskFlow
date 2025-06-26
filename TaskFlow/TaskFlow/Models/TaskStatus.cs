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
            string color = "#F19CBB";

            switch (status)
            {
                case Status.Delayed:
                    color = "#df2a2a";
                    break;
                case Status.In_Progress:
                    color = "#df2a2a";
                    break;
                case Status.Completed:
                    color = "#df2a2a";
                    break;
            }
            return new((Color)ColorConverter.ConvertFromString(color));
        }
    }
}

using System.Windows.Media;
using System.Drawing;
using static TaskFlow.Models.TaskStatus;
using static TaskFlow.Models.TaskPriority;
using MediaColor = System.Windows.Media.Color;
using MediaColorConverter = System.Windows.Media.ColorConverter;

namespace TaskFlow.Services
{
    public class ColorConverterService<T> where T : Enum
    {
        private static readonly Dictionary<Status, string> StatusColors = new Dictionary<Status, string>()
        {
            {Status.Delayed, "#df2a2a"},
            {Status.In_Progress, "#d0d81a"},
            {Status.Completed, "#299530"}
        };

        private static readonly Dictionary<Priority, string> PriorityColors = new Dictionary<Priority, string>()
        {
            {Priority.High, "#df2a2a"},
            {Priority.Medium, "#d0d81a"},
            {Priority.Low, "#299530"}
        };

        public static SolidColorBrush GetColor(T enumKey)
        {
            string colorHex = string.Empty;
            if (typeof(T) == typeof(Status))
            {
                StatusColors.TryGetValue((Status)(object)enumKey, out colorHex);
            }
            else if (typeof(T) == typeof(Priority))
            {
                PriorityColors.TryGetValue((Priority)(object)enumKey, out colorHex);
            }

            if (string.IsNullOrEmpty(colorHex))
            {
                colorHex = "f19cbb";
            }

            return StringToBrush(colorHex);
        }

        public static SolidColorBrush StringToBrush(string colorHex)
        {
            return new SolidColorBrush((MediaColor)MediaColorConverter.ConvertFromString(colorHex));
        }
    }
}

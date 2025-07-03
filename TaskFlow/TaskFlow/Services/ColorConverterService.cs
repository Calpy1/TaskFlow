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
        private static readonly Dictionary<Status, SolidColorBrush> StatusColors = new Dictionary<Status, SolidColorBrush>()
        {
            {Status.Delayed, StringToBrush("#df2a2a")},
            {Status.In_Progress, StringToBrush("#edbb55")},
            {Status.Completed, StringToBrush("#299530")}
        };

        private static readonly Dictionary<Priority, SolidColorBrush> PriorityColors = new Dictionary<Priority, SolidColorBrush>()
        {
            {Priority.High, StringToBrush("#df2a2a")},
            {Priority.Medium, StringToBrush("#edbb55")},
            {Priority.Low, StringToBrush("#299530")}
        };

        public static SolidColorBrush GetColor(T enumKey)
        {
            SolidColorBrush colorHex;
            if (typeof(T) == typeof(Status))
            {
                StatusColors.TryGetValue((Status)(object)enumKey, out colorHex);
                return colorHex;
            }
            else if (typeof(T) == typeof(Priority))
            {
                PriorityColors.TryGetValue((Priority)(object)enumKey, out colorHex);
                return colorHex;
            }
            return new SolidColorBrush(Colors.Pink);
        }

        public static SolidColorBrush StringToBrush(string colorHex)
        {
            return new SolidColorBrush((MediaColor)MediaColorConverter.ConvertFromString(colorHex));
        }
    }
}

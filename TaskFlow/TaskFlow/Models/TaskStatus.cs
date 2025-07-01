using System.Windows.Media;
using TaskFlow.Services;

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
    }
}

using System.Windows;
using TaskFlow.Controls;
using TaskFlow.Models;
using Priority = TaskFlow.Models.TaskPriority.Priority;
using TaskStatus = TaskFlow.Models.TaskStatus;
using Status = TaskFlow.Models.TaskStatus.Status;

namespace TaskFlow.Services
{
    internal class TaskCardService
    {
        private readonly CreateTaskService _createService = new CreateTaskService();

        public async Task<TaskCard> CreateCardAsync(string taskName, string author, string assignee, string dueDateRaw, Priority priority, Status status)
        {
            var dueDate = ParseDate(dueDateRaw);
            var priorityColor = ColorConverterService<Priority>.GetColor(priority);
            var statusColor = ColorConverterService<Status>.GetColor(status);

            var statusFormatted = status == Status.In_Progress ? "In Progress" : status.ToString();

            var model = new TaskModel
            {
                TaskName = taskName,
                TaskAuthor = author,
                TaskAssignee = assignee,
                CreatedDate = DateTime.Now.ToString("dd.MM.yyyy"),
                DueDate = dueDate,
                PriorityColor = priorityColor,
                TaskPriority = priority.ToString(),
                StatusColor = statusColor,
                TaskStatus = statusFormatted
            };

            var card = new TaskCard
            {
                DataContext = model
            };

            return card;
        }

        private string ParseDate(string input)
        {
            return DateTime.TryParse(input, out var dt) ? dt.ToString("dd.MM.yyyy") : "Invalid date format";
        }
    }
}

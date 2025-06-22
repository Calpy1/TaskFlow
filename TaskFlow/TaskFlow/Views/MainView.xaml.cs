using System.Windows;
using TaskFlow.Controls;
using TaskFlow.Models;
using Priority = TaskFlow.Models.TaskPriority.Priority;
using TaskStatus = TaskFlow.Models.TaskStatus;
using Status = TaskFlow.Models.TaskStatus.Status;

namespace TaskFlow.Views
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : Window
    {
        public TaskModel TaskModel { get; set; }

        public MainView()
        {
            InitializeComponent();

            AddTaskCard("Create some info additions", "Ivan N.", Priority.High, Status.In_Progress);
        }

        public void AddTaskCard(string taskName, string taskAuthor, Priority priority, Status status)
        {
            var priorityColor = TaskPriority.ToBrush(priority);
            var statusColor = TaskStatus.ToBrush(status);

            string statusFormatted;

            if (status == Status.In_Progress)
            {
                statusFormatted = "In Progress";
            }
            else
            {
                statusFormatted = status.ToString();
            }

            var taskModel = new TaskModel()
            {
                TaskName = taskName,
                TaskAuthor = taskAuthor,
                PriorityColor = priorityColor,
                TaskPriority = priority.ToString(),
                StatusColor = statusColor,
                TaskStatus = statusFormatted
            };


            var card = new TaskCard()
            {
                DataContext = taskModel,
            };

            CardsPanel.Children.Add(card);
        }

    }
}

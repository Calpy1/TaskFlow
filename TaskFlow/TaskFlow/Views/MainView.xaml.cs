using System.Windows;
using TaskFlow.Controls;
using TaskFlow.Models;
using Priority = TaskFlow.Models.TaskPriority.Priority;
using TaskStatus = TaskFlow.Models.TaskStatus;
using Status = TaskFlow.Models.TaskStatus.Status;
using TaskFlow.Properties;
using TaskFlow.Common;

namespace TaskFlow.Views
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : Window
    {
        private WindowPropertiesSaver _windowSaver;
        public TaskModel TaskModel { get; set; }

        public MainView()
        {
            InitializeComponent();

            AddTaskCard("First task", "Bobby A.", "Ivan G.", "14.08.25", Priority.High, Status.Completed);
            AddTaskCard("First task", "Bobby A.", "Ivan G.", "14.08.25", Priority.High, Status.Completed);
            AddTaskCard("First task", "Bobby A.", "Ivan G.", "14.08.25", Priority.High, Status.Completed);
            AddTaskCard("First task", "Bobby A.", "Ivan G.", "14.08.25", Priority.High, Status.Completed);
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);


            _windowSaver = new WindowPropertiesSaver(this, "MainView");
            _windowSaver.Load();

            this.LocationChanged += (s, ev) => _windowSaver.Save();
            this.SizeChanged += (s, ev) => _windowSaver.Save();
            this.StateChanged += (s, ev) => _windowSaver.Save();
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);

            _windowSaver?.Save();
        }

        public void AddTaskCard(string taskName, string taskAuthor, string taskAssignee, string dueDate, Priority priority, Status status)
        {
            dueDate = DueDateParse(dueDate);
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

            TaskModel taskModel = new TaskModel()
            {
                TaskName = taskName,
                TaskAuthor = taskAuthor,
                TaskAssignee = taskAssignee,
                DueDate = dueDate,
                PriorityColor = priorityColor,
                TaskPriority = priority.ToString(),
                StatusColor = statusColor,
                TaskStatus = statusFormatted,
                CreatedDate = DateTime.Now.ToString("dd.MM.yyyy"),
            };


            var card = new TaskCard()
            {
                DataContext = taskModel,
            };

            CardsPanel.Children.Add(card);
        }

        public string DueDateParse(string dateString)
        {
            if (DateTime.TryParse(dateString, out DateTime dueDate))
            {
                return dueDate.ToString("dd.MM.yyyy");
            }
            else
            {
                return "Invalid date format";
            }
        }
    }
}

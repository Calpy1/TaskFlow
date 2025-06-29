using System.Windows;
using TaskFlow.Controls;
using TaskFlow.Models;
using Priority = TaskFlow.Models.TaskPriority.Priority;
using TaskStatus = TaskFlow.Models.TaskStatus;
using Status = TaskFlow.Models.TaskStatus.Status;
using TaskFlow.Properties;
using TaskFlow.Common;
using TaskFlow.Services;

namespace TaskFlow.Views
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : Window // TODO: Разделить бизнес-логику и UI по SRP
    {
        private WindowPropertiesSaver _windowSaver;
        private GetTaskService _taskService = new GetTaskService();
        public TaskModel TaskModel { get; set; }

        public MainView()
        {
            InitializeComponent();

            //_ = AddTaskCard("First task", "FFFF.", "AAAAA", "14.08.25", Priority.High, Status.Completed); 
            _ = _taskService.GetTaskWithApiAsync();
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

        public async Task AddTaskCard(string taskName, string taskAuthor, string taskAssignee, string dueDate, Priority priority, Status status)
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
                CreatedDate = DateTime.Now.ToString("dd.MM.yyyy"),
                DueDate = dueDate,
                PriorityColor = priorityColor,
                TaskPriority = priority.ToString(),
                StatusColor = statusColor,
                TaskStatus = statusFormatted,
            };


            //TasksHelper tasksHelper = new TasksHelper();
            CreateTaskService createTaskService = new CreateTaskService();
            bool result = await createTaskService.CreateWithApiAsync(taskModel);

            if (result)
            {
                var card = new TaskCard()
                {
                    DataContext = taskModel,
                };

                CardsPanel.Children.Add(card);
            }
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

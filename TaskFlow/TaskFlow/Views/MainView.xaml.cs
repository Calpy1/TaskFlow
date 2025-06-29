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
    public partial class MainView : Window
    {
        private WindowPropertiesSaver _windowSaver;
        private GetTaskService _taskService = new GetTaskService();
        private readonly TaskCardService _cardService = new TaskCardService();

        public MainView()
        {
            InitializeComponent();

            _ = AddTaskCard("First task", "Alexander", "Alexander", "14.08.25", Priority.High, Status.Completed);
            LoadTasks();
        }

        public async void LoadTasks()
        {
            var cards = await _taskService.GetTasksWithApiAsync();
            if (cards != null && cards.Count > 0)
            {
                foreach (var card in cards)
                {
                    CardsPanel.Children.Add(card);
                }
            }
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            _windowSaver = new WindowPropertiesSaver(this, "MainView");
            _windowSaver.Load();

            LocationChanged += (s, ev) => _windowSaver.Save();
            SizeChanged += (s, ev) => _windowSaver.Save();
            StateChanged += (s, ev) => _windowSaver.Save();
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);
            _windowSaver?.Save();
        }

        public async Task AddTaskCard(string taskName, string taskAuthor, string taskAssignee, string dueDate, Priority priority, Status status) // Test
        {
            var task = new TaskModel()
            {
                TaskName = taskName,
                TaskAuthor = taskAuthor,
                TaskAssignee = taskAssignee,
                CreatedDate = DateTime.Now.ToString("dd.MM.yy"),
                DueDate = dueDate,
                TaskPriority = priority.ToString(),
                TaskStatus = status.ToString(),
                
            };
            CreateTaskService createTaskService = new CreateTaskService();
            await createTaskService.CreateWithApiAsync(task);
        }
    }
}

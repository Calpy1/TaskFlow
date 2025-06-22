using System.Windows;
using TaskFlow.Controls;
using TaskFlow.Models;
using Priority = TaskFlow.Models.TaskPriority.Priority;

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

            AddTaskCard("Create some info additions", "I am", Priority.Medium);
            AddTaskCard("Add user information", "I am", Priority.Low);
            AddTaskCard("Check user information", "I am", Priority.High);
        }

        public void AddTaskCard(string taskName, string taskAuthor, Priority priority)
        {
            var color = TaskPriority.ToBrush(priority);

            var taskModel = new TaskModel()
            {
                TaskName = taskName,
                TaskAuthor = taskAuthor,
                PriorityColor = color,
                TaskPriority = priority.ToString(),
            };

            var card = new TaskCard()
            {
                DataContext = taskModel,
            };

            CardsPanel.Children.Add(card);
        }

    }
}

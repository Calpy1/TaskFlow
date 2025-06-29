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
        private readonly TaskCardService _cardService = new TaskCardService();

        public MainView()
        {
            InitializeComponent();

            //_ = AddTaskCard("First task", "FFFF.", "AAAAA", "14.08.25", Priority.High, Status.Completed);
            //_ = _taskService.GetTaskWithApiAsync();
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

        public async Task AddTaskCard(string taskName, string taskAuthor, string taskAssignee, string dueDate, Priority priority, Status status)
        {
            var card = await _cardService.CreateCardAsync(taskName, taskAuthor, taskAssignee, dueDate, priority, status);
            if (card != null)
            {
                CardsPanel.Children.Add(card);
            }
            else
            {
                MessageBox.Show("Не удалось создать задачу.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}

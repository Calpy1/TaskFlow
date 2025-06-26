using System.Text.Json.Serialization;
using System.Windows.Media;

namespace TaskFlow.Models
{
    public class TaskModel : BaseModel
    {
        private string _taskName;
        private string _taskAuthor;
        private string _taskAssignee;
        private SolidColorBrush _taskPriorityColor;
        private string _taskPriorityText;
        private SolidColorBrush _taskStatusColor;
        private string _taskStatusText;
        private string _createdDate;
        private string _dueDate;

        public string TaskName
        {
            get => _taskName;

            set
            {
                if (_taskName == value)
                {
                    return;
                }

                _taskName = value;
                OnPropertyChanged();
            }
        }

        public string TaskAssignee
        {
            get => _taskAssignee;

            set
            {
                if (_taskAssignee == value)
                {
                    return;
                }

                _taskAssignee = value;
                OnPropertyChanged();
            }
        }

        public string TaskAuthor
        {
            get => _taskAuthor;

            set
            {
                if (_taskAuthor == value)
                {
                    return;
                }

                _taskAuthor = value;
                OnPropertyChanged();
            }
        }

        [JsonIgnore]
        public SolidColorBrush PriorityColor
        {
            get => _taskPriorityColor;

            set
            {
                if (_taskPriorityColor == value)
                {
                    return;
                }

                _taskPriorityColor = value;
                OnPropertyChanged();
            }
        }

        public string TaskPriority
        {
            get => _taskPriorityText;

            set
            {
                if (_taskPriorityText == value)
                {
                    return;
                }

                _taskPriorityText = value;
                OnPropertyChanged();
            }
        }

        [JsonIgnore]
        public SolidColorBrush StatusColor
        {
            get => _taskStatusColor;

            set
            {
                if (_taskStatusColor == value)
                {
                    return;
                }

                _taskStatusColor = value;
                OnPropertyChanged();
            }
        }

        public string TaskStatus
        {
            get => _taskStatusText;

            set
            {
                if (_taskStatusText == value)
                {
                    return;
                }

                _taskStatusText = value;
                OnPropertyChanged();
            }
        }

        public string CreatedDate
        {
            get => _createdDate;

            set
            {
                if (_createdDate == value)
                {
                    return;
                }

                _createdDate = value;
                OnPropertyChanged();
            }
        }

        public string DueDate
        {
            get => _dueDate;

            set
            {
                if (_dueDate == value)
                {
                    return;
                }

                _dueDate = value;
                OnPropertyChanged();
            }
        }
    }
}

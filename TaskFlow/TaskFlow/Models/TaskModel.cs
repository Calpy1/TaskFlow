using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace TaskFlow.Models
{
    public class TaskModel : BaseModel
    {
        private string _taskName;
        private string _taskAuthor;
        private SolidColorBrush _taskPriorityColor;
        private string _taskPriorityText;

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
    }
}

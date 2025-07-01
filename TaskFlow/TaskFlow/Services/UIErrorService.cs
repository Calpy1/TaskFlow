using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using TaskFlow.Controls;
using System.Windows.Media;

namespace TaskFlow.Services
{
    class UIErrorService
    {
        public UIErrorService(params CustomTextBox[] customTextBoxes)
        {
            foreach (var customTextBox in customTextBoxes)
            {
                AttachInputEventHandlers(customTextBox);
            }
        }

        private static readonly SolidColorBrush ErrorBrush = new((Color)ColorConverter.ConvertFromString("#FFE05D5D"));
        private static readonly SolidColorBrush NormalBrush = new((Color)ColorConverter.ConvertFromString("#FF5A6F86"));
        private static readonly SolidColorBrush TransparentBrush = new(Colors.Transparent);

        public void AttachInputEventHandlers(CustomTextBox customTextBox)
        {
            var textBox = customTextBox.TextBoxInput;
            textBox.PreviewMouseDown += ClearErrorVisuals;
            textBox.GotFocus += ClearErrorVisuals;
        }

        public static void MarkFieldAsError(CustomTextBox field)
        {
            field.OuterBorder.BorderBrush = ErrorBrush;
            field.PlaceholderTextBlock.Foreground = ErrorBrush;
            field.PlaceholderSymbolBlock.Foreground = ErrorBrush;
        }

        private CustomTextBox? FindParentCustomTextBox(DependencyObject child)
        {
            while (child != null && child is not CustomTextBox)
            {
                child = VisualTreeHelper.GetParent(child);
            }
            return child as CustomTextBox;
        }

        private void ClearErrorVisuals(object sender, RoutedEventArgs e) => ResetFieldError(sender);
        private void ClearErrorVisuals(object sender, MouseButtonEventArgs e) => ResetFieldError(sender);

        private void ResetFieldError(object sender)
        {
            if (sender is not TextBox textBox)
            {
                return;
            }

            var parent = FindParentCustomTextBox(textBox);
            if (parent == null)
            {
                return;
            }

            parent.OuterBorder.BorderBrush = TransparentBrush;
            parent.PlaceholderTextBlock.Foreground = NormalBrush;
            parent.PlaceholderSymbolBlock.Foreground = NormalBrush;
        }
    }
}

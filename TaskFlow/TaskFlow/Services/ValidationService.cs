using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Controls;

namespace TaskFlow.Services
{
    class ValidationService
    {
        public bool ValidateRequiredFields(params CustomTextBox[] fields)
        {
            bool allValid = true;
            foreach (var field in fields)
            {
                if (string.IsNullOrWhiteSpace(field.Text))
                {
                    UIErrorService.MarkFieldAsError(field);
                    allValid = false;
                }
            }
            return allValid;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using STS.Models;
using STS.ViewModels;

namespace STS.Validators
{
    public class EmployeeEmailValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var ViewModel = ((EmployeeFormViewModel)validationContext.ObjectInstance);
            var IsNewEmployee = ViewModel.id == null;
            if (IsNewEmployee)
            {
                var Employee = new ApplicationDbContext().Users.SingleOrDefault(User => User.Email == ViewModel.Email);
                if (Employee != null)
                {
                    return new ValidationResult(STS.Resources.Views.Employees.UsedEmail);
                }
            }
            return ValidationResult.Success;
        }
    }
}

using STS.Validators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace STS.ViewModels
{
    public class EmployeeFormViewModel
    {
        public IEnumerable<SelectListItem> Locations { get; set; }
        public string id { set; get; }
        [EmployeeEmailValidation]
        [EmailAddress(ErrorMessageResourceName = "IsEmail", ErrorMessageResourceType = typeof(Resources.Views.Employees))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.Views.Employees))]
        [Display(Name = "Email", ResourceType = typeof(Resources.Views.Employees))]
        public string Email { set; get; }
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.Views.Employees))]
        [Display(Name = "EmployeeLocationId", ResourceType = typeof(Resources.Views.Employees))]
        public int EmployeeLocationId { set; get; }
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.Views.Employees))]
        [StringLength(90, ErrorMessageResourceName = "StringLength", ErrorMessageResourceType = typeof(Resources.Views.Shipments))]
        [Display(Name = "EmployeeName", ResourceType = typeof(Resources.Views.Employees))]
        public string EmployeeName { get; set; }
        [Phone(ErrorMessageResourceName = "IsPhone", ErrorMessageResourceType = typeof(Resources.Views.Employees))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.Views.Employees))]
        [Display(Name = "PhoneNumber", ResourceType = typeof(Resources.Views.Employees))]
        public string PhoneNumber { get; set; }
        [Display(Name = "Enabled", ResourceType = typeof(Resources.Views.Employees))]
        public bool Enabled { get; set; }
    }

    public class EmployeeDetailsViewModel
    {
        [Display(Name = "Email", ResourceType = typeof(Resources.Views.Employees))]
        public string Email { set; get; }
        [Display(Name = "EmployeeLocationId", ResourceType = typeof(Resources.Views.Employees))]
        public string Location { set; get; }
        [Display(Name = "EmployeeName", ResourceType = typeof(Resources.Views.Employees))]
        public string EmployeeName { get; set; }
        [Display(Name = "PhoneNumber", ResourceType = typeof(Resources.Views.Employees))]
        public string PhoneNumber { get; set; }
        [Display(Name = "DateAdded", ResourceType = typeof(Resources.Views.Employees))]
        public string DateAdded { get; set; }
        [Display(Name = "Status", ResourceType = typeof(Resources.Views.Employees))]
        public string Status { get; set; }
    }
}
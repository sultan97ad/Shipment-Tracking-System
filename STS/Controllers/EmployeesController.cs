using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;
using STS.Models;
using STS.ViewModels;
using STS.Dtos;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using STS.Resources.Views;

namespace STS.Controllers
{
    [Authorize(Roles = "Admin")]
    public class EmployeesController : Controller
    {
        private ApplicationUserManager _userManager;
        private ApplicationDbContext DbContext;

        public EmployeesController()
        {
            DbContext = ApplicationDbContext.Create();
        }

        public EmployeesController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // GET: Employees{?LocationId=00}
        public ActionResult Index(int LocationId = 0)
        {
            if (LocationId != 0)
            {
                ViewBag.LocationId = LocationId;
            }
            return View();
        }

        public ActionResult LoadData(int LocationId = 0)
        {
            try
            {
                var draw = Request.Form.GetValues("draw").FirstOrDefault();
                var start = Request.Form.GetValues("start").FirstOrDefault();
                var length = Request.Form.GetValues("length").FirstOrDefault();
                var searchValue = Request.Form.GetValues("search[value]").FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                var EmployeesData = GetEmployeesData(LocationId);
                EmployeesData = SortEmployeesData(EmployeesData);
                EmployeesData = SearchEmployeesData(EmployeesData, searchValue);
                recordsTotal = EmployeesData.Count();
                IEnumerable<EmployeeDto> FilteredEmployeesData = FilterEmployeesData(EmployeesData, pageSize, skip);
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = FilteredEmployeesData });
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Route("Employees/Details/{id}")]
        public ActionResult Details(string Id)
        {
            var Employee = UserManager.FindById(Id);
            if (IsExist(Employee))
            {
                return View(GenerateEmployeeDetailsViewModel(Employee));
            }
            return HttpNotFound();
        }

        // GET: Employees/New
        public ActionResult New()
        {
            return View("EmployeeForm", GenerateEmployeeFormViewModel());
        }

        // Get: Locations/Save
        public ActionResult Save()
        {
            return RedirectToAction("Index");
        }

        // POST: Locations/Save
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(EmployeeFormViewModel ViewModel)
        {
            if (!ModelState.IsValid)
            {
                ViewModel = UpdateEmployeeFormViewModel(ViewModel);
                return View("EmployeeForm", ViewModel);
            }
            var Employee = new ApplicationUser { };
            if (IsNewEmployee(ViewModel))
            {
                 Employee = GenerateNewEmployee(ViewModel);
                var result = UserManager.Create(Employee, Credentials.NewEmployeePassword);
                if (result.Succeeded)
                {
                    UserManager.AddToRole(Employee.Id, "Employee");
                }
                return RedirectToAction("index");
            }
            else
            {
                Employee = UserManager.FindById(ViewModel.id);
                Employee = UpdateEmployee(Employee, ViewModel);
                UserManager.Update(Employee);
            }
            return Redirect("Details/" + Employee.Id);
        }

        // GET: Employees/Update/{id}
        [Route("Employees/Update/{Id}")]
        public ActionResult Update(string Id)
        {
            var Employee = UserManager.FindById(Id);
            if (IsExist(Employee))
            {
                var ViewModel = GenerateEmployeeFormViewModel(Employee);
                return View("EmployeeForm", ViewModel);
            }
            return HttpNotFound();
        }

        #region Helpers

        private string LocationToString(Location Location)
        {
            return Location.City.ToString() + " - " + Location.LocationName.ToString();
        }

        private EmployeeDetailsViewModel GenerateEmployeeDetailsViewModel(ApplicationUser Employee)
        {
            var ViewModel = new EmployeeDetailsViewModel
            {
                Email = Employee.Email,
                EmployeeName = Employee.EmployeeName,
                Location = LocationToString(GetLocation(Employee.EmployeeLocationId)),
                DateAdded = Employee.EmployeeDateAdded.ToShortDateString(),
                PhoneNumber = Employee.PhoneNumber,
                Status = GetAccountStatus(Employee)
            };
            return ViewModel;
        }

        private string GetAccountStatus(ApplicationUser Employee)
        {
            return Employee.LockoutEnabled ? Employees.Disabled : Employees.Enabled;
        }

        private Location GetLocation(int employeeLocationId)
        {
            return DbContext.Locations.SingleOrDefault(Location=> Location.Id == employeeLocationId);
        }

        private EmployeeFormViewModel GenerateEmployeeFormViewModel()
        {
            return new EmployeeFormViewModel { Locations = GetLocationsList() };
        }

        private bool IsNewEmployee(EmployeeFormViewModel EmployeeFormViewModel)
        {
            return EmployeeFormViewModel.id == null;
        }

        private ApplicationUser GenerateNewEmployee(EmployeeFormViewModel ViewModel)
        {
            var Employee = new ApplicationUser
            {
                UserName = ViewModel.Email,
                Email = ViewModel.Email,
                EmployeeLocationId = ViewModel.EmployeeLocationId,
                EmployeeName = ViewModel.EmployeeName,
                PhoneNumber = ViewModel.PhoneNumber,
                EmployeeDateAdded = DateTime.Now,
                LockoutEndDateUtc = new DateTime(3000,1,1),
                LockoutEnabled = false
            };
            return Employee;
        }

        private ApplicationUser UpdateEmployee(ApplicationUser Employee, EmployeeFormViewModel ViewModel)
        {
            Employee.EmployeeName = ViewModel.EmployeeName;
            Employee.EmployeeLocationId = ViewModel.EmployeeLocationId;
            Employee.PhoneNumber = ViewModel.PhoneNumber;
            Employee.LockoutEnabled = ViewModel.LockedOut;
            return Employee;
        }

        private EmployeeFormViewModel GenerateEmployeeFormViewModel(ApplicationUser Employee)
        {
            var ViewModel = new EmployeeFormViewModel
            {
                Locations = GetLocationsList(),
                id = Employee.Id,
                Email = Employee.Email,
                EmployeeName = Employee.EmployeeName,
                EmployeeLocationId = Employee.EmployeeLocationId,
                PhoneNumber = Employee.PhoneNumber,
                LockedOut = Employee.LockoutEnabled
            };
            return ViewModel;
        }

        private IQueryable<ApplicationUser> GetEmployeesData(int LocationId)
        {
            var LocationEmployeesRequest = LocationId != 0;
            if (LocationEmployeesRequest)
            {
                return DbContext.Users.Where(User => User.EmployeeLocationId == LocationId).AsQueryable();
            }
            return DbContext.Users.Where(User => User.EmployeeLocationId != 0).AsQueryable();
        }

        private IQueryable<ApplicationUser> SortEmployeesData(IQueryable<ApplicationUser> EmployeesData)
        {
                EmployeesData = EmployeesData.OrderBy(Employee => Employee.EmployeeLocationId);
            return EmployeesData;
        }

        private IQueryable<ApplicationUser> SearchEmployeesData(IQueryable<ApplicationUser> EmployeesData, string searchValue)
        {
            if (!string.IsNullOrEmpty(searchValue))
            {
                EmployeesData = EmployeesData.Where(Employee => Employee.EmployeeName.StartsWith(searchValue));
            }
            return EmployeesData;
        }

        private IEnumerable<EmployeeDto> FilterEmployeesData(IQueryable<ApplicationUser> EmployeesData, int pageSize, int skip)
        {
            IEnumerable<EmployeeDto> FilteredEmployeesData = EmployeesData.Skip(skip).Take(pageSize).ToList().Select(Employee => new EmployeeDto
            {
            id = Employee.Id,
            Name = Employee.EmployeeName,
            Email = Employee.Email,
            Location = LocationToString(GetLocation(Employee.EmployeeLocationId))
            });
            return FilteredEmployeesData;
        }

        private IEnumerable<SelectListItem> GetLocationsList()
        {
            IEnumerable<SelectListItem> Locations = DbContext.Locations.Where(Location => Location.InService).ToList().Select(Location => new SelectListItem
            {
                Value = Location.Id.ToString(),
                Text = LocationToString(Location)
            });
            return Locations;
        }

        private EmployeeFormViewModel UpdateEmployeeFormViewModel(EmployeeFormViewModel ViewModel)
        {
            ViewModel.Locations = GetLocationsList();
            return ViewModel;
        }

        private bool IsExist(ApplicationUser Employee)
        {
            return Employee != null;
        }

        #endregion

    }
}

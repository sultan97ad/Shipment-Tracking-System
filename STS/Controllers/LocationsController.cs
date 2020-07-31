using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;
using STS.Models;
using STS.ViewModels;
using STS.Dtos;


namespace STS.Controllers
{
    [Authorize(Roles = "Admin")]
    public class LocationsController : Controller
    {
        private ApplicationDbContext DbContext;

        public LocationsController()
        {
            DbContext = ApplicationDbContext.Create();
        }

        // GET: Locations
        public ActionResult Index()
        {
            return View();
        }

        // GET: Locations/LoadData
        public ActionResult LoadData()
        {
            try
            {
                var draw = Request.Form.GetValues("draw").FirstOrDefault();
                var start = Request.Form.GetValues("start").FirstOrDefault();
                var length = Request.Form.GetValues("length").FirstOrDefault();
                var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
                var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
                var searchValue = Request.Form.GetValues("search[value]").FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                var LocationsData = GetLocationsData();
                LocationsData = SortLocationsData(LocationsData, sortColumn, sortColumnDir);
                LocationsData = SearchLocationsData(LocationsData, searchValue);
                recordsTotal = LocationsData.Count();
                IEnumerable<LocationDto> FilteredLocationsData = FilterLocationsData(LocationsData, pageSize, skip);
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = FilteredLocationsData });
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Route("Locations/Details/{LocationId}")]
        public ActionResult Details(int LocationId)
        {
            var Location = GetLocationById(LocationId);
            if (IsExist(Location))
            {
                return View(GenerateLocationDetailsViewModel(Location));
            }
             return HttpNotFound();
        }

        // GET: Locations/New
        public ActionResult New()
        {
            return View("LocationForm", GenerateNewLocationFormViewModel());
        }

        // Get: Locations/Save
        public ActionResult Save()
        {
            return RedirectToAction("Index");
        }

        // POST: Locations/Save
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(LocationFormViewModel ViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("LocationForm", ViewModel);
            }
            var Location = new Location { };
            if (IsNewLocation(ViewModel))
            {
                Location = GenerateNewLocation(ViewModel);
                DbContext.Locations.Add(Location);
            }
            else
            {
                Location = GetLocationById(ViewModel.LocationId);
                Location = UpdateLocation(Location, ViewModel);
            }
            DbContext.SaveChanges();
            return Redirect("Details/" + Location.Id);
        }

        // GET: Locations/Update/1
        [Route("Locations/Update/{LocationId}")]
        public ActionResult Update(int LocationId)
        {
            var Location = GetLocationById(LocationId);
            if (IsExist(Location))
            {
                var ViewModel = GenerateLocationFormViewModel(Location);
                return View("LocationForm", ViewModel);
            }
           return HttpNotFound();
        }

        #region Helpers

        private Location GetLocationById(int LocationId)
        {
            return DbContext.Locations.SingleOrDefault(Location => Location.Id == LocationId && Location.InService);
        }

        private bool IsExist(Location Location)
        {
            return Location != null;
        }

        private string LocationToString(Location Location)
        {
            return Location.City.ToString() + " - " + Location.LocationName.ToString();
        }

        private LocationDetailsViewModel GenerateLocationDetailsViewModel(Location Location)
        {
            var ViewModel = new LocationDetailsViewModel
            {
                LocationId = Location.Id,
                LocationName = Location.LocationName,
                City = Location.City,
                CanBeDestination = Location.CanBeDestination,
                NumberOfEmployees = GetNumberOfEmployees(Location),
                NumberOfShipments = GetNumberOfShipments(Location)
            };
            return ViewModel;
        }

        private LocationFormViewModel GenerateNewLocationFormViewModel()
        {
            return new LocationFormViewModel { LocationId = 0 };
        }



        private bool IsNewLocation(LocationFormViewModel LocationFormViewModel)
        {
            return LocationFormViewModel.LocationId == 0;
        }

        private Location GenerateNewLocation(LocationFormViewModel ViewModel)
        {
            var Location = new Location { };
            Location.LocationName = ViewModel.LocationName;
            Location.City = ViewModel.City;
            Location.CanBeDestination = ViewModel.CanBeDestination;
            Location.InService = true;
            return Location;
        }



        private Location UpdateLocation(Location Location, LocationFormViewModel ViewModel)
        {
            Location.LocationName = ViewModel.LocationName;
            Location.City = ViewModel.City;
            Location.CanBeDestination = ViewModel.CanBeDestination;
            return Location;
        }

        private LocationFormViewModel GenerateLocationFormViewModel(Location Location)
        {
            var ViewModel = new LocationFormViewModel
            {
                LocationId = Location.Id,
                LocationName = Location.LocationName,
                City = Location.City,
                CanBeDestination = Location.CanBeDestination
            };
            return ViewModel;
        }

        private IQueryable<Location> GetLocationsData()
        {
            return DbContext.Locations.Where(Location => Location.InService).AsQueryable();
        }

        private IQueryable<Location> SortLocationsData(IQueryable<Location> LocationsData, string sortColumn, string sortColumnDir)
        {
            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
            {
                LocationsData = LocationsData.OrderBy(Location => Location.Id);
            }
            return LocationsData;
        }

        private IQueryable<Location> SearchLocationsData(IQueryable<Location> LocationsData, string searchValue)
        {
            if (!string.IsNullOrEmpty(searchValue))
            {
                LocationsData = LocationsData.Where(Location => Location.City.StartsWith(searchValue));
            }
            return LocationsData;
        }

        private IEnumerable<LocationDto> FilterLocationsData(IQueryable<Location> LocationsData, int pageSize, int skip)
        {
            IEnumerable<LocationDto> FilteredLocationsData = LocationsData.Skip(skip).Take(pageSize).ToList().Select(Location => new LocationDto
            {
            LocationId = Location.Id.ToString(),
            Address = LocationToString(Location),
            });
            return FilteredLocationsData;
        }

        private string GetNumberOfEmployees(Location Location)
        {
            return DbContext.Users.Where(User => User.EmployeeLocationId == Location.Id).Count().ToString();
        }

        private string GetNumberOfShipments(Location Location)
        {
            return DbContext.Shipments.Include(Shipment => Shipment.CurrentLocation).Where(Shipment => Shipment.CurrentLocation.Id == Location.Id).Count().ToString();
        }

        #endregion

    }
}

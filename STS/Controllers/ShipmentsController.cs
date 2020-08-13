using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;
using STS.Models;
using STS.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using STS.Dtos;
using IronBarCode;
using STS.Resources.Views;

namespace STS.Controllers
{
    [Authorize(Roles = "Employee")]
    public class ShipmentsController : Controller
    {
        private ApplicationDbContext DbContext;

        public ShipmentsController()
        {
            DbContext = ApplicationDbContext.Create();
        }

        // GET: Shipments
        public ActionResult Index()
        {
            return View();
        }

        // GET: Shipments/LoadData
        public ActionResult LoadData()
        {
            try
            {
                var EmployeeLocation = GetEmployeeLocation();
                var draw = Request.Form.GetValues("draw").FirstOrDefault();
                var start = Request.Form.GetValues("start").FirstOrDefault();
                var length = Request.Form.GetValues("length").FirstOrDefault();
                var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
                var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
                var searchValue = Request.Form.GetValues("search[value]").FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                var ShipmentsData = GetShipmentsData();
                ShipmentsData = SortShipmentsData(ShipmentsData, sortColumn, sortColumnDir);
                ShipmentsData = SearchShipmentsData(ShipmentsData, searchValue);
                recordsTotal = ShipmentsData.Count();
                IEnumerable<ShipmentDto> FilteredShipmentsData = FilterShipmentsData(ShipmentsData, pageSize, skip);
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = FilteredShipmentsData });
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Route("Shipments/Details/{TrackingNumber}")]
        public ActionResult Details(string TrackingNumber)
        {
            var Shipment = GetShipmentByTrackingNumber(TrackingNumber);
            if (IsExist(Shipment))
            {
                return View(GenerateShipmentDetailsViewModel(Shipment));
            }
             return HttpNotFound();
        }

        // GET: Shipments/New
        public ActionResult New()
        {
            return View("ShipmentForm", GenerateNewShipmentFormViewModel());
        }

        // Get: Shipments/Save
        public ActionResult Save()
        {
            return RedirectToAction("Index");
        }

        // POST: Shipments/Save
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(ShipmentFormViewModel ViewModel)
        {
            var EmployeeLocation = GetEmployeeLocation();
            if (!ModelState.IsValid)
            {
                ViewModel = UpdateShipmentFormViewModel(ViewModel);
                return View("ShipmentForm", ViewModel);
            }
            var Shipment = new Shipment { };
            if (IsNewShipment(ViewModel))
            {
                Shipment = GenerateNewShipment(ViewModel, EmployeeLocation);
                DbContext.Shipments.Add(Shipment);
                DbContext.Reports.Add(GenerateReport(Shipment, Event.Registered));
            }
            else
            {
                Shipment = GetShipmentByTrackingNumber(ViewModel.TrackingNumber);
                Shipment = UpdateShipment(Shipment, ViewModel);
                DbContext.Reports.Add(GenerateReport(Shipment, Event.Updated));
            }
            DbContext.SaveChanges();
            return Redirect("Details/" + Shipment.TrackingNumber);
        }

        // GET: Shipments/Update/1
        [Route("Shipments/Update/{TrackingNumber}")]
        public ActionResult Update(string TrackingNumber)
        {
            var Shipment = GetShipmentByTrackingNumber(TrackingNumber);
            if (IsExist(Shipment))
            {
                var ViewModel = GenerateShipmentFormViewModel(Shipment);
                return View("ShipmentForm", ViewModel);
            }
            return HttpNotFound();
        }

        [Route("Shipments/Barcode/{TrackingNumber}")]
        public ActionResult Barcode(string TrackingNumber)
        {
            var Shipment = GetShipmentByTrackingNumber(TrackingNumber);
            if (IsExist(Shipment))
            {
                return base.File(BarcodeWriter.CreateBarcode(TrackingNumber, BarcodeWriterEncoding.Code128).ToGifBinaryData() , "image/gif");
            }
            return HttpNotFound();
        }

        #region Helpers

        private string StatusToString(Status StatusCode)
        {
            switch (StatusCode)
            {
                case Status.WaitingShipping:
                    return Resources.Views.Shipments.ShipmentWaitingShipping;
                case Status.Shipping:
                    return Resources.Views.Shipments.ShipmentShipping;
                case Status.WaitingCollection:
                    return Resources.Views.Shipments.ShipmentWaitingCollection;
                case Status.Collected:
                    return Resources.Views.Shipments.ShipmentCollected;
                default:
                    return null;
            }
        }

        private string GenerateTrackingNumber()
        {
            string TrackingNumber = RandomDigits(11);
            while(IsUsed(TrackingNumber))
            {
                TrackingNumber = RandomDigits(11);
            }
            return TrackingNumber;
        }

        private string RandomDigits(int NumberOfDigits)
        {
            Random rand = new Random();
            string Result = "";
            for (int i = 0; i <= NumberOfDigits; i++)
            {
                Result += rand.Next(0, 9).ToString();
            }
            return Result;
        }

        private ApplicationUser GetEmployeeInformation(String UserId)
        {
            return new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(DbContext)).FindById(UserId);
        }

        private Shipment GetShipmentByTrackingNumber(string TrackingNumber)
        {
            return DbContext.Shipments.Include(Shipment => Shipment.Source).Include(Shipment => Shipment.CurrentLocation).Include(Shipment => Shipment.Destination).SingleOrDefault(Shipment => Shipment.TrackingNumber == TrackingNumber);
        }

        private bool IsExist(Shipment Shipment)
        {
            return Shipment != null;
        }

        private IEnumerable<SelectListItem> GetLocationsList()
        {
            IEnumerable<SelectListItem> Locations = DbContext.Locations.Where(Location => Location.CanBeDestination && Location.InService).ToList().Select(Location => new SelectListItem
            {
                Value = Location.Id.ToString(),
                Text = LocationToString(Location)
            });
            return Locations;
        }

        private List<SelectListItem> GetCollectionMethodsList()
        {
             var Methods = new List<SelectListItem> {
                new SelectListItem { Text = Shipments.PickUp , Value = "0" , Selected = true},
                new SelectListItem { Text = Shipments.Delivery , Value = "1"},
            };
            return Methods;
        }

        private string LocationToString(Location Location)
        {
            return Location.City + " - " + Location.LocationName;
        }

        private ShipmentDetailsViewModel GenerateShipmentDetailsViewModel(Shipment Shipment)
        {
            var ViewModel = new ShipmentDetailsViewModel
            {
                TrackingNumber = Shipment.TrackingNumber,
                DateAdded = Shipment.DateAdded.ToString(),
                ArrivalDate = Shipment.ArrivalDate.ToString(),
                ReceiverName = Shipment.ReceiverName,
                ReceiverPhoneNumber = Shipment.ReceiverPhoneNumber,
                SenderName = Shipment.SenderName,
                SenderPhoneNumber = Shipment.SenderPhoneNumber,
                WeightKG = Shipment.WeightKG.ToString(),
                Description = Shipment.Description,
                Source = LocationToString(Shipment.Source),
                Destination = LocationToString(Shipment.Destination),
                CollectionMethod = CollectionMethodToString((CollectionMethod)Shipment.CollectionMethod),
                DeliveryLocationUrl = GetDeliveryLocationUrl(Shipment)
            };
            return ViewModel;
        }

        private string GetDeliveryLocationUrl(Shipment shipment)
        {
            if(shipment.DeliveryLocationLatitude != 0 && shipment.DeliveryLocationlongitude != 0)
            {
                return "https://www.google.com/maps?q=Lat,Long".Replace("Lat", shipment.DeliveryLocationLatitude.ToString()).Replace("Long", shipment.DeliveryLocationlongitude.ToString());
            }
            return null;
        }

        private string CollectionMethodToString(CollectionMethod collectionMethod)
        {
            switch(collectionMethod)
            {
                case CollectionMethod.Pickup: return Shipments.PickUp;
                case CollectionMethod.Delivery: return Shipments.Delivery;
                default : return null; 
            }
        }

        private ShipmentFormViewModel GenerateNewShipmentFormViewModel()
        {
            return new ShipmentFormViewModel { CollectionMethods = GetCollectionMethodsList() , Locations = GetLocationsList(), TrackingNumber = null };
        }

        private Location GetEmployeeLocation()
        {
            var EmployeeLocationId = GetEmployeeInformation(User.Identity.GetUserId()).EmployeeLocationId;
            return DbContext.Locations.SingleOrDefault(Location => Location.Id == EmployeeLocationId);
        }

        private bool IsNewShipment(ShipmentFormViewModel ShipmentFormViewModel)
        {
            return ShipmentFormViewModel.TrackingNumber == null;
        }

        private Shipment GenerateNewShipment(ShipmentFormViewModel ViewModel, Location EmployeeLocation)
        {
            var Shipment = new Shipment { };
            Shipment.TrackingNumber = GenerateTrackingNumber();
            Shipment.SenderName = ViewModel.SenderName;
            Shipment.SenderPhoneNumber = ViewModel.SenderPhoneNumber;
            Shipment.ReceiverName = ViewModel.ReceiverName;
            Shipment.ReceiverPhoneNumber = ViewModel.ReceiverPhoneNumber;
            Shipment.WeightKG = ViewModel.WeightKG;
            Shipment.Description = ViewModel.Description;
            Shipment.Status = (byte)Status.WaitingShipping;
            Shipment.DateAdded = DateTime.Now;
            Shipment.ArrivalDate = DateTime.Now;
            Shipment.Destination = DbContext.Locations.SingleOrDefault(Location => Location.Id == ViewModel.DestinationLocationId);
            Shipment.CurrentLocation = EmployeeLocation;
            Shipment.Source = EmployeeLocation;
            Shipment.CollectionMethod = ViewModel.CollectionMethod;
            return Shipment;
        }

        private Report GenerateReport(Shipment Shipment, Event Event)
        {
            var TrackingRecord = new Report
            {
                Shipment = Shipment,
                Location = Shipment.CurrentLocation,
                DateTime = DateTime.Now,
                Event = (byte)Event
            };
            return TrackingRecord;
        }

        private ShipmentFormViewModel UpdateShipmentFormViewModel(ShipmentFormViewModel ViewModel)
        {
            ViewModel.Locations = GetLocationsList();
            ViewModel.CollectionMethods = GetCollectionMethodsList();
            return ViewModel;
        }

        private Shipment UpdateShipment(Shipment Shipment, ShipmentFormViewModel ViewModel)
        {
            Shipment.ReceiverName = ViewModel.ReceiverName;
            Shipment.ReceiverPhoneNumber = ViewModel.ReceiverPhoneNumber;
            Shipment.SenderName = ViewModel.SenderName;
            Shipment.SenderPhoneNumber = ViewModel.SenderPhoneNumber;
            Shipment.WeightKG = ViewModel.WeightKG;
            Shipment.Destination = DbContext.Locations.SingleOrDefault(Location => Location.Id == ViewModel.DestinationLocationId);
            Shipment.Description = ViewModel.Description;
            Shipment.CollectionMethod = ViewModel.CollectionMethod;
            return Shipment;
        }

        private ShipmentFormViewModel GenerateShipmentFormViewModel(Shipment Shipment)
        {
            var ViewModel = new ShipmentFormViewModel
            {
                TrackingNumber = Shipment.TrackingNumber,
                Locations = GetLocationsList(),
                CollectionMethods = GetCollectionMethodsList(),
                ReceiverName = Shipment.ReceiverName,
                ReceiverPhoneNumber = Shipment.ReceiverPhoneNumber,
                SenderName = Shipment.SenderName,
                SenderPhoneNumber = Shipment.SenderPhoneNumber,
                WeightKG = Shipment.WeightKG,
                Description = Shipment.Description,
                DestinationLocationId = Shipment.Destination.Id,
                CollectionMethod = Shipment.CollectionMethod
            };
            return ViewModel;
        }

        private IQueryable<Shipment> GetShipmentsData()
        {
            var EmployeeLocation = GetEmployeeLocation();
            return DbContext.Shipments.Include(Shipment => Shipment.Destination).Where(Shipment => Shipment.CurrentLocation.Id == EmployeeLocation.Id).AsQueryable();
        }

        private IQueryable<Shipment> SortShipmentsData(IQueryable<Shipment> ShipmentsData, string sortColumn, string sortColumnDir)
        {
            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
            {
                ShipmentsData = ShipmentsData.OrderBy(Shipment => Shipment.TrackingNumber);
            }
            return ShipmentsData;
        }

        private IQueryable<Shipment> SearchShipmentsData(IQueryable<Shipment> ShipmentsData, string searchValue)
        {
            if (!string.IsNullOrEmpty(searchValue))
            {
                ShipmentsData = ShipmentsData.Where(Shipment => Shipment.TrackingNumber.StartsWith(searchValue));
            }
            return ShipmentsData;
        }

        private IEnumerable<ShipmentDto> FilterShipmentsData(IQueryable<Shipment> ShipmentsData, int pageSize, int skip)
        {
            IEnumerable<ShipmentDto> FilteredShipmentsData = ShipmentsData.Skip(skip).Take(pageSize).ToList().Select(Shipment => new ShipmentDto
            {
                TrackingNumber = Shipment.TrackingNumber,
                ReceiverName = Shipment.ReceiverName,
                Destination = Shipment.Destination.City,
                Status = StatusToString((Status)Shipment.Status),
                HoldSince = (DateTime.Now - Shipment.ArrivalDate).Days.ToString()
            });
            return FilteredShipmentsData;
        }

        private bool IsUsed(string TrackingNumber)
        {
            return DbContext.Shipments.SingleOrDefault(Shipment => Shipment.TrackingNumber == TrackingNumber) != null;
        }

        enum Status
        {
        WaitingShipping,
        Shipping,
        WaitingCollection,
        Collected
        }

        enum Event
        {
        Registered,
        Departed,
        Arrived,
        WaitingCollection,
        Collected,
        Updated
        }

        enum CollectionMethod
        {
            Pickup,
            Delivery
        }

        #endregion

    }
}

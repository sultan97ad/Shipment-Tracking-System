using STS.Dtos;
using STS.Models;
using System.Web.Http;
using System.Data.Entity;
using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using STS.Resources.Api;

namespace STS.Controllers.Operations_api
{
    [Authorize(Roles = "Employee")]
    public class ShipmentsController : ApiController
    {
        private ApplicationDbContext DbContext;

        public ShipmentsController()
        {
            DbContext = ApplicationDbContext.Create();
        }

        [HttpPost]
        [Route("Operations-api/Shipments/{TrackingNumber}")]
        public IHttpActionResult Departed(string TrackingNumber)
        {
            var Shipment = GetShipmentByTrackingNumber(TrackingNumber);
            if (IsExist(Shipment))
            {
                if(SameLocation(Shipment , GetEmployeeInformation(User.Identity.GetUserId())) && IsWaitingShipping(Shipment))
                {
                    DbContext.Reports.Add(GenerateReport(Shipment, Event.Departed));
                    Shipment.CurrentLocation = null;
                    Shipment.Status = (byte)Status.Shipping;
                    DbContext.SaveChanges();
                    return Ok(Shipments.ShipmentOperationSuccess);
                }
                return BadRequest(Shipments.DifferentLocation);
            }
            return NotFound();
     }

        [HttpPost]
        [Route("Operations-api/Shipments/Collected/{TrackingNumber}")]
        public IHttpActionResult Collected(string TrackingNumber , string CollectorName)
        {
            var Shipment = GetShipmentByTrackingNumber(TrackingNumber);
            if (IsExist(Shipment))
            {
                if (SameLocation(Shipment, GetEmployeeInformation(User.Identity.GetUserId())) && IsWaitingCollection(Shipment))
                {
                        DbContext.Reports.Add(GenerateReport(Shipment, Event.Collected, CollectorName));
                        Shipment.CurrentLocation = null;
                        Shipment.Status = (byte)Status.Collected;
                        DbContext.SaveChanges();
                        return Ok(Shipments.ShipmentOperationSuccess);
                }
                return BadRequest(Shipments.DifferentLocation);
            }
            return NotFound();
        }


        [HttpGet]
        [Route("Operations-api/Shipments/Arrived/{TrackingNumber}")]
        public IHttpActionResult Preview(string TrackingNumber)
        {
            var Shipment = GetShipmentByTrackingNumber(TrackingNumber);
            if (IsExist(Shipment))
            {
                if(IsShipping(Shipment))
                {
                    return Ok(GetShipmentPreviewData(Shipment));
                }
                 return BadRequest(Shipments.IsNotShipping);
             }
            return NotFound();
        }


        [HttpPost]
        [Route("Operations-api/Shipments/Arrived/{TrackingNumber}")]
        public IHttpActionResult Arrived(string TrackingNumber)
        {
            var Shipment = GetShipmentByTrackingNumber(TrackingNumber);
            if (IsExist(Shipment))
            {
                if (IsShipping(Shipment))
                {
                    var EmployeeLocation = GetEmployeeLocation(User.Identity.GetUserId());
                    Shipment.CurrentLocation = EmployeeLocation;
                    Shipment.ArrivalDate = DateTime.Now;
                    Shipment.Status = (byte)UpdateArrivedShipmentStatus(Shipment, EmployeeLocation);
                    DbContext.Reports.Add(GenerateReport(Shipment , Event.Arrived));
                    if (IsWaitingCollection(Shipment))
                    {
                        DbContext.Reports.Add(GenerateReport(Shipment, Event.WaitingCollection));
                    }
                    DbContext.SaveChanges();
                    return Ok(Shipments.ShipmentOperationSuccess);
                }
                return BadRequest(Shipments.IsNotShipping);
            }
            return NotFound();
        }

        #region Helpers

        private Shipment GetShipmentByTrackingNumber(string TrackingNumber)
        {
            return DbContext.Shipments.Include(Shipment => Shipment.Source).Include(Shipment => Shipment.CurrentLocation).Include(Shipment => Shipment.Destination).SingleOrDefault(Shipment => Shipment.TrackingNumber == TrackingNumber);
        }

        private bool IsExist(Shipment Shipment)
        {
            return Shipment != null;
        }

        private bool SameLocation(Shipment Shipment , ApplicationUser ApplicationUser)
        {
            return Shipment.CurrentLocation.Id == ApplicationUser.EmployeeLocationId;
        }

        private ApplicationUser GetEmployeeInformation(String UserId)
        {
            return new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(DbContext)).FindById(UserId);
        }

        private Report GenerateReport(Shipment Shipment, Event Event, string CollectorName = null)
        {
            var TrackingRecord = new Report
            {
                Shipment = Shipment,
                Location = Shipment.CurrentLocation,
                DateTime = DateTime.Now,
                Event = (byte)Event
            };
            if (Event == Event.Collected)
            {
                TrackingRecord.SignedBy = CollectorName;
            }
            return TrackingRecord;
        }

        private bool IsShipping(Shipment Shipment)
        {
            return Shipment.Status == (byte)Status.Shipping;
        }

        public static string LocationToString(Location Location)
        {
            return Location.City.ToString() + " - " + Location.LocationName.ToString();
        }

        private ShipmentDto GetShipmentPreviewData(Shipment Shipment)
        {
            var ShipmentPreviewData = new ShipmentDto
            {
                TrackingNumber = Shipment.TrackingNumber,
                ReceiverName = Shipment.ReceiverName,
                Destination = LocationToString(Shipment.Destination),
                Status = StatusToString((Status)Shipment.Status)
            };
            return ShipmentPreviewData;
        }

        private Location GetEmployeeLocation(string UserId)
        {
            var EmployeeLocationId = GetEmployeeInformation(UserId).EmployeeLocationId;
            return DbContext.Locations.SingleOrDefault(Location => Location.Id == EmployeeLocationId);
        }

        private Status UpdateArrivedShipmentStatus(Shipment Shipment , Location EmployeeLocation)
        {
            return EmployeeLocation == Shipment.Destination ? Status.WaitingCollection : Status.WaitingShipping;
        }

        private bool IsWaitingCollection(Shipment Shipment)
        {
            return Shipment.Status == (byte)Status.WaitingCollection;
        }

        private bool IsWaitingShipping(Shipment Shipment)
        {
            return Shipment.Status == (byte)Status.WaitingShipping;
        }

        private string StatusToString(Status Status)
        {
            switch (Status)
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

        #endregion




    }
}

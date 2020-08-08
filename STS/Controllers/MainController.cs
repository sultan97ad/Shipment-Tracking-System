using STS.Dtos;
using STS.Models;
using STS.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using RouteAttribute = System.Web.Mvc.RouteAttribute;

namespace STS.Controllers
{
    public class MainController : Controller
    {
        private ApplicationDbContext DbContext;

        public MainController()
        {
            DbContext = ApplicationDbContext.Create();
        }

        public ActionResult Index()
        {
            return View();
        }

        // Get: Main/Track
        public ActionResult Track()
        {
            return RedirectToAction("Index");
        }

        // Post: Main/Track
        [HttpPost]
        public ActionResult Track(MainIndexViewModel ViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", ViewModel);
            }
            return Redirect("TrackShipment/"+ ViewModel.TrackingNumber);
        }

        [Route("Main/TrackShipment/{TrackingNumber}")]
        public ActionResult TrackShipment(string TrackingNumber)
        {
            var Shipment = GetShipmentByTrackingNumber(TrackingNumber);
            if (IsExist(Shipment))
            {
                return View(GenerateTrackShipmentViewModel(Shipment));
            }
            return HttpNotFound();
        }

        [Route("Main/TrackShipment/GetTrackingRecords/{TrackingNumber}")]
        public ActionResult GetTrackingRecords(string TrackingNumber)
        {
            var Shipment = GetShipmentByTrackingNumber(TrackingNumber);
            if (IsExist(Shipment))
            {
                return Json(GetShipmentTrackingRecords(Shipment));
            }
             return HttpNotFound();
        }

        #region Helpers

        private string StatusCodeToString(Status StatusCode)
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

        private  string LocationToString(Location Location)
        {
            return Location.City.ToString() + " - " + Location.LocationName.ToString();
        }

        private  Shipment GetShipmentByTrackingNumber(string TrackingNumber)
        {
            return DbContext.Shipments.Include(Shipment => Shipment.Source).Include(Shipment => Shipment.CurrentLocation).Include(Shipment => Shipment.Destination).SingleOrDefault(Shipment => Shipment.TrackingNumber == TrackingNumber);
        }

        private bool IsExist(Shipment Shipment)
        {
            return Shipment != null;
        }

        private string TrackingRecordToStatement(TrackingRecord TrackingRecord)
        {
            switch ((TrackingRecordType)TrackingRecord.Type)
            {
                case TrackingRecordType.Registered:
                    return Resources.Views.Main.RegisteredTrackingStatement;
                case TrackingRecordType.Departed:
                    return Resources.Views.Main.DepartedTrackingStatement.Replace("_LocationName_", TrackingRecord.Location.LocationName);
                case TrackingRecordType.Arrived:
                    return Resources.Views.Main.ArrivedTrackingStatement.Replace("_LocationName_", TrackingRecord.Location.LocationName);
                case TrackingRecordType.WaitingCollection:
                    return Resources.Views.Main.WaitingCollectionTrackingStatement;
                case TrackingRecordType.Collected:
                    return Resources.Views.Main.CollectedTrackingStatement.Replace("_SignedBy_", TrackingRecord.SignedBy);
                case TrackingRecordType.Updated:
                    return Resources.Views.Main.UpdatedTrackingStatement;
                default:
                    return null;
            };
        }

        private TrackShipmentViewModel GenerateTrackShipmentViewModel(Shipment Shipment)
        {
            var ViewModel = new TrackShipmentViewModel
            {
                TrackingNumber = Shipment.TrackingNumber,
                Status = StatusCodeToString((Status)Shipment.Status),
                Source = LocationToString(Shipment.Source),
                Destination = LocationToString(Shipment.Destination),
                EstimatedDeliveryDate = GetEstimatedDeliveryDate(Shipment)
            };
            if (Shipment.Status == (byte)Status.WaitingCollection || Shipment.Status == (byte)Status.WaitingShipping)
            {
                ViewModel.CurrentLocation = LocationToString(Shipment.CurrentLocation);
                ViewModel.DistanceToDestination = CalculateDistance(Shipment.Source, Shipment.Destination).ToString();
            }
            return ViewModel;
        }

        private string GetEstimatedDeliveryDate(Shipment Shipment)
        {
            var LastDelivery = LastDeliveryBetween(Shipment.Source, Shipment.Destination);
            if (LastDelivery != null)
            {
                var LastDeliveryNumberOfDays = (LastDelivery.DateTime - LastDelivery.Shipment.DateAdded).TotalDays;
                var MinDeliveryDate = Shipment.DateAdded.AddDays(LastDeliveryNumberOfDays);
                var MaxDeliveryDate = MinDeliveryDate.AddDays(3);
                return MinDeliveryDate.ToShortDateString() + " - " + MaxDeliveryDate.ToShortDateString();
            }
            return "Unknown";
        }

        private TrackingRecord LastDeliveryBetween(Location source, Location destination)
        {
            var LastDelivery = DbContext.TrackingRecords
                .Include(TrackingRecord => TrackingRecord.Shipment)
                .Include(TrackingRecord => TrackingRecord.Shipment.Source)
                .Include(TrackingRecord => TrackingRecord.Shipment.Destination)
                .Where(TrackingRecord => TrackingRecord.Type == (byte)TrackingRecordType.Collected && TrackingRecord.Shipment.Source.Id == source.Id && TrackingRecord.Shipment.Destination.Id == destination.Id)
                .FirstOrDefault();
            return LastDelivery;
        }

        public int CalculateDistance(Location L1 , Location L2)
        {
            double lat1 = L1.Latitude;
            double lon1 = L1.longitude;
            double lat2 = L2.Latitude;
            double lon2 = L2.longitude;
            double rlat1 = Math.PI * lat1 / 180;
            double rlat2 = Math.PI * lat2 / 180;
            double theta = lon1 - lon2;
            double rtheta = Math.PI * theta / 180;
            double dist =
                Math.Sin(rlat1) * Math.Sin(rlat2) + Math.Cos(rlat1) *
                Math.Cos(rlat2) * Math.Cos(rtheta);
            dist = Math.Acos(dist);
            dist = dist * 180 / Math.PI;
            dist = dist * 60 * 1.1515;
            return Convert.ToInt32(dist * 1.609344);
        }

        private IEnumerable<TrackingRecordDto> GetShipmentTrackingRecords(Shipment Shipment)
        {
            var ShipmentTrackingRecords = DbContext.TrackingRecords.Include(TrackingRecord => TrackingRecord.Shipment).Include(TrackingRecord => TrackingRecord.Location).Where(TrackingRecord => TrackingRecord.Shipment.TrackingNumber == Shipment.TrackingNumber).ToList()
                    .Select(TrackingRecord => new TrackingRecordDto
                    {
                        DateTime = TrackingRecord.DateTime.ToString(),
                        Location = TrackingRecord.Location.City,
                        Statement = TrackingRecordToStatement(TrackingRecord)
                    });
            return ShipmentTrackingRecords;
        }

        enum Status
        {
            WaitingShipping,
            Shipping,
            WaitingCollection,
            Collected
        }

        enum TrackingRecordType
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
﻿using STS.Dtos;
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

        [Route("Main/TrackShipment/GetReports/{TrackingNumber}")]
        public ActionResult GetReports(string TrackingNumber)
        {
            var Shipment = GetShipmentByTrackingNumber(TrackingNumber);
            if (IsExist(Shipment))
            {
                return Json(GetShipmentReports(Shipment));
            }
             return HttpNotFound();
        }

        public ActionResult SetDeliveryLocation(string TrackingNumber , string ReceiverPhoneNumber)
        {
            var Shipment = GetShipmentToSetDeliveryLocation(TrackingNumber , ReceiverPhoneNumber);
            if (Shipment != null)
            {
                return View(GenerateSetDeliveryLocationViewModel(Shipment));
            }
            return HttpNotFound();
        }

        // Post: Main/Set
        [HttpPost]
        public ActionResult Set(SetDeliveryLocationViewModel ViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("SetDeliveryLocation", ViewModel);
            }
            var Shipment = GetShipmentToSetDeliveryLocation(ViewModel.TrackingNumber, ViewModel.ReceiverPhoneNumber);
            if(Shipment != null)
            {
                Shipment = SetNewDeliveryLocation(Shipment , ViewModel);
                DbContext.Reports.Add(GenerateReport(Shipment, Event.NewDeliverylocation));
                DbContext.SaveChanges();
                return Redirect("TrackShipment/" + ViewModel.TrackingNumber);
            }
            return HttpNotFound();
        }

        #region Helpers

        private Shipment SetNewDeliveryLocation(Shipment shipment, SetDeliveryLocationViewModel viewModel)
        {
            shipment.DeliveryLocationLatitude = Double.Parse(viewModel.Latitude);
            shipment.DeliveryLocationlongitude = Double.Parse(viewModel.longitude);
            return shipment;
        }

        private Shipment GetShipmentToSetDeliveryLocation(string trackingNumber , string ReceiverPhoneNumber)
        {
            var Shipment = GetShipmentByTrackingNumber(trackingNumber);
            if (IsExist(Shipment))
            {
                if (Shipment.Status == (byte)Status.WaitingCollection && Shipment.CollectionMethod == (byte)CollectionMethod.Delivery && Shipment.ReceiverPhoneNumber == ReceiverPhoneNumber)
                {
                    return Shipment;
                }
            }
            return null;
        }

        private SetDeliveryLocationViewModel GenerateSetDeliveryLocationViewModel(Shipment shipment)
        {
            var ViewModel = new SetDeliveryLocationViewModel
            {
                TrackingNumber = shipment.TrackingNumber,
                ReceiverPhoneNumber = shipment.ReceiverPhoneNumber,
                Latitude = shipment.CurrentLocation.Latitude.ToString(),
                longitude = shipment.CurrentLocation.longitude.ToString()
            };
            if (shipment.DeliveryLocationLatitude != 0 && shipment.DeliveryLocationlongitude != 0)
            {
                ViewModel.Latitude = shipment.DeliveryLocationLatitude.ToString();
                ViewModel.longitude = shipment.DeliveryLocationlongitude.ToString();
            }
            return ViewModel;
        }

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

        private string ReportToStatement(Report Report)
        {
            switch ((Event)Report.Event)
            {
                case Event.Registered:
                    return Resources.Views.Main.RegisteredTrackingStatement;
                case Event.Departed:
                    return Resources.Views.Main.DepartedTrackingStatement.Replace("_LocationName_", Report.Location.LocationName);
                case Event.Arrived:
                    return Resources.Views.Main.ArrivedTrackingStatement.Replace("_LocationName_", Report.Location.LocationName);
                case Event.WaitingCollection:
                    return Resources.Views.Main.WaitingCollectionTrackingStatement;
                case Event.Collected:
                    return Resources.Views.Main.CollectedTrackingStatement.Replace("_SignedBy_", Report.SignedBy);
                case Event.Updated:
                    return Resources.Views.Main.UpdatedTrackingStatement;
                case Event.NewDeliverylocation:
                    return Resources.Views.Main.NewDeliverylocationStatement;
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

        private Report LastDeliveryBetween(Location source, Location destination)
        {
            var LastDelivery = DbContext.Reports
                .Include(Report => Report.Shipment)
                .Include(Report => Report.Shipment.Source)
                .Include(Report => Report.Shipment.Destination)
                .Where(Report => Report.Event == (byte)Event.Collected && Report.Shipment.Source.Id == source.Id && Report.Shipment.Destination.Id == destination.Id)
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

        private IEnumerable<ReportDto> GetShipmentReports(Shipment Shipment)
        {
            var ShipmentReports = DbContext.Reports.Include(Report => Report.Shipment).Include(Report => Report.Location).Where(Report => Report.Shipment.TrackingNumber == Shipment.TrackingNumber).ToList()
                    .Select(Report => new ReportDto
                    {
                        DateTime = Report.DateTime.ToString(),
                        Location = Report.Location.City,
                        Statement = ReportToStatement(Report)
                    });
            return ShipmentReports;
        }

        private Report GenerateReport(Shipment Shipment, Event Event)
        {
            var Report = new Report
            {
                Shipment = Shipment,
                Location = Shipment.CurrentLocation,
                DateTime = DateTime.Now,
                Event = (byte)Event
            };
            return Report;
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
            Updated,
            NewDeliverylocation
        }

        enum CollectionMethod
        {
            Pickup,
            Delivery
        }

        #endregion

    }
}
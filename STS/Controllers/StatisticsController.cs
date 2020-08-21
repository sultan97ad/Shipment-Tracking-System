using STS.Dtos;
using STS.Models;
using STS.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace STS.Controllers
{
    [Authorize(Roles = "Admin")]
    public class StatisticsController : Controller
    {
        private ApplicationDbContext DbContext;

        public StatisticsController()
        {
            DbContext = ApplicationDbContext.Create();
        }

        // GET: Statistics
        public ActionResult Index()
        {
            return View(GenerateStatisticsIndexViewModel());
        }

        // GET: Statistics/NewShipmentsData
        public ActionResult NewShipmentsData()
        {
            return Json(GetNewShipmentsData() , JsonRequestBehavior.AllowGet);
        }

        // GET: Statistics/ShipmentsCollectionData
        public ActionResult ShipmentsCollectionData()
        {
            return Json(GetShipmentsCollectionData(), JsonRequestBehavior.AllowGet);
        }

        #region Helpers

        private StatisticsIndexViewModel GenerateStatisticsIndexViewModel()
        {
            var ViewModel = new StatisticsIndexViewModel
            {
                RegisteredShipments = GetRegisteredShipments(),
                ShipmentsWaitingShipping = GetShipmentsWaitingShipping(),
                ShipmentsWaitingCollection = GetShipmentsWaitingCollection(),
                CollectedShipments = GetCollectedShipments(),
                NumberOfLocations = GetNumberOfLocations().ToString(),
                NumberOfEmployees = GetNumberOfEmployees().ToString(),
                AverageEmployeesPerLocation = GetAverageEmployeesPerLocation().ToString()
            };
            return ViewModel;
        }

        private double GetAverageEmployeesPerLocation()
        {
            return GetNumberOfEmployees() / GetNumberOfLocations();
        }

        private int GetNumberOfEmployees()
        {
            return (DbContext.Users.Count() - 1);
        }

        private int GetNumberOfLocations()
        {
            return DbContext.Locations.Where(Location => Location.InService).Count();
        }

        private string GetCollectedShipments()
        {
            return DbContext.Shipments.Where(Shipment => Shipment.Status == (byte)Status.Collected).Count().ToString();
        }

        private string GetShipmentsWaitingCollection()
        {
            return DbContext.Shipments.Where(Shipment => Shipment.Status == (byte)Status.WaitingCollection).Count().ToString();
        }

        private string GetShipmentsWaitingShipping()
        {
            return DbContext.Shipments.Where(Shipment => Shipment.Status == (byte)Status.WaitingShipping ).Count().ToString();
        }

        private string GetRegisteredShipments()
        {
            return DbContext.Shipments.Count().ToString();
        }

        private IEnumerable<ChartPointDto> GetNewShipmentsData()
        {
            var ChartPoints = DbContext.Shipments.GroupBy(Shipment => DbFunctions.TruncateTime(Shipment.DateAdded)).ToList().Select(Rtn=> new ChartPointDto {
                date = ((DateTime)Rtn.Key).ToString("MM/dd/yyyy"),
                value = Rtn.Count()
            });
            return ChartPoints;
        }

        private IEnumerable<ChartPointDto> GetShipmentsCollectionData()
        {
            var ChartPoints = DbContext.Shipments.Where(Shipment => Shipment.Status == (byte)Status.Collected).GroupBy(Shipment => DbFunctions.TruncateTime(Shipment.DateAdded)).ToList().Select(Rtn => new ChartPointDto
            {
                date = ((DateTime)Rtn.Key).ToString("MM/dd/yyyy"),
                value = Rtn.Count()
            });
            return ChartPoints;
        }

        enum Status
        {
            WaitingShipping,
            Shipping,
            WaitingCollection,
            Collected
        }

        #endregion
    }
}
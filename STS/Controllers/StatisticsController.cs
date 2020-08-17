using STS.Models;
using STS.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace STS.Controllers
{
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

        #region Helpers

        private StatisticsIndexViewModel GenerateStatisticsIndexViewModel()
        {
            var ViewModel = new StatisticsIndexViewModel
            {
                RegisteredShipments = GetRegisteredShipments(),
                ShipmentsWaitingShipping = GetShipmentsWaitingShipping(),
                ShipmentsWaitingCollection = GetShipmentsWaitingCollection(),
                CollectedShipments = GetCollectedShipments()
            };
            return ViewModel;
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
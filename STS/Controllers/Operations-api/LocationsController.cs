using STS.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using STS.Resources.Api;

namespace STS.Controllers.Operations_api
{
    [Authorize(Roles = "Admin")]
    public class LocationsController : ApiController
    {
        private ApplicationDbContext DbContext;

        public LocationsController()
        {
            DbContext = ApplicationDbContext.Create();
        }

        [HttpPost]
        [Route("Operations-api/Locations/Remove/{LocationId}")]
        public IHttpActionResult Remove(int LocationId)
        {
            var Location = GetLocationById(LocationId);
            if (IsExist(Location))
            {
                if (HasShipments(Location))
                {
                    return BadRequest(Locations.HasShipments);
                }
                else 
                if (HasEmployees(Location))
                {
                    return BadRequest(Locations.HasEmployees);
                }
                else 
                if (HasComingShipments(Location))
                {
                    return BadRequest(Locations.HasComingShipments);
                }
                Location.InService = false;
                DbContext.SaveChanges();
                return Ok(Locations.LocationRemoveOperationSuccess);
            }
            return NotFound();
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

        private bool HasShipments(Location location)
        {
            return DbContext.Shipments.Include(Shipment => Shipment.CurrentLocation).Where(Shipment => Shipment.CurrentLocation.Id == location.Id).Count() != 0;
        }

        private bool HasEmployees(Location location)
        {
            return DbContext.Users.Where(User => User.EmployeeLocationId == location.Id).Count() != 0;
        }

        private bool HasComingShipments(Location location)
        {
            return DbContext.Shipments.Include(Shipment => Shipment.Destination).Where(Shipment => Shipment.Destination.Id == location.Id && Shipment.Status != (byte)Status.Collected).Count() != 0;
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

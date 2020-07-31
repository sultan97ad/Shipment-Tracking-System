using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace STS.Models
{
        public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
        {
            public DbSet<Shipment> Shipments { set; get; }
            public DbSet<Location> Locations { set; get; }
            public DbSet<TrackingRecord> TrackingRecords { set; get; }
            public ApplicationDbContext()
                : base("DefaultConnection", throwIfV1Schema: false)
            {
            }

            public static ApplicationDbContext Create()
            {
                return new ApplicationDbContext();
            }



    }
    
}
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace xlsParser.Models.Context
{
    public class DataBaseContext : DbContext
    {
        public IDbSet<Service> Services { get; set; }

        public IDbSet<Carrier> Carriers { get; set; }

        public IDbSet<Vehicle> Vehicles { get; set; }

        public IDbSet<AccountingType> AccountingTypes { get; set; }

        public IDbSet<Parking> Parkings { get; set; }

        public IDbSet<ParkingToService> ParkingToServices { get; set; }

        public IDbSet<Users> Users { get; set; }
    }

    public class DbInitialiser : CreateDatabaseIfNotExists<DataBaseContext>
    {
        protected override void Seed(DataBaseContext context)
        {
            var item1 = new AccountingType() { Title = "деньги" };
            var item2 = new AccountingType() { Title = "факт" };
            context.AccountingTypes.Add(item1);
            context.AccountingTypes.Add(item2);
            context.SaveChanges();
            base.Seed(context);
        }
    }
}
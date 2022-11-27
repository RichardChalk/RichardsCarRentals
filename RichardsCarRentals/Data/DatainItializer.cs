using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCoreCodeFirstTogether.Data
{
    public class DatainItializer
    {
        public void MigrateAndSeed(ApplicationDbContext dbContext)
        {
            dbContext.Database.Migrate();
            SeedCars(dbContext);
            dbContext.SaveChanges();
        }

        private void SeedCars(ApplicationDbContext dbContext)
        {
            if (!dbContext.Cars.Any(c => c.LicensePlate == "ABC123"))
            {
                dbContext.Cars.Add(new Car
                {
                    Make = "Kia",
                    Model = "Sportage",
                    ManufacturingYear = 2019,
                    LicensePlate = "ABC123"
                });
            }
            if (!dbContext.Cars.Any(c => c.LicensePlate == "XYZ789"))
            {
                dbContext.Cars.Add(new Car
                {
                    Make = "Ford",
                    Model = "Sierra",
                    ManufacturingYear = 2010,
                    LicensePlate = "XYZ789"
                });
            }
            if (!dbContext.Cars.Any(c => c.LicensePlate == "MNO567"))
            {
                dbContext.Cars.Add(new Car
                {
                    Make = "Ferrari",
                    Model = "Testarossa",
                    ManufacturingYear = 1998,
                    LicensePlate = "MNO567"
                });
            }
        }
    }
}

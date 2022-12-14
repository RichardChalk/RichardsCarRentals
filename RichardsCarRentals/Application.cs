using EFCoreCodeFirstTogether.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// ÖVNING ÖVNING ÖVNING ÖVNING ÖVNING ÖVNING ÖVNING ÖVNING ÖVNING ÖVNING
// ÖVNING ÖVNING ÖVNING ÖVNING ÖVNING ÖVNING ÖVNING ÖVNING ÖVNING ÖVNING
// ÖVNING ÖVNING ÖVNING ÖVNING ÖVNING ÖVNING ÖVNING ÖVNING ÖVNING ÖVNING
// ÖVNING ÖVNING ÖVNING ÖVNING ÖVNING ÖVNING ÖVNING ÖVNING ÖVNING ÖVNING

// REFACTORING REFACTORING REFACTORING REFACTORING REFACTORING REFACTORING
// REFACTORING REFACTORING REFACTORING REFACTORING REFACTORING REFACTORING
// REFACTORING REFACTORING REFACTORING REFACTORING REFACTORING REFACTORING
// REFACTORING REFACTORING REFACTORING REFACTORING REFACTORING REFACTORING

// OBS: FACIT FINNS EJ! Denna övning ska ni lösa på bästa sätt!
// 1: Skapa en 'Build' klass
// 2: Skapa en klass som heter "MainMenu"
// 3: Skapa en klass som heter 'BookingController'
// 4: CreateBooking() & ListBookings() ska flyttas till klassen 'BookingController'
// 5: 'CreateBooking' metoden är enorm (SRP!!!).
//      Splittra den så det blir många små metoder istället


namespace EFCoreCodeFirstTogether
{
    public class Application
    {
        public void Run()
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile($"appsettings.json", true, true);
            var config = builder.Build();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>();
            var connectionString = config.GetConnectionString("DefaultConnection");
            options.UseSqlServer(connectionString);

            using (var dbContext = new ApplicationDbContext(options.Options))
            {
                var dataInitiaizer = new DatainItializer();
                dataInitiaizer.MigrateAndSeed(dbContext);

                // Migrate har flyttats till dataInitializer class (BEST PRACTISE)
                // dbContext.Database.Migrate();

                while (true)
                {
                    var sel = MainMenu(dbContext);

                    if (sel == 0)
                    {
                        break;
                    }
                }
            }
        }
        public int MainMenu(ApplicationDbContext dbContext)
        {
            Console.Clear();
            Console.WriteLine("1: Rent a car");
            Console.WriteLine("2: Show rented cars");
            Console.WriteLine("0: Exit");

            var sel = Convert.ToInt32(Console.ReadLine());

            switch (sel)
            {
                case 1:
                    CreateBooking(dbContext);
                    break;
                case 2:
                    ListBookings(dbContext);
                    break;
                case 0:
                    break;

                default:
                    break;
            }

            return sel;
        }

        public void CreateBooking(ApplicationDbContext dbContext)
        {
            var bookingToCreate = new Booking();

            // Check what the guest wants
            Console.Clear();
            Console.WriteLine(" How many days would you like to rent the car?");
            int numberOfDays = Convert.ToInt32(Console.ReadLine());

            // Keep asking for a valid date in the future
            bookingToCreate.DateStart = new DateTime(2001, 01, 01, 23, 59, 59);
            while (bookingToCreate.DateStart < DateTime.Now.Date)
            {
                Console.WriteLine("\n From which date would you like your booking to start from? (yyyy-mm-dd)");
                bookingToCreate.DateStart = Convert.ToDateTime(Console.ReadLine());
            }

            // set dateEnd
            if (numberOfDays == 1) bookingToCreate.DateEnd = bookingToCreate.DateStart;
            else if (numberOfDays > 1) bookingToCreate.DateEnd = bookingToCreate.DateStart.AddDays(numberOfDays);

            // Now we need to create a list of available cars for the user to choose from.
            // Lets start by making a list of ALL the dates included in the new booking
            List<DateTime> newBookingAllDates = new List<DateTime>();
            for (var dt = bookingToCreate.DateStart; dt <= bookingToCreate.DateEnd; dt = dt.AddDays(1))
            {
                newBookingAllDates.Add(dt);
            }

            // Lets loop through all the cars in the system 
            // and check if they have booking dates that block our new booking,
            List<Car> availableCars = new List<Car>();

            foreach (var car in dbContext.Cars.ToList())
            {
                bool carIsFree = true;
                foreach (var booking in dbContext.Bookings.Include(b => b.CarBooking).Where(b => b.CarBooking == car))
                {
                    for (var dt = booking.DateStart; dt <= booking.DateEnd; dt = dt.AddDays(1))
                    {
                        if (newBookingAllDates.Contains(dt))
                        {
                            carIsFree = false;

                        }
                    }

                    // if the car is already booked on the date of the new booking...
                    // we dont need to check any of the other bookings... the car isnt available
                    // so we break out of the loop and check the next car
                    if (!carIsFree)
                    {
                        break;
                    }
                }


                // finally if the car is free we can add it to our list of available cars
                if (carIsFree)
                {
                    availableCars.Add(car);
                }
            }

            // Lets show the bookings details
            Console.Clear();
            Console.WriteLine(" Your booking details");
            Console.WriteLine(" ==================================================================");
            Console.WriteLine(" Start\t\tEnd\t\tNo. of days");
            Console.WriteLine($" {bookingToCreate.DateStart.ToShortDateString()}\t{bookingToCreate.DateEnd.ToShortDateString()}\t{numberOfDays}");

            // FAIL! Display if no avaialable cars
            if (availableCars.Count() < 1)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n\n There are no cars available for these dates. Please try another date");
                Console.ForegroundColor = ConsoleColor.Gray;

                Console.WriteLine(" Press any key to continue");
                Console.ReadLine();
                return; // end method
            }
            else
            {
                // Display the available cars
                Console.WriteLine("\n\n\n These cars are available for booking");
                Console.WriteLine("\n License Plate\tMake\t\tModel");
                Console.WriteLine(" ==================================================================");

                foreach (var car in availableCars.OrderBy(r => r.LicensePlate))
                {
                    Console.WriteLine($" {car.LicensePlate}\t\t{car.Make}\t\t{car.Model}");
                    Console.WriteLine(" ------------------------------------------------------------------");
                }
            }

            // Assign the car the user chose to the booking 
            Console.WriteLine("\n Please choose a car (license plate) from the available cars");
            string carLicensPlateForBooking = Console.ReadLine();
            bookingToCreate.CarBooking = dbContext.Cars
                .Where(c => c.LicensePlate == carLicensPlateForBooking)
                .FirstOrDefault();

            dbContext.Add(bookingToCreate);
            dbContext.SaveChanges();

            // SUCCESS!
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Clear();
            Console.WriteLine(" Booking successful!");
            Console.WriteLine(" ==============================================================================");
            Console.WriteLine(" Start\t\tEnd\t\tNo. of days");
            Console.WriteLine($" {bookingToCreate.DateStart.ToShortDateString()}\t{bookingToCreate.DateEnd.ToShortDateString()}\t{numberOfDays}");
            Console.ForegroundColor = ConsoleColor.Gray;

            Console.WriteLine("\n Press any key to continue");
            Console.ReadLine();
        }

        private void ListBookings(ApplicationDbContext dbContext)
        {
            Console.Clear();
            Console.WriteLine(" Bookings information");
            Console.WriteLine("\n Id\tStart\t\tEnd\t\tCar");
            Console.WriteLine(" =============================================");


            // If no bookings in system
            if (dbContext.Bookings == null)
            {
                Console.WriteLine(" No active bookings in database. Create a booking first");
            }
            else
            {
                // Lets join tables including, 
                var bookingInclAllData = dbContext.Bookings
                    .Include(b => b.CarBooking);

                // display active bookings
                foreach (var booking in bookingInclAllData.OrderBy(b => b.Id))
                {
                    Console.WriteLine($" {booking.Id}\t{booking.DateStart.ToShortDateString()}\t{booking.DateEnd.ToShortDateString()}\t{booking.CarBooking.LicensePlate}");

                    //Console.WriteLine(" ------------------------------------------------------------------------------");
                }
            }
            Console.WriteLine("\n Press any key to continue");
            Console.ReadLine();
        }
    }
}

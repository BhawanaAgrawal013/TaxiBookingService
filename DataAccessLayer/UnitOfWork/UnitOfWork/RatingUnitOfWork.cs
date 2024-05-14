using DataAccessLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class RatingUnitOfWork : IRatingUnitOfWork
    {
        private readonly TaxiBookingContext _dBContext;

        public RatingUnitOfWork(TaxiBookingContext dbcontext)
        {
            _dBContext = dbcontext;

            Users = new UserRepository(_dBContext);
            Bookings = new BookingRepository(_dBContext);
            Drivers = new DriverRepository(_dBContext);
            Ratings = new RatingRepository(_dBContext);
        }
        public IUserRepository Users { get; }
        public IBookingRepository Bookings { get; }
        public IDriverRepository Drivers { get; }
        public IRatingRepository Ratings { get; }
        public void Complete()
        {
            _dBContext.SaveChanges();
        }

    }
}

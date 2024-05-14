using Microsoft.Identity.Client;

namespace DataAccessLayer
{
    public class BookingService : IBookingService
    {
        private readonly IBookingUnitOfWork _unitOfWork;
        private readonly IPaymentService _paymentService;
        private readonly IDriverService _driverService;
        private readonly ICancelledBookingService _cancelledBookingService;
        private readonly ILog _log;

        public BookingService(IBookingUnitOfWork unitOfWork, IPaymentService paymentService, ICancelledBookingService cancelledBookingService, ILog log, IDriverService driverService)
        {
            _unitOfWork = unitOfWork;
            _paymentService = paymentService;
            _cancelledBookingService = cancelledBookingService;
            _log = log;
            _driverService = driverService;
        }

        private BookingDTO CreatingBookingDTO(Booking booking)
        {
            BookingDTO bookingdto = new()
            {
                DropLocation = booking.DropoffLocation,
                PickupLocation = booking.PickupLocation,
                VehiclePreference = booking.VehiclePreference,
                PickupTime = booking.PickupDate,
                DropTime = booking.DropOffTime,
                Payment = booking.Payment.amount,
                PaymentMode = booking.Payment.PaymentMode.Mode,
                UserEmail = booking.User.Email,
                PhoneNumber = booking.PhoneNumber,
                Id = booking.Id
            };

            return bookingdto;
        }

        private BookingResponse GetBookingDM(Booking booking)
        {
            BookingResponse bookingDM = new()
            {
                PickupLocation = booking.PickupLocation,
                PickupTime = booking.PickupDate,
                DropTime = booking.DropOffTime,
                DropLocation = booking.DropoffLocation,
                Payment = booking.Payment.amount,
                PaymentMode = booking.Payment.PaymentMode.Mode,
                PhoneNumber = booking.PhoneNumber,
                Rating = booking.Rating.Ratings,
                UserRating = booking.UserRating.Ratings,
            };
            return bookingDM;
        }

        public List<BookingResponse> History(string email)
        {
            try
            {
                List<Booking> bookings = _unitOfWork.Bookings.GetAll().FindAll(item => item.User.Email == email);

                List<BookingResponse> bookingDMs = new List<BookingResponse>();

                foreach (var booking in bookings)
                {
                    decimal Rating = 0;

                    BookingStatus bookingStatus = _unitOfWork.BookingsStatus.Get(e => e.Id == booking.StatusId);

                    if (booking.RatingId == null) Rating = 0;
                    else Rating = booking.Rating.Ratings;

                    BookingResponse bookingDM = GetBookingDM(booking);

                    bookingDM.Status = bookingStatus.Status;
                    bookingDM.Rating = Rating;
                    bookingDM.UserName = booking.User.FirstName + " " + booking.User.LastName;
                    bookingDM.DriverName = booking.Driver.User.FirstName + " " + booking.Driver.User.LastName;
                    bookingDMs.Add(bookingDM);
                }

                _log.Information("BookingRep: Booking history fetched successfully");
                return bookingDMs;
            }
            catch (Exception ex)
            {
                _log.Error(ex.StackTrace);
                return null;
            }
        }

        public List<BookingResponse> Complaints()
        {
            try
            {
                List<Booking> bookings = _unitOfWork.Bookings.GetAll().FindAll(e => e.Payment.Status == Statuses.Declined.ToString());
                if (bookings.Count == 0) return null;

                List<BookingResponse> bookingDMs = new List<BookingResponse>();


                foreach (var booking in bookings)
                {
                    decimal Rating = 0;

                    BookingStatus bookingStatus = _unitOfWork.BookingsStatus.Get(e => e.Id == booking.StatusId);

                    if (booking.RatingId == null) Rating = 0;
                    else Rating = booking.Rating.Ratings;

                    BookingResponse bookingDM = GetBookingDM(booking);

                    bookingDM.Status = bookingStatus.Status;
                    bookingDM.Rating = Rating;
                    bookingDM.UserName = booking.User.FirstName + " " + booking.User.LastName;
                    bookingDM.DriverName = booking.Driver.User.FirstName + " " + booking.Driver.User.LastName; 
                    bookingDMs.Add(bookingDM);
                }

                _log.Information("BookingRep: Booking history fetched successfully");
                return bookingDMs;

            }
            catch (Exception ex)
            {
                _log.Error(ex.StackTrace);
                return null;
            }
        }
        public BookingDTO Search(int id)
        {
            try
            {
                Booking booking = _unitOfWork.Bookings.GetAll().Where(e => e.Id == id).FirstOrDefault();

                BookingStatus bookingStatus= _unitOfWork.BookingsStatus.Get(e => e.Id == booking.StatusId);

                BookingDTO bookingdto = CreatingBookingDTO(booking);
                bookingdto.Status = bookingStatus.Status;

                if (booking.DriverId == null) bookingdto.DriverEmail = null;
                else bookingdto.DriverEmail = booking.Driver.User.Email;

                _log.Information("BookRepo: Booking found");
                return bookingdto;
            }
            catch (Exception ex)
            {
                _log.Error(ex.StackTrace);
                return null;
            }
        }

        private bool CheckPayments(int id)
        {
            try
            {
                var payments = _unitOfWork.Bookings.GetAll().FindAll(e => e.User.Id == id).Where(e => e.Payment.Status == Statuses.Declined.ToString()).Select(e => new Payment {amount = e.Payment.amount, Status = e.Payment.Status });

                foreach(var payment in payments)
                {
                    if (payment.Status == Statuses.Declined.ToString()) return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _log.Error(ex.StackTrace);
                return true;
            }
        }

        public int Add(BookingDTO bookingDTO)
        {
            try
            {
                User user = _unitOfWork.Users.Get(item => item.Email == bookingDTO.UserEmail);

                if (CheckPayments(user.Id.Value)) return 0;

                if (bookingDTO.PaymentMode == PaymentModes.Wallet.ToString() && user.wallet < bookingDTO.Payment) return 0;

                BookingStatus status = _unitOfWork.BookingsStatus.Get(item => item.Status == Statuses.Requested.ToString());

                int paymentId = CreatePayment(bookingDTO);

                if (paymentId == 0 || status == null || user == null) return 0;

                Booking booking = new()
                {
                    VehiclePreference = bookingDTO.VehiclePreference,
                    PickupLocation = bookingDTO.PickupLocation,
                    DropoffLocation = bookingDTO.DropLocation,
                    PickupDate = bookingDTO.PickupTime,
                    DropOffTime = bookingDTO.DropTime,
                    PaymentId = paymentId,
                    PhoneNumber = bookingDTO.PhoneNumber,
                    StatusId = status.Id,
                    UserId = user.Id,
                    CreatedAt = DateTime.Now,
                    isDelete = false,
                    CreatedBy = user.Id
                };

                _unitOfWork.Bookings.Add(booking);
                _unitOfWork.Complete();

                _log.Information("BookRepo: Booking added successfully");
                return booking.Id;
            }
            catch (Exception ex)
            {
                _log.Error(ex.StackTrace);
                return 0;
            }
        }

        public bool PostRequest(int id, string email)
        {
            try
            {
                Booking booking = _unitOfWork.Bookings.GetAll().Find(e => e.Id == id);
                if (booking == null) return false;

                booking.DriverId = _unitOfWork.Drivers.GetAll().Find(e => e.User.Email ==  email).Id;

                _unitOfWork.Complete();

                _log.Information($"Booking request sent to the driver {email}");

                //driver accepted
                

                return true;
            }
            catch (Exception ex)
            {
                _log.Error(ex.StackTrace);
                return false;
            }
        }

        public int CreatePayment(BookingDTO bookingdto)
        {
            try
            {
                PaymentDTO payment = new PaymentDTO();
                payment.Date = DateTime.Now;
                payment.Amount = bookingdto.Payment;
                payment.Mode = bookingdto.PaymentMode;
                payment.UserEmail = bookingdto.UserEmail;

                int id = _paymentService.Add(payment);

                _log.Information("BookRepo: Payment created successfully");
                return id;
            }
            catch (Exception ex)
            {
                _log.Error(ex.StackTrace);
                return 0;
            }
        }

        public bool GetDriver(int id)
        {
            try
            {
                Booking booking = _unitOfWork.Bookings.GetAll().Where(e => e.Id == id).FirstOrDefault();

                List<DriverDTO> drivers = _driverService.Display().OrderBy(e => e.Rating).ToList();
                int index = 0;

                while(booking.BookingStatus.Status != Statuses.Accepted.ToString() || index < drivers.Count)
                {
                    int driverId = _unitOfWork.Drivers.GetAll().Find(e => e.User.Email == drivers[index].Email).Id;
                    booking.DriverId = driverId;
                    //20 sec
                    index++; 
                } 

                return true;
            }
            catch (Exception ex)
            {
                _log.Error(ex.StackTrace);
                return false;
            }
        }

        public BookingDTO AcceptRequest(int id, string email)
        {
            try
            {
                Booking booking = _unitOfWork.Bookings.GetAll().Where(e => e.Id == id).FirstOrDefault();

                if (booking.StatusId != (int)Statuses.Requested) return null;

                booking.DriverId = _unitOfWork.Drivers.Get(e => e.User.Email == email).Id;

                if (!_unitOfWork.Drivers.Find(i => i.Id == booking.DriverId).isApproved) return null;

                booking.StatusId = _unitOfWork.BookingsStatus.Get(e => e.Status == Statuses.Accepted.ToString()).Id;

                booking.ModifiedBy = _unitOfWork.Users.Get(e => e.Email == email).Id;

                booking.ModifiedAt = DateTime.Now;

                BookingDTO bookingdto = CreatingBookingDTO(booking);

                _unitOfWork.Bookings.Update(booking);
                _unitOfWork.Complete();

                _log.Information("BookRepo: Booking accepted succesffuly");
                return bookingdto;
            }
            catch (Exception ex)
            {
                _log.Error(ex.StackTrace);
                return null;
            }
        }

        public bool CancelRequest(int id)
        {
            try
            {
                Booking booking = _unitOfWork.Bookings.GetAll().Where(e => e.Id == id).FirstOrDefault();

                BookingStatus bookingStatus = _unitOfWork.BookingsStatus.Get(e => e.Status == Statuses.Requested.ToString());

                if (booking.StatusId == bookingStatus.Id)
                {
                    if (!_unitOfWork.Users.Exists(i => i.Id == booking.UserId)) return false;

                    booking.StatusId = _unitOfWork.BookingsStatus.Get(e => e.Status == Statuses.Cancelled.ToString()).Id;

                    booking.DriverId = null;

                    booking.ModifiedBy = booking.UserId;

                    booking.ModifiedAt = DateTime.Now;

                    BookingDTO bookingdto = CreatingBookingDTO(booking);

                    _paymentService.PaymentCancel(booking.PaymentId.Value);

                    _unitOfWork.Bookings.Update(booking);
                    _unitOfWork.Complete();

                    _log.Information("BookRepo: Booking accepted succesffuly");
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _log.Error(ex.StackTrace);
                return false;
            }
        }

        public bool ArrivedAtLocation(int id)
        {
            try
            {
                Booking booking = _unitOfWork.Bookings.GetAll().Find(e => e.Id == id);

                if(booking == null || booking.StatusId == (int)Statuses.Cancelled || booking.StatusId == (int)Statuses.Declined) return false;

                booking.StatusId = _unitOfWork.BookingsStatus.Get(e => e.Status == Statuses.Arrived.ToString()).Id;

                if(booking.DriverId == null) return false;

                booking.ModifiedBy = booking.Driver.UserId;

                booking.ModifiedAt = DateTime.Now;

                _unitOfWork.Bookings.Update(booking);
                _unitOfWork.Complete();
                return true;
            }
            catch (Exception ex)
            {
                _log.Error(ex.StackTrace);
                return false;
            }
        }

        public BookingDTO DeclineRequest(int id, string reason)
        {
            try
            {
                Booking booking = _unitOfWork.Bookings.GetAll().Find(e => e.Id == id);

                if (booking.StatusId == (int)Statuses.Arrived || booking.StatusId == (int)Statuses.Completed) return null;

                if (_cancelledBookingService.Add(booking.Id, reason, booking.User.Email))
                {
                    booking.StatusId = _unitOfWork.BookingsStatus.Get(e => e.Status == Statuses.Declined.ToString()).Id;

                    booking.ModifiedBy = booking.UserId;

                    booking.ModifiedAt = DateTime.Now;

                    BookingDTO bookingdto = CreatingBookingDTO(booking);

                    _paymentService.PaymentCancel(booking.PaymentId.Value);

                    _unitOfWork.Bookings.Update(booking);
                    _unitOfWork.Complete();

                    _log.Information("BookRepo: Booking Declined successfully");
                    return bookingdto;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                _log.Error(ex.StackTrace);
                return null;
            }
        }

        public BookingDTO CompleteRide(int id)
        {
            try
            {
                Booking booking = _unitOfWork.Bookings.GetAll().Find(e => e.Id == id);

                if(booking == null || booking.StatusId == (int)Statuses.Cancelled || booking.StatusId == (int)Statuses.Declined) return null;

                booking.StatusId = _unitOfWork.BookingsStatus.Get(e => e.Status == Statuses.Completed.ToString()).Id;

                booking.ModifiedBy = booking.Driver.User.Id;

                booking.ModifiedAt = DateTime.Now;

                BookingDTO bookingdto = CreatingBookingDTO(booking);

                _unitOfWork.Bookings.Update(booking);
                _unitOfWork.Complete();

                _log.Information("BookRepo: Booking Completed successfully");
                return bookingdto;
            }
            catch (Exception ex)
            {
                _log.Error(ex.StackTrace);
                return null;
            }
        }
    }
}

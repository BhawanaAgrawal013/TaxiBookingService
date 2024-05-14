namespace DataAccessLayer
{
    public class CancelledBookingService : ICancelledBookingService
    {
        private readonly ICancellationBookingUOW _unitOfWork;
        private readonly ILog _log;

        public CancelledBookingService(ICancellationBookingUOW unitOfWork, ILog log)
        {
            _unitOfWork = unitOfWork;
            _log = log;
        }

        public bool Add(int id, string reason, string userEmail)
        {
            try
            {
                CancellationReason cancellationReason = _unitOfWork.CancellationReasons.Get(e => e.Reason == reason);

                User user = _unitOfWork.Users.Get(e => e.Email == userEmail);

                CancelledBooking cancelledBooking = new CancelledBooking();
                cancelledBooking.BookingId = id;
                cancelledBooking.ReasonId = cancellationReason.id;
                cancelledBooking.CreatedBy = (int)user.Id;
                cancelledBooking.isPending = true;
                cancelledBooking.CreatedAt = DateTime.Now;
                if (cancelledBooking.CreatedBy == 0) return false;

                _unitOfWork.CancellationBookings.Add(cancelledBooking);
                _unitOfWork.Complete();
                _log.Information("CancelledBooking: Booking Cancelled sucessfully");
                return true;
            }
            catch (Exception ex)
            {
                _log.Error(ex.StackTrace);
                return false;
            }
        }

        public List<CancelledBookingDTO> Search(string userEmail)
        {
            try
            {
                int userId = (int)_unitOfWork.Users.Get(e => e.Email == userEmail).Id;

                List<CancelledBooking> bookings = _unitOfWork.CancellationBookings.GetAll().Where(e => e.Booking.UserId == userId).ToList();

                List<CancelledBookingDTO> cancelledBookings = new List<CancelledBookingDTO>();

                foreach (var booking in bookings)
                {
                    CancelledBookingDTO cancelledBooking = new()
                    {
                        Amount = booking.Booking.Payment.amount,
                        IsPending = booking.isPending,
                        IsCharged = booking.CancellationReason.isCharged,
                        Id = booking.Id
                    };
                    if (cancelledBooking.IsPending) cancelledBookings.Add(cancelledBooking);
                }

                _log.Information("CancelledBooking: All the cancelled bookings are fetched");
                return cancelledBookings;
            }
            catch (Exception ex)
            {
                _log.Error(ex.StackTrace);
                return null;
            }
        }

        public bool PaidCancellationFee(int id)
        {
            try
            {
                CancelledBooking cancelledBooking = _unitOfWork.CancellationBookings.Get(e => e.Id == id);
                cancelledBooking.isPending = false;

                _unitOfWork.CancellationBookings.Update(cancelledBooking);
                _unitOfWork.Complete();
                _log.Information($"CancelledBooking: {id} booking paid successfully");
                return true;
            }
            catch (Exception ex)
            {
                _log.Error(ex.StackTrace);
                return false;
            }
        }

        public bool PayCancellationFee(string email)
        {
            try
            {
                var cancelledBookings = _unitOfWork.CancellationBookings.GetAll().FindAll(e => e.Booking.User.Email == email);

                foreach(var cancelledBooking in cancelledBookings)
                {
                    cancelledBooking.isPending = false;
                    _unitOfWork.CancellationBookings.Update(cancelledBooking);
                }

                _unitOfWork.Complete();
                _log.Information($"CancelledBooking: {email} booking paid successfully");
                return true;
            }
            catch (Exception ex)
            {
                _log.Error(ex.StackTrace);
                return false;
            }
        }
    }
}

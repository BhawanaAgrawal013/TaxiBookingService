namespace DataAccessLayer
{
    public class BookingStatusService : IBookingStatusService
    {
        private readonly IBookingStatusUnitOfWork _unitOfWork;
        private readonly ILog _log;

        public BookingStatusService(IBookingStatusUnitOfWork unitOfWork, ILog log)
        {
            _unitOfWork = unitOfWork;
            _log = log;
        }

        public List<BookingStatusResponse> Display()
        {
            try
            {
                _log.Information("BookStatus: Status fetched successfully");
                return _unitOfWork.BookingsStatus.Display(item => item.isDelete == false).Select(e => new BookingStatusResponse { Id = e.Id, Status = e.Status}).ToList();
            }
            catch (Exception ex)
            {
                _log.Error(ex.StackTrace);
                return null;
            }
        }

        public bool Add(BookingStatusDTO statusDTO)
        {
            try
            {
                if (_unitOfWork.BookingsStatus.Exists(item => item.Status == statusDTO.Status))
                {
                    BookingStatus existingStatus = _unitOfWork.BookingsStatus.Get(item => item.Status == statusDTO.Status);

                    if (existingStatus.isDelete == false)
                        return false;
                }
                User user = _unitOfWork.Users.Get(item => item.Email == statusDTO.UserEmail);

                if (user == null) { return false; }

                BookingStatus status = new()
                {
                    Status = statusDTO.Status,
                    CreatedAt = DateTime.Now,
                    isDelete = false,
                    CreatedBy = (int)user.Id
                };

                _unitOfWork.BookingsStatus.Add(status);
                _unitOfWork.Complete();

                _log.Information("BookStatus: Staus added successfully");
                return true;
            }
            catch (Exception ex)
            {
                _log.Error(ex.StackTrace);
                return false;
            }
        }

        public BookingStatusResponse Search(int id)
        {
            try
            {
                BookingStatus status = _unitOfWork.BookingsStatus.Get(item => item.Id == id);

                if (status == null || status.isDelete == true) return null;

                BookingStatusResponse bookingStatus = new()
                {
                    Id = status.Id,
                    Status = status.Status
                };

                _log.Information("BookStatus: Status searched successfully");
                return bookingStatus;
            }
            catch (Exception ex)
            {
                _log.Error(ex.StackTrace);
                return null;
            }
        }

        public bool Update(BookingStatusDTO bookingStatus, int id)
        {
            try
            {
                BookingStatus status = _unitOfWork.BookingsStatus.Get(item => item.Id == id);
                User user = _unitOfWork.Users.Get(item => item.Email == bookingStatus.UserEmail);

                if (status == null || status.isDelete) return false;

                status.Status = bookingStatus.Status;
                status.ModifiedAt = DateTime.Now;
                status.ModifiedBy = user.Id;

                _unitOfWork.BookingsStatus.Update(status);
                _unitOfWork.Complete();

                _log.Information($"BookStatus: Updated status: {bookingStatus.Status}");
                return true;
            }
            catch (Exception ex)
            {
                _log.Error(ex.StackTrace);
                return false;
            }
        }

        public bool Delete(int id)
        {
            try
            {
                BookingStatus status = _unitOfWork.BookingsStatus.Get(item => item.Id == id);

                if (status == null || status.isDelete) return false;

                status.isDelete = true;

                _unitOfWork.BookingsStatus.Update(status);
                _unitOfWork.Complete();

                _log.Information("BookStatus: Status deleted successfully");
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

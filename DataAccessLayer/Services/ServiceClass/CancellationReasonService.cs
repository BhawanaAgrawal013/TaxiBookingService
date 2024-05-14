namespace DataAccessLayer
{
    public class CancellationReasonService : ICancellationReasonService
    {
        private readonly ICancellationBookingUOW _unitOfWork;
        private readonly ILog _log;

        public CancellationReasonService(ICancellationBookingUOW unitOfWork, ILog log)
        {
            _unitOfWork = unitOfWork;
            _log = log;
        }

        public List<CancellationReasonResponse> Display()
        {
            try 
            {
                var reasons = _unitOfWork.CancellationReasons.Display(item => item.isDelete == false);
                List<CancellationReasonResponse> cancellationReasons = new List<CancellationReasonResponse>();

                foreach (var reason in reasons)
                {
                    CancellationReasonResponse cancellationReason = new()
                    {
                        Id = reason.id,
                        Reason = reason.Reason,
                        IsCharged = reason.isCharged
                    };
                    cancellationReasons.Add(cancellationReason);
                }

                _log.Information("CancelReasonRepo: Reasons displayed successfully");
                return cancellationReasons;
            }
            catch (Exception ex)
            {
                _log.Error(ex.StackTrace);
                return null;
            }
        }

        public bool Add(CancellationReasonDTO cancellationReason)
        {
            try
            {
                if (!_unitOfWork.Users.Exists(item => item.Email == cancellationReason.UserEmail)) return false;

                CancellationReason existingReason = _unitOfWork.CancellationReasons.Get(item => item.Reason == cancellationReason.Reason);

                if (existingReason.isDelete == false) return false;

                User user = _unitOfWork.Users.Get(item => item.Email == cancellationReason.UserEmail);

                CancellationReason reason = new()
                {
                    Reason = cancellationReason.Reason,
                    CreatedAt = DateTime.Now,
                    isDelete = false,
                    CreatedBy = (int)user.Id
                };

                _unitOfWork.CancellationReasons.Add(reason);
                _unitOfWork.Complete();

                _log.Information("CancelReasonRepo: Reasons added successfully");
                return true;
            }
            catch (Exception ex)
            {
                _log.Error(ex.StackTrace);
                return false;
            }
        }

        public bool Update(CancellationReasonDTO cancellationReason, int id)
        {
            try
            {
                CancellationReason reason = _unitOfWork.CancellationReasons.Get(item => item.id == id);
                User user = _unitOfWork.Users.Get(item => item.Email == cancellationReason.UserEmail);

                if (reason == null) return false;

                if (reason.isDelete) return false;

                reason.Reason = cancellationReason.Reason;
                reason.ModifiedAt = DateTime.Now;
                reason.ModifiedBy = user.Id;

                _unitOfWork.CancellationReasons.Update(reason);
                _unitOfWork.Complete();

                _log.Information("CancelReasonRepo: Reason updated successfully");
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
                CancellationReason reason = _unitOfWork.CancellationReasons.Get(item => item.id == id);

                if (reason == null) return false;
                if (reason.isDelete) return false;

                reason.isDelete = true;

                _unitOfWork.CancellationReasons.Update(reason);
                _unitOfWork.Complete();

                _log.Information("CancelReasonRepo: Reason Deleted successfully");
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

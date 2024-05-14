using NLog.Fluent;

namespace DataAccessLayer
{
    public class PaymentModeService : IPaymentModeService
    {
        private readonly IPaymentUnitOfWork _unitOfWork;
        private readonly ILog _log;

        public PaymentModeService(IPaymentUnitOfWork unitOfWork, ILog log)
        {
            _unitOfWork = unitOfWork;
            _log = log;
        }

        public List<PaymentModeResponse> Display()
        {
            try
            {
                _log.Information("Payment Modes fetched successfully");
                return _unitOfWork.PaymentModes.Display(item => item.isDelete == false).Select(e => new PaymentModeResponse { Id = e.Id, PaymentMode = e.Mode}).ToList();
            }
            catch (Exception ex)
            {
                _log.Error(ex.StackTrace);
                return null;
            }
        }

        public bool Add(PaymentModeDTO newMode)
        {
            try
            {
                bool userExists = _unitOfWork.Users.Exists(item => item.Email == newMode.UserEmail);

                if (!userExists) return false;

                if (_unitOfWork.PaymentModes.Exists(item => item.Mode == newMode.PaymentMode))
                {
                    PaymentMode existingmode = _unitOfWork.PaymentModes.Get(item => item.Mode == newMode.PaymentMode);

                    if (existingmode.isDelete == false)
                    {
                        return false;
                    }
                }
                User user = _unitOfWork.Users.Get(item => item.Email == newMode.UserEmail);

                PaymentMode mode = new()
                {
                    Mode = newMode.PaymentMode,
                    CreatedAt = DateTime.Now,
                    isDelete = false,
                    CreatedBy = (int)user.Id
                };

                _unitOfWork.PaymentModes.Add(mode);
                _unitOfWork.Complete();

                _log.Information("Payment Mode added successfully");
                return true;
            }
            catch (Exception ex)
            {
                _log.Error(ex.StackTrace);
                return false;
            }
        }

        public PaymentModeResponse Search(int id)
        {
            try
            {
                PaymentMode type = _unitOfWork.PaymentModes.Get(item => item.Id == id);

                if (type.isDelete || type == null) return null;

                PaymentModeResponse paymentMode = new()
                {
                    Id = type.Id,
                    PaymentMode = type.Mode
                };

                _log.Information("Payment mode searched successfully");
                return paymentMode;
            }
            catch (Exception ex)
            {
                _log.Error(ex.StackTrace);
                return null;
            }
        }

        public bool Update(PaymentModeDTO updatedMode, int id)
        {
            try
            {
                PaymentMode mode = _unitOfWork.PaymentModes.Get(item => item.Id == id);
                User user = _unitOfWork.Users.Get(item => item.Email == updatedMode.UserEmail);

                if (mode == null) return false;

                if (mode.isDelete) return false;

                mode.Mode = updatedMode.PaymentMode;
                mode.ModifiedAt = DateTime.Now;
                mode.ModifiedBy = user.Id;

                _unitOfWork.PaymentModes.Update(mode);
                _unitOfWork.Complete();

                _log.Information("Payment Mode updated successfully");
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
                PaymentMode mode = _unitOfWork.PaymentModes.Get(item => item.Id == id);

                if (mode == null) return false;
                if (mode.isDelete) return false;

                mode.isDelete = true;

                _unitOfWork.PaymentModes.Update(mode);
                _unitOfWork.Complete();

                _log.Information("Payment Mode deleted successfully");
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

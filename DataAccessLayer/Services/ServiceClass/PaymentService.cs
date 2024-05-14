using DataAccessLayer.Entity;
using DataAccessLayer.ResponseModels;

namespace DataAccessLayer
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentUnitOfWork _unitOfWork;
        private readonly ILog _log;

        public PaymentService(IPaymentUnitOfWork unitOfWork, ILog log)
        {
            _unitOfWork = unitOfWork;
            _log = log;
        }

        public int Add(PaymentDTO paymentDTO)
        {
            try
            {   
                PaymentMode paymentMode = _unitOfWork.PaymentModes.Get(item => item.Mode == paymentDTO.Mode);

                if (paymentMode.isDelete == true) return 0;

                User user = _unitOfWork.Users.Get(item => item.Email == paymentDTO.UserEmail);

                if(user == null) return 0;

                Payment payment = new()
                {
                    ModeId = paymentMode.Id,
                    amount = paymentDTO.Amount,
                    date = DateTime.Now,
                    Status = Statuses.Requested.ToString(),
                    CreatedAt = paymentDTO.Date,
                    isDelete = false,
                    CreatedBy = (int)user.Id
                };

                _unitOfWork.Payments.Add(payment);
                _unitOfWork.Complete();

                _log.Information("Payment for booking added successfully");
                return payment.id;
            }
            catch (Exception ex)
            {
                _log.Error(ex.StackTrace);
                return 0;
            }
        }

        public bool AddWallet(int amount, string email)
        {
            try
            {
                User user = _unitOfWork.Users.Get(e => e.Email == email);
                user.wallet += amount;

                _unitOfWork.Complete();
                _log.Information("Wallet is incremented with amount");
                return true;
            }
            catch (Exception ex)
            {
                _log.Error(ex.StackTrace);
                return false;
            }
        }

        public bool PaymentVerify(int id, PaymentVerificationModel verificationModel)
        {
            try
            {
                Booking booking = _unitOfWork.Bookings.GetAll().Find(e => e.Id == id);
                if (booking == null) return false;

                Payment payment = _unitOfWork.Payments.GetAll().Find(e => e.id == booking.PaymentId);

                payment.ModeId = _unitOfWork.PaymentModes.Get(e => e.Mode == verificationModel.PaymentMode).Id;
                payment.Remark = verificationModel.Remark;

                if (!verificationModel.IsVerify)
                {
                    payment.Status = Statuses.Declined.ToString();
                    _unitOfWork.Payments.Update(payment);
                    _unitOfWork.Complete();

                    _log.Information("Payment has been declined");
                    return true;
                }

                if (booking.Payment.PaymentMode.Mode == PaymentModes.Wallet.ToString())
                {
                    if (!DeductWallet(booking))
                    {
                        return false;
                    }
                }

                payment.Status = Statuses.Completed.ToString();

                _unitOfWork.Payments.Update(payment);
                _unitOfWork.Complete();

                _log.Information("Payment has been verified");
                return true;
            }
            catch (Exception ex)
            {
                _log.Error(ex.StackTrace);
                return false;
            }
        }

        private bool DeductWallet(Booking booking)
        {
            try
            {
                User user = _unitOfWork.Users.Get(e => e.Id == booking.UserId);
                
                User driver = _unitOfWork.Users.Get(e => e.Id == booking.Driver.UserId);
                
                user.wallet -= booking.Payment.amount;
                
                driver.wallet += booking.Payment.amount;

                _unitOfWork.Complete();
                _log.Information("Payment for booking done successfully");
                return true;
            }
            catch (Exception ex)
            {
                _log.Error(ex.StackTrace);
                return false;
            }
        }

        public void CheckStatus(int id)
        {
            try
            {
                Payment payment = _unitOfWork.Payments.GetAll().Find(e => e.id == id);
                if(payment.Status != Statuses.Completed.ToString()) 
                {
                    payment.Status = Statuses.Declined.ToString();

                    _unitOfWork.Payments.Update(payment);
                    _unitOfWork.Complete();

                    _log.Information($"Status: {payment.Status}");
                }
            }
            catch (Exception ex)
            {
                _log.Error(ex.StackTrace);
            }
        }

        public void PaymentCancel(int id)
        {
            try
            {
                Payment payment = _unitOfWork.Payments.GetAll().Find(e => e.id == id);
                payment.Status = Statuses.Cancelled.ToString();

                _unitOfWork.Payments.Update(payment);
                _unitOfWork.Complete();

                _log.Information($"Status: {payment.Status}");
            }
            catch (Exception ex)
            {
                _log.Error(ex.StackTrace);
            }
        }

        public bool Update(PaymentDTO paymentDTO, int id)
        {
            try
            {
                if (!_unitOfWork.Users.Exists(item => item.Email == paymentDTO.UserEmail)) return false;

                PaymentMode paymentMode = _unitOfWork.PaymentModes.Get(item => item.Mode == paymentDTO.Mode);

                if (paymentMode.isDelete == true) return false;

                User user = _unitOfWork.Users.Get(item => item.Email == paymentDTO.UserEmail);

                Payment payment = _unitOfWork.Payments.GetAll().Find(item => item.id == id);

                payment.amount = paymentDTO.Amount;
                payment.date = paymentDTO.Date;
                payment.ModeId = paymentMode.Id;
                payment.ModifiedAt = DateTime.Now;
                payment.ModifiedBy = user.Id;

                _unitOfWork.Payments.Update(payment);
                _unitOfWork.Complete();

                _log.Information("Payment for booking added successfully");
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

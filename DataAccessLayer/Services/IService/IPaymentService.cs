using DataAccessLayer.ResponseModels;

namespace DataAccessLayer
{
    public interface IPaymentService
    {
        public int Add(PaymentDTO paymentdto);
        public bool Update(PaymentDTO paymentDTO, int id);
        public void CheckStatus(int id);
        public void PaymentCancel(int id);
        //public bool PaymentVerify(int id, bool verify, string paymentMode);
        public bool PaymentVerify(int id, PaymentVerificationModel verificationModel);
        public bool AddWallet(int amount, string email);
    }
}

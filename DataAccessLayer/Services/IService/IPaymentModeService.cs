namespace DataAccessLayer
{
    public interface IPaymentModeService
    {
        public List<PaymentModeResponse> Display();

        public PaymentModeResponse Search(int id);
        public bool Add(PaymentModeDTO paymentModedto);

        public bool Update(PaymentModeDTO paymentModedto, int id);

        public bool Delete(int id);
    }
}

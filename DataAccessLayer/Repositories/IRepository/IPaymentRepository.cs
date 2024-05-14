namespace DataAccessLayer
{
    public interface IPaymentRepository : IGenericRepository<Payment>
    {
        List<Payment> GetAll();
    }
}

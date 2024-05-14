namespace DataAccessLayer
{
    public interface ICancellationReasonService
    {
        List<CancellationReasonResponse> Display();
        bool Add(CancellationReasonDTO newReason);
        bool Update(CancellationReasonDTO updatedReason, int id);
        bool Delete(int id);
    }
}

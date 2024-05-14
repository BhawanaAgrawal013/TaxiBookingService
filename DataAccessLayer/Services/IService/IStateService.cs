namespace DataAccessLayer
{
    public interface IStateService
    {
        List<StateResponse> Display();
        bool Add(StateDTO newState);
        StateResponse Search(int id);
        bool Update(StateDTO updatedState, int id);
        bool Delete(int id);
    }
}

namespace DataAccessLayer
{
    public class StateService : IStateService
    {
        private readonly IStateUnitOfWork _unitOfWork;
        private readonly ILog _log;

        public StateService(IStateUnitOfWork unitOfWork, ILog log)
        {
            _unitOfWork = unitOfWork;
            _log = log;
        }

        public List<StateResponse> Display()
        {
            try
            {
                _log.Information("StateRepo: State list displayed successfully");
                return _unitOfWork.States.Display(item => item.isDelete == false).Select(e => new StateResponse { State = e.Name, Id = e.Id}).ToList();
            }
            catch (Exception ex)
            {
                _log.Error(ex.StackTrace);
                return null;
            }
        }

        public bool Add(StateDTO stateDTO)
        {
            try
            {

                if (!_unitOfWork.Users.Exists(item => item.Email == stateDTO.UserEmail)) return false;

                if (_unitOfWork.States.Exists(item => item.Name == stateDTO.State))
                {
                    State existingState = _unitOfWork.States.Get(item => item.Name == stateDTO.State);

                    if (existingState.isDelete == false) return false;
                }

                User user = _unitOfWork.Users.Get(item => item.Email == stateDTO.UserEmail);

                State state = new()
                {
                    Name = stateDTO.State,
                    CreatedAt = DateTime.Now,
                    isDelete = false,
                    CreatedBy = (int)user.Id
                };

                _unitOfWork.States.Add(state);
                _unitOfWork.Complete();

                _log.Information("StateRepo: State added successfully");
                return true;
            }
            catch (Exception ex)
            {
                _log.Error(ex.StackTrace);
                return false;
            }
        }

        public StateResponse Search(int id)
        {
            try
            {
                _log.Information("StateRepo: State searched successfully");
                return _unitOfWork.States.Display(item => item.isDelete == false && item.Id == id).Select(e => new StateResponse { State = e.Name, Id = e.Id }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                _log.Error(ex.StackTrace);
                return null;
            }
        }

        public bool Update(StateDTO stateDTO, int id)
        {
            try
            {
                if (!_unitOfWork.Users.Exists(item => item.Email == stateDTO.UserEmail) || !_unitOfWork.States.Exists(i => i.Id == id)) 
                    return false;

                State state = _unitOfWork.States.Get(item => item.Id == id);
                User user = _unitOfWork.Users.Get(item => item.Email == stateDTO.UserEmail);

                if (state == null) return false;

                if (state.isDelete) return false;

                state.Name = stateDTO.State;
                state.ModifiedAt = DateTime.Now;
                state.ModifiedBy = user.Id;

                _unitOfWork.States.Update(state);
                _unitOfWork.Complete();

                _log.Information("StateRepo: State updated successfully");
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
                State state = _unitOfWork.States.Get(item => item.Id == id);

                if (state == null || state.isDelete) return false;

                state.isDelete = true;

                _unitOfWork.States.Update(state);
                _unitOfWork.Complete();

                _log.Information("StateRepo: state is deleted successfully");
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

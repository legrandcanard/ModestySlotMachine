using ModestySlotMachine.Core.Entities;
using ModestySlotMachine.Core.Repositories;

namespace ModestySlotMachine.Persistent
{
    public class LocalUserDataRepository : IUserDataRepository
    {
        public Task<UserData> GetUserDataAsync()
        {
            throw new NotImplementedException();
        }

        public Task SaveUserDataAsync(UserData userData)
        {
            throw new NotImplementedException();
        }
    }
}


using ModestySlotMachine.Core.Entities;

namespace ModestySlotMachine.Persistent.Contracts
{
    public interface IUserDataRepository
    {
        public Task<UserData> GetUserDataAsync();
        public Task SaveUserDataAsync(UserData userData);
    }
}

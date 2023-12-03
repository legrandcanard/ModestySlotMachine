
using ModestySlotMachine.Core.Entities;

namespace ModestySlotMachine.Core.Repositories
{
    public interface IUserDataRepository
    {
        public Task<UserData> GetUserDataAsync();
        public Task SaveUserDataAsync(UserData userData);
    }
}

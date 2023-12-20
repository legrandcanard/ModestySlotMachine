
using ModestSlotMachine.Core.Entities;

namespace ModestSlotMachine.Core.Repositories
{
    public interface IUserDataRepository
    {
        public Task<UserData> GetUserDataAsync();
        public Task SaveUserDataAsync(UserData userData);
    }
}

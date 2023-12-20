using ModestSlotMachine.Core.Entities;
using ModestSlotMachine.Core.Repositories;

namespace ModestSlotMachine.Core.Services
{
    public class UserDataService
    {
        private readonly IUserDataRepository _userDataRepository;

        public UserDataService(IUserDataRepository userDataRepository)
        {
            _userDataRepository = userDataRepository;
        }

        public async Task<UserData> GetUserDataAsync()
            => await _userDataRepository.GetUserDataAsync();

        public async Task SaveUserDataAsync(UserData userData)
            => await _userDataRepository.SaveUserDataAsync(userData);
    }
}

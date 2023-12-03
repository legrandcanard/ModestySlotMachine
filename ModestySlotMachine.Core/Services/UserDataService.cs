using ModestySlotMachine.Core.Entities;
using ModestySlotMachine.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModestySlotMachine.Core.Services
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

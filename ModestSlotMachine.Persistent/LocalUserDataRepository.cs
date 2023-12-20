using ModestSlotMachine.Core.Entities;
using ModestSlotMachine.Core.Repositories;
using System.Text.Json;

namespace ModestSlotMachine.Persistent
{
    public class LocalUserDataRepository : IUserDataRepository
    {
        protected readonly string BaseDirPath
            = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Modest Slot Machine");

        protected readonly string UserDataSaveFileName = "user_data.json";

        protected string UserDataSaveFilePath => Path.Combine(BaseDirPath, UserDataSaveFileName);

        public async Task<UserData> GetUserDataAsync()
        {
            if (!File.Exists(UserDataSaveFilePath))
            {
                var defaultUserData = new UserData()
                {
                    UserSettings = new UserData.Settings
                    {
                        Selectedlanguage = "en-US"
                    }
                };
                await SaveUserDataAsync(defaultUserData);
                return defaultUserData;
            }

            using StreamReader sr = new(Path.Combine(BaseDirPath, UserDataSaveFileName));
            string serializedUserData = await sr.ReadToEndAsync();
            return JsonSerializer.Deserialize(serializedUserData, typeof(UserData)) as UserData ?? throw new Exception("Unable to load user data.");
        }

        public async Task SaveUserDataAsync(UserData userData)
        {
            if (!Directory.Exists(BaseDirPath))
                Directory.CreateDirectory(BaseDirPath);

            var serializedUserData = JsonSerializer.Serialize(userData);
            using StreamWriter sw = new(Path.Combine(BaseDirPath, UserDataSaveFileName));
            await sw.WriteAsync(serializedUserData);
        }


    }
}

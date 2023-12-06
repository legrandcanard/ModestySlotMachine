
namespace ModestySlotMachine.Core.Entities
{
    public class UserData
    {
        public decimal Balance { get; set; }
        public required Settings UserSettings { get; set; }

        public class Settings
        {
            public sbyte SoundLevel { get; set; }
            public string? Selectedlanguage { get; set; }
        }
    }
}

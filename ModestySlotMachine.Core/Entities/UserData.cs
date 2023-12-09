
namespace ModestySlotMachine.Core.Entities
{
    public class UserData
    {
        public decimal Balance { get; set; }
        public required Settings UserSettings { get; set; }

        public class Settings
        {
            public double SoundVolume { get; set; }
            public double FxSoundVolume { get; set; }
            public string? Selectedlanguage { get; set; }
        }
    }
}

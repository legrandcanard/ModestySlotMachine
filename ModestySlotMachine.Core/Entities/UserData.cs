
using System.Globalization;

namespace ModestSlotMachine.Core.Entities
{
    public class UserData
    {
        public decimal Balance { get; set; }
        public required Settings UserSettings { get; set; }
        public DynamicSettings GameRelatedSettings { get; set; } = new DynamicSettings();

        public class Settings
        {
            public double SoundVolume { get; set; }
            public double FxSoundVolume { get; set; }
            public string? Selectedlanguage { get; set; }
        }

        public class DynamicSettings : Dictionary<string, string>
        {
            public bool TryGetDecimal(string key, out decimal value)
            {
                if (!base.ContainsKey(key))
                {
                    value = default;
                    return false;
                }

                value = Convert.ToDecimal(base[key]);
                return true;
            }

            public void AddOrUpdate(string key, decimal value)
            {
                base[key] = value.ToString(CultureInfo.InvariantCulture);
            }

            public void AddOrUpdate(string key, double value)
            {
                base[key] = value.ToString(CultureInfo.InvariantCulture);
            }

            public bool TryGetDouble(string key, out double value)
            {
                if (!base.ContainsKey(key))
                {
                    value = default;
                    return false;
                }

                value = Convert.ToDouble(base[key]);
                return true;
            }

            public bool TryGetInteger(string key, out int value)
            {
                if (!base.ContainsKey(key))
                {
                    value = default;
                    return false;
                }

                value = Convert.ToInt32(base[key]);
                return true;
            }

            public void AddOrUpdate(string key, int value)
            {
                base[key] = value.ToString(CultureInfo.InvariantCulture);
            }

            public bool TryGetGuid(string key, out Guid value)
            {
                if (!base.ContainsKey(key))
                {
                    value = default;
                    return false;
                }

                return Guid.TryParse(base[key], out value);
            }

            public void AddOrUpdate(string key, Guid value)
            {
                base[key] = value.ToString();
            }
        }
    }
}

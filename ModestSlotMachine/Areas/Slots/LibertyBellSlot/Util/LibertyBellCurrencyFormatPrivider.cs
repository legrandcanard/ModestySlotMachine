
using System.Globalization;

namespace ModestSlotMachine.Areas.Slots.LibertyBellSlot.Util
{
    internal class LibertyBellCurrencyFormatPrivider
        : IFormatProvider, ICustomFormatter
    {
        const char DollarChar = '$';
        const char CentChar = '¢';

        public string Format(string format, object arg, IFormatProvider formatProvider)
        {
            if (!this.Equals(formatProvider))
                return null;

            if (arg == null)
                return "0";

            decimal value = arg as decimal? ?? throw new ArgumentNullException(nameof(arg));

            if (value >= 1)
                return $"{DollarChar}{value}";
            else
                return $"{CentChar}{(int)(value * 100)}";

        }

        public object GetFormat(Type formatType)
        {
            if (formatType == typeof(NumberFormatInfo))
            {
                return new NumberFormatInfo()
                {
                    // something here
                };
            }
            return this;
        }
    }
}

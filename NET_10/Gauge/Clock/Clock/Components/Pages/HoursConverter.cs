using C1.Blazor.Core;
using System.Globalization;

namespace Clock.Components.Pages
{
    public class HoursConverter : IValueConverter
    {
        public bool ShowRomanNumbers { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double dValue && targetType == typeof(string))
            {
                if (ShowRomanNumbers)
                    return ToRoman((int)dValue / 60);
                else
                    return (dValue / 60).ToString();
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public static string ToRoman(int number)
        {
            if (number <= 0 || number > 3999)
                throw new ArgumentOutOfRangeException(nameof(number), "Value must be between 1 and 3999.");

            var map = new (int value, string symbol)[]
            {
                (1000, "M"),
                (900,  "CM"),
                (500,  "D"),
                (400,  "CD"),
                (100,  "C"),
                (90,   "XC"),
                (50,   "L"),
                (40,   "XL"),
                (10,   "X"),
                (9,    "IX"),
                (5,    "V"),
                (4,    "IV"),
                (1,    "I")
            };

            var result = new System.Text.StringBuilder();

            foreach (var (value, symbol) in map)
            {
                while (number >= value)
                {
                    result.Append(symbol);
                    number -= value;
                }
            }

            return result.ToString();
        }
    }
}

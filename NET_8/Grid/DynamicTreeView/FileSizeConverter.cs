using C1.Blazor.Core;
using System.Globalization;
using System.Text.RegularExpressions;

namespace DynamicTreeView
{
    public partial class FileSizeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType == typeof(string))
            {
                if (value == null) return string.Empty;
                try
                {
                    var longValue = System.Convert.ToInt64(value);
                    return BytesToString(longValue);
                }
                catch { }
            }
            return value;
        }

        [GeneratedRegex(@"\D", RegexOptions.IgnoreCase| RegexOptions.CultureInvariant)]
        private static partial Regex NonDigit();

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is null)
            {
                return default(long);
            }

            if (value is long v)
            {
                return v;
            }

            return long.TryParse(NonDigit().Replace(value.ToString(), ""), out v) ? v 
                : (object)default(long);
        }

        static string BytesToString(long byteCount)
        {
            string[] suf = { "B", "KB", "MB", "GB", "TB", "PB", "EB" }; //Longs run out around EB
            if (byteCount == 0)
                return "0" + suf[0];
            long bytes = Math.Abs(byteCount);
            int place = System.Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
            double num = Math.Round(bytes / Math.Pow(1024, place), 1);
            return (Math.Sign(byteCount) * num).ToString() + suf[place];
        }
    }

}

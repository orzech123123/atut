using Newtonsoft.Json.Converters;

namespace Atut.Converters
{
    public class DateTimeJsonConverter : IsoDateTimeConverter
    {
        public DateTimeJsonConverter(string format)
        {
            DateTimeFormat = format;
        }
    }
}

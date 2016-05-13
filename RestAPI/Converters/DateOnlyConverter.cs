using Newtonsoft.Json.Converters;

namespace RestAPI.Converters
{
    class DateOnlyConverter : IsoDateTimeConverter
    {
        public DateOnlyConverter()
        {
            base.DateTimeFormat = "yyyy-MM-dd";
        }
    }
}

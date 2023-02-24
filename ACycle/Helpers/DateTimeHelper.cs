using Newtonsoft.Json;

namespace ACycle.Helpers
{
    public static class DateTimeHelper
    {
        public static DateTime ParseISO8601DateTimeString(string dateTimeString)
        {
            if (!dateTimeString.StartsWith('"'))
                dateTimeString = '"' + dateTimeString;
            if (!dateTimeString.EndsWith('"'))
                dateTimeString += '"';

            return JsonConvert.DeserializeObject<DateTime>(dateTimeString).ToLocalTime();
        }
    }
}

namespace ACycle.Extensions
{
    public static class DateTimeExtension
    {
        public static DateTime Trim(this DateTime dateTime, long roundTicks)
        {
            return new DateTime(dateTime.Ticks - dateTime.Ticks % roundTicks, dateTime.Kind);
        }

        public static DateTime TrimSeconds(this DateTime dateTime)
        {
            return dateTime.Trim(TimeSpan.TicksPerSecond);
        }
    }
}

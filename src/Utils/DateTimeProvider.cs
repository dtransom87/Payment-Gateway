using System;

namespace payment_gateway.Utils
{
    public interface IDateTimeProvider
    {
        DateTime GetDateTime();
    }

    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime GetDateTime()
        {
            return DateTime.UtcNow;
        }
    }
}
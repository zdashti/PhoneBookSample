namespace PhoneBook.Common
{
    public class UtcDateTimeProvider : IDateTimeProvider
    {
        public DateTime Now => DateTime.UtcNow;

        public DateTime Today => DateTime.UtcNow.Date;

        public DateTime UtcNow => DateTime.UtcNow;

        public DateTime ToLocal(DateTime utcDateTime) => utcDateTime.ToLocalTime();
    }
}
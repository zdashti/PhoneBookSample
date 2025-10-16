namespace PhoneBook.Common
{
    public interface IDateTimeProvider
    {
        DateTime Now { get; }
        DateTime Today { get; }
        DateTime UtcNow { get; }
        DateTime ToLocal(DateTime utcDateTime);
    }
}
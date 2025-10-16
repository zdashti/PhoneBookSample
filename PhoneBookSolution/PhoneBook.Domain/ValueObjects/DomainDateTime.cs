namespace PhoneBook.Domain.ValueObjects
{
    public class DomainDateTime : IComparable<DomainDateTime>
    {
        public DateTime Value { get; private set; }

        private DomainDateTime(DateTime dateTime)
        {
            Value = dateTime;
        }

        public static DomainDateTime Now()
        {
            return UtcNow();
        }

        public static DomainDateTime UtcNow()
        {
            return new DomainDateTime(DateTime.UtcNow);
        }

        public static DomainDateTime LocalNow()
        {
            return new DomainDateTime(DateTime.Now);
        }

        public DateTime ToLocal()
        {
            return Value.ToLocalTime();
        }

        public DateTime ToUtc()
        {
            return Value.ToUniversalTime();
        }

        public string ToString(string format)
        {
            return Value.ToString(format);
        }

        public override string ToString()
        {
            return Value.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public int CompareTo(DomainDateTime other)
        {
            if (other == null) return 1;
            return Value.CompareTo(other.Value);
        }

        public static bool operator >(DomainDateTime left, DomainDateTime right) => left.CompareTo(right) > 0;
        public static bool operator <(DomainDateTime left, DomainDateTime right) => left.CompareTo(right) < 0;
        public static bool operator >=(DomainDateTime left, DomainDateTime right) => left.CompareTo(right) >= 0;
        public static bool operator <=(DomainDateTime left, DomainDateTime right) => left.CompareTo(right) <= 0;

        public static bool operator ==(DomainDateTime left, DomainDateTime right) => left?.Value == right?.Value;
        public static bool operator !=(DomainDateTime left, DomainDateTime right) => !(left == right);

        public override bool Equals(object obj)
        {
            return obj is DomainDateTime other && Value.Equals(other.Value);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public static implicit operator DomainDateTime(DateTime dateTime)
        {
            return new DomainDateTime(dateTime);
        }

        public static implicit operator DateTime(DomainDateTime domainDateTime)
        {
            return domainDateTime.Value;
        }
    }
}

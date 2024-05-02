
namespace CBT3_Domain.Common;
public abstract class BaseId<T> : IEquatable<BaseId<T>>
{
    public T Value { get; }

    protected BaseId(T value)
    {
        if (value is null)
        {
            throw new ArgumentNullException(nameof(value), "ID value cannot be null.");
        }
        if (value.Equals(string.Empty))
        {
            throw new ArgumentException("ID value cannot be empty.", nameof(value));
        }
        Value = value;
    }

    public bool Equals(BaseId<T>? other)
    {
        if (other is null)
        {
            return false;
        }

        return Equals(Value, other.Value);
    }

    public override bool Equals(object obj)
    {
        if (obj is null || !(obj is BaseId<T> other))
        {
            return false;
        }

        return Equals(other);
    }

    public override int GetHashCode() => Value.GetHashCode();

    public override string ToString() => Value.ToString();
}


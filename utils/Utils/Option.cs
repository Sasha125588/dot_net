namespace Utils;

public abstract class Option<T>
{
	public sealed class Some(T value) : Option<T>
	{
		public T Value { get; } = value;
	}

	public sealed class None : Option<T>;
}
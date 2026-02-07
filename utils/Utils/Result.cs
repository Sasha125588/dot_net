namespace Utils;

public abstract class Result<T, E>
{
	public sealed class Ok(T value) : Result<T, E>
	{
		public T Value { get; } = value;
	}

	public sealed class Err(E error) : Result<T, E>
	{
		public E Error { get; } = error;
	}
}

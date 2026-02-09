namespace Utils;

public abstract class Result<T, E>
{
	protected Result()
	{
	}
}

public sealed class Ok<T, E>(T value) : Result<T, E>
{
	public T Value { get; } = value;

	public void Deconstruct(out T value) => value = Value;
}

public sealed class Err<T, E>(E error) : Result<T, E>
{
	public E Error { get; } = error;

	public void Deconstruct(out E error) => error = Error;
}

public static class Result
{
	public static Ok<T, E> Ok<T, E>(T value) => new(value);
	public static Err<T, E> Err<T, E>(E error) => new(error);
}
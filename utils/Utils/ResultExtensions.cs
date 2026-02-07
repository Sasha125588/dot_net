namespace Utils;

public static class ResultExtensions
{
	public static Result<TOut, E> AndThen<T, TOut, E>(this Result<T, E> result, Func<T, Result<TOut, E>> binder)
	{
		return result switch
		{
			Result<T, E>.Ok ok => binder(ok.Value),
			Result<T, E>.Err err => new Result<TOut, E>.Err(err.Error),
			_ => throw new InvalidOperationException()
		};
	}

	public static Result<TOut, E> Map<T, TOut, E>(
		this Result<T, E> result,
		Func<T, TOut> mapper)
	{
		return result switch
		{
			Result<T, E>.Ok ok => new Result<TOut, E>.Ok(mapper(ok.Value)),
			Result<T, E>.Err err => new Result<TOut, E>.Err(err.Error),
			_ => throw new InvalidOperationException()
		};
	}

	public static void Match<T, E>(
		this Result<T, E> result,
		Action<T> onOk,
		Action<E> onErr)
	{
		switch (result)
		{
			case Result<T, E>.Ok ok: onOk(ok.Value); break;
			case Result<T, E>.Err err: onErr(err.Error); break;
		}
	}

	public static Result<T, EOut> MapError<T, E, EOut>(
		this Result<T, E> result,
		Func<E, EOut> mapper)
	{
		return result switch
		{
			Result<T, E>.Ok ok => new Result<T, EOut>.Ok(ok.Value),
			Result<T, E>.Err err => new Result<T, EOut>.Err(mapper(err.Error)),
			_ => throw new InvalidOperationException()
		};
	}

	public static T ValueOr<T, E>(this Result<T, E> result, T fallback)
	{
		return result switch
		{
			Result<T, E>.Ok ok => ok.Value,
			Result<T, E>.Err => fallback,
			_ => throw new InvalidOperationException()
		};
	}

	public static T Unwrap<T, E>(this Result<T, E> result)
	{
		return result switch
		{
			Result<T, E>.Ok ok => ok.Value,
			Result<T, E>.Err err => throw new InvalidOperationException($"Called Unwrap on Err: {err.Error}"),
			_ => throw new InvalidOperationException()
		};
	}

	public static T UnwrapOr<T, E>(this Result<T, E> result, T defaultValue)
	{
		return result.ValueOr(defaultValue);
	}

	public static T Expect<T, E>(this Result<T, E> result, string message)
	{
		return result switch
		{
			Result<T, E>.Ok ok => ok.Value,
			Result<T, E>.Err err => throw new InvalidOperationException($"{message}: {err.Error}"),
			_ => throw new InvalidOperationException()
		};
	}

	public static bool IsOk<T, E>(this Result<T, E> result) => result is Result<T, E>.Ok;

	public static bool IsErr<T, E>(this Result<T, E> result) => result is Result<T, E>.Err;
}
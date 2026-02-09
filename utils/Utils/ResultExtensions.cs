using System.Collections.Immutable;

namespace Utils;

public static class ResultExtensions
{
	// ============================================================
	// Перевірка стану Result
	// ============================================================

	public static bool IsOk<T, E>(this Result<T, E> result) =>
		result is Ok<T, E>;

	public static bool IsErr<T, E>(this Result<T, E> result) =>
		result is Err<T, E>;


	// ============================================================
	// Перетворення значень
	// ============================================================

	public static Result<TOut, E> Map<T, TOut, E>(
		this Result<T, E> result,
		Func<T, TOut> mapper) =>
		result switch
		{
			Ok<T, E> ok => Result.Ok<TOut, E>(mapper(ok.Value)),
			Err<T, E> err => Result.Err<TOut, E>(err.Error),
			_ => throw new InvalidOperationException()
		};

	public static Result<T, EOut> MapErr<T, E, EOut>(
		this Result<T, E> result,
		Func<E, EOut> mapper) =>
		result switch
		{
			Ok<T, E> ok => Result.Ok<T, EOut>(ok.Value),
			Err<T, E> err => Result.Err<T, EOut>(mapper(err.Error)),
			_ => throw new InvalidOperationException()
		};

	public static Result<ImmutableList<T>, E> Collect<T, E>(
		this IEnumerable<Result<T, E>> results)
	{
		var builder = ImmutableList.CreateBuilder<T>();

		foreach (var result in results)
		{
			switch (result)
			{
				case Ok<T, E> ok:
					builder.Add(ok.Value);
					break;
				case Err<T, E> err:
					return Result.Err<ImmutableList<T>, E>(err.Error);
				default:
					throw new InvalidOperationException();
			}
		}

		return Result.Ok<ImmutableList<T>, E>(builder.ToImmutable());
	}

	// ============================================================
	// Композиція та chaining
	// ============================================================

	public static Result<TOut, E> AndThen<T, TOut, E>(
		this Result<T, E> result,
		Func<T, Result<TOut, E>> mapper) =>
		result switch
		{
			Ok<T, E> ok => mapper(ok.Value),
			Err<T, E> err => Result.Err<TOut, E>(err.Error),
			_ => throw new InvalidOperationException()
		};

	public static Result<TOut, E> And<T, TOut, E>(
		this Result<T, E> result,
		Result<TOut, E> other) =>
		result switch
		{
			Ok<T, E> => other,
			Err<T, E> err => Result.Err<TOut, E>(err.Error),
			_ => throw new InvalidOperationException()
		};

	public static Result<T, E> Or<T, E>(
		this Result<T, E> result,
		Result<T, E> fallback) =>
		result.IsOk() ? result : fallback;

	public static Result<T, EOut> OrElse<T, E, EOut>(
		this Result<T, E> result,
		Func<E, Result<T, EOut>> fallback) =>
		result switch
		{
			Ok<T, E> ok => Result.Ok<T, EOut>(ok.Value),
			Err<T, E> err => fallback(err.Error),
			_ => throw new InvalidOperationException()
		};


	// ============================================================
	// Pattern matching
	// ============================================================

	public static void Match<T, E>(
		this Result<T, E> result,
		Action<T> onOk,
		Action<E> onErr)
	{
		switch (result)
		{
			case Ok<T, E> ok:
				onOk(ok.Value);
				break;

			case Err<T, E> err:
				onErr(err.Error);
				break;

			default:
				throw new InvalidOperationException();
		}
	}

	public static TOut Match<T, E, TOut>(
		this Result<T, E> result,
		Func<T, TOut> onOk,
		Func<E, TOut> onErr) =>
		result switch
		{
			Ok<T, E> ok => onOk(ok.Value),
			Err<T, E> err => onErr(err.Error),
			_ => throw new InvalidOperationException()
		};

	// ============================================================
	// Отримання значення з fallback
	// ============================================================

	public static TOut MapOr<T, TOut, E>(
		this Result<T, E> result,
		TOut fallback,
		Func<T, TOut> mapper) =>
		result switch
		{
			Ok<T, E> ok => mapper(ok.Value),
			Err<T, E> => fallback,
			_ => throw new InvalidOperationException()
		};

	public static TOut MapOrElse<T, TOut, E>(
		this Result<T, E> result,
		Func<E, TOut> fallbackMapper,
		Func<T, TOut> mapper) =>
		result switch
		{
			Ok<T, E> ok => mapper(ok.Value),
			Err<T, E> err => fallbackMapper(err.Error),
			_ => throw new InvalidOperationException()
		};

	public static T UnwrapOr<T, E>(
		this Result<T, E> result,
		T fallback) =>
		result switch
		{
			Ok<T, E> ok => ok.Value,
			Err<T, E> => fallback,
			_ => throw new InvalidOperationException()
		};

	public static T ValueOr<T, E>(this Result<T, E> result, T fallback) =>
		result.UnwrapOr(fallback);

	public static T UnwrapOrElse<T, E>(
		this Result<T, E> result,
		Func<E, T> fallbackMapper) =>
		result switch
		{
			Ok<T, E> ok => ok.Value,
			Err<T, E> err => fallbackMapper(err.Error),
			_ => throw new InvalidOperationException()
		};


	// ============================================================
	// Отримання значення
	// ============================================================

	public static T Unwrap<T, E>(this Result<T, E> result) =>
		result switch
		{
			Ok<T, E> ok => ok.Value,
			Err<T, E> err =>
				throw new InvalidOperationException($"Called Unwrap on Err: {err.Error}"),
			_ => throw new InvalidOperationException()
		};

	public static T Expect<T, E>(
		this Result<T, E> result,
		string message) =>
		result switch
		{
			Ok<T, E> ok => ok.Value,
			Err<T, E> err =>
				throw new InvalidOperationException($"{message}: {err.Error}"),
			_ => throw new InvalidOperationException()
		};
}

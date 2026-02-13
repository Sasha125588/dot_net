namespace Utils;

public static class Validation
{
	public static Result<int, string> ParseInt(
		string? input,
		string fieldName = "значення")
	{
		if (string.IsNullOrWhiteSpace(input))
			return Result.Err<int, string>($"{fieldName}: не введено");

		return int.TryParse(input, out var value)
			? Result.Ok<int, string>(value)
			: Result.Err<int, string>($"{fieldName}: очікується ціле число");
	}

	public static Result<double, string> ParseDouble(
		string? input,
		string fieldName = "значення")
	{
		if (string.IsNullOrWhiteSpace(input))
			return Result.Err<double, string>($"{fieldName}: не введено");

		return double.TryParse(input, out var value)
			? Result.Ok<double, string>(value)
			: Result.Err<double, string>($"{fieldName}: очікується число");
	}

	public static Result<T, string> InRange<T>(
		T value,
		T min,
		T max,
		string fieldName = "значення")
		where T : IComparable<T>
	{
		if (value.CompareTo(min) < 0 || value.CompareTo(max) > 0)
			return Result.Err<T, string>(
				$"{fieldName} має бути між {min} та {max}");

		return Result.Ok<T, string>(value);
	}
}
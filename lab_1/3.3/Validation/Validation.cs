using Utils;
using _3._3.Errors;

namespace _3._3.Validation;

public static class Validator
{
	private const int MinValue = 5;
	private const int Divisor = 5;

	private static Result<int, ValidationError> TryParseInt(string input) =>
		int.TryParse(input, out var value)
			? Result.Ok<int, ValidationError>(value)
			: Result.Err<int, ValidationError>(new ValidationError.ParseError(input));

	private static Result<int, ValidationError> ValidateTooSmall(int value) =>
		value >= MinValue
			? Result.Ok<int, ValidationError>(value)
			: Result.Err<int, ValidationError>(new ValidationError.TooSmall(value, MinValue));

	private static Result<int, ValidationError> ValidateDivisible(int value) =>
		value % Divisor == 0
			? Result.Ok<int, ValidationError>(value)
			: Result.Err<int, ValidationError>(new ValidationError.NotDivisibleBy(value, Divisor));

	public static Result<int, ValidationError> ValidateHeight(string input) =>
		TryParseInt(input)
			.AndThen(ValidateTooSmall)
			.AndThen(ValidateDivisible);
}
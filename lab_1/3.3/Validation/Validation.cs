using Utils;
using Utils.Common;
using _3._3.Errors;

namespace _3._3.Validation;

public static class Validator
{
	private static readonly IntValidator IntValidator = new();
	private const int MinHeight = 5;
	private const int Divisor = 5;

	private static Result<int, IValidationError> TryParseInt(string input) =>
		IntValidator.Validate(input);

	private static Result<int, IValidationError> ValidateMinHeight(int height) =>
		height >= MinHeight
			? Result.Ok<int, IValidationError>(height)
			: Result.Err<int, IValidationError>(new ValidationError.TooSmallHeightError(height, MinHeight));

	private static Result<int, IValidationError> ValidateDivisibleByFive(int height) =>
		height % Divisor == 0
			? Result.Ok<int, IValidationError>(height)
			: Result.Err<int, IValidationError>(new ValidationError.HeightNotDivisibleByError(height, Divisor));

	public static Result<int, IValidationError> Validate(string input) =>
		TryParseInt(input)
			.AndThen(ValidateMinHeight)
			.AndThen(ValidateDivisibleByFive);
}
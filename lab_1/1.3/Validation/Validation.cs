using _1._3.Errors;
using Utils;
using Utils.Common;

namespace _1._3.Validation;

public static class Validator
{
	private static readonly IntValidator IntValidator = new();

	public static Result<int, IValidationError> Validate(string? input)
	{
		if (string.IsNullOrWhiteSpace(input))
		{
			return Result.Err<int, IValidationError>(new GradeValidationError.EmptyInput());
		}

		return IntValidator.Validate(input)
			.AndThen(ValidateRange);
	}

	private static Result<int, IValidationError> ValidateRange(int value) =>
		value is < 0 or > 100
			? Result.Err<int, IValidationError>(new GradeValidationError.OutOfRange(value, 0, 100))
			: Result.Ok<int, IValidationError>(value);
}

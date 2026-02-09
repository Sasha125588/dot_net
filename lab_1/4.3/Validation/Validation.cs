using Utils;
using _4._3.Errors;

namespace _4._3.Validation;

public static class Validator
{
	private static Result<DateOnly, ValidationError> TryParseDateOnly(string input) =>
		DateOnly.TryParse(input, out var value)
			? Result.Ok<DateOnly, ValidationError>(value)
			: Result.Err<DateOnly, ValidationError>(new ValidationError.ParseDateOnlyError(input));

	private static Result<DateOnly, ValidationError>
		ValidateEndDateIsAfterStartDate(DateOnly startDate, DateOnly endDate) =>
		startDate < endDate
			? Result.Ok<DateOnly, ValidationError>(endDate)
			: Result.Err<DateOnly, ValidationError>(
				new ValidationError.ValidateStartDateAfterEndDate(startDate, endDate));

	public static Result<DateOnly, ValidationError> ValidateStartDate(string input) =>
		TryParseDateOnly(input);

	public static Result<DateOnly, ValidationError> ValidateEndDate(string input, DateOnly startDate) =>
		TryParseDateOnly(input)
			.AndThen(endDate => ValidateEndDateIsAfterStartDate(startDate, endDate));
}

using Utils;
using _4._3.Errors;

namespace _4._3.Validation;

public static class Validator
{
	private static Result<DateOnly, ValidationError> TryParseDateOnly(string input) =>
		DateOnly.TryParse(input, out var value)
			? new Result<DateOnly, ValidationError>.Ok(value)
			: new Result<DateOnly, ValidationError>.Err(new ValidationError.ParseDateOnlyError(input));

	private static Result<DateOnly, ValidationError> ValidateEndDateIsAfterStartDate(DateOnly startDate, DateOnly endDate) =>
		startDate < endDate
			? new Result<DateOnly, ValidationError>.Ok(endDate)
			: new Result<DateOnly, ValidationError>.Err(
				new ValidationError.ValidateStartDateAfterEndDate(startDate, endDate));

	public static Result<DateOnly, ValidationError> ValidateStartDate(string input) =>
		TryParseDateOnly(input);

	public static Result<DateOnly, ValidationError> ValidateEndDate(string input, DateOnly startDate) =>
		TryParseDateOnly(input)
			.AndThen(endDate => ValidateEndDateIsAfterStartDate(startDate, endDate));
}

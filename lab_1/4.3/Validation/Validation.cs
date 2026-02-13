using Utils;
using Utils.Common;
using _4._3.Models;

namespace _4._3.Validation;

public static class Validator
{
	private static readonly DateOnlyValidator DateOnlyValidator = new();

	private static Result<DateOnly, IValidationError>
		ValidateEndDateIsAfterStartDate(DateOnly startDate, DateOnly endDate) =>
		startDate < endDate
			? Result.Ok<DateOnly, IValidationError>(endDate)
			: Result.Err<DateOnly, IValidationError>(
				new DateRange.ValidateStartDateAfterEndDateError(startDate, endDate));

	public static Result<DateOnly, IValidationError> ValidateStartDate(string input) =>
		DateOnlyValidator.Validate(input);

	public static Result<DateOnly, IValidationError> Validate(string input, DateOnly startDate) =>
		DateOnlyValidator.Validate(input)
			.AndThen(endDate => ValidateEndDateIsAfterStartDate(startDate, endDate));
}

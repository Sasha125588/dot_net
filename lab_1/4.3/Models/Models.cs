using Utils;
using Utils.Common;

namespace _4._3.Models;

public sealed record DateRange(DateOnly StartDate, DateOnly EndDate)
{
	public sealed record ValidateStartDateAfterEndDateError(DateOnly StartDate, DateOnly EndDate) : IValidationError;

	public int YearsBetween()
	{
		var years = EndDate.Year - StartDate.Year;

		if (EndDate < StartDate.AddYears(years))
		{
			years--;
		}

		return years;
	}

	public static Result<DateRange, IValidationError> Create(DateOnly start, DateOnly end) =>
		start < end
			? Result.Ok<DateRange, IValidationError>(new DateRange(start, end))
			: Result.Err<DateRange, IValidationError>(new ValidateStartDateAfterEndDateError(start, end));
}

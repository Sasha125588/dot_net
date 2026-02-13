using _4._3.Models;
using Utils.Common;

namespace _4._3.ErrorFormatters;

public static class ValidationErrorFormatter
{
	public static string Format(IValidationError err) => err switch
	{
		DateRange.ValidateStartDateAfterEndDateError e =>
			$"Дата Початку({e.StartDate:dd.MM.yyyy}) повинна бути раніше за дату Кінця({e.EndDate:dd.MM.yyyy})",
		_ => CommonValidationErrorFormatter.Format(err)
	};
}

using _4._3.Errors;

namespace _4._3.ErrorFormatters;

public static class ValidationErrorFormatter
{
	public static string Format(ValidationError err) => err switch
	{
		ValidationError.ParseDateOnlyError e => $"Не вдалося розпізнати '{e.Input}' як дату",
		ValidationError.ValidateStartDateAfterEndDate e =>
			$"Дата Початку({e.FirstDate}) повинна бути раніше за дату Кінця({e.SecondDate})",
		_ => "Невідома помилка"
	};
}
using _3._3.Errors;

namespace _3._3.ErrorFormatters;

public static class ValidationErrorFormatter
{
	public static string Format(ValidationError err) => err switch
	{
		ValidationError.ParseError e => $"Не вдалося розпізнати '{e.Input}' як число",
		ValidationError.TooSmall e => $"Значення {e.Value} занадто мале (мінімум: {e.MinValue})",
		ValidationError.NotDivisibleBy e => $"Значення {e.Value} має бути кратне {e.Divisor}",
		_ => "Невідома помилка"
	};
}
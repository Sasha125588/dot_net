using _3._3.Errors;
using Utils.Common;

namespace _3._3.ErrorFormatters;

public static class ValidationErrorFormatter
{
	public static string Format(IValidationError err) => err switch
	{
		ValidationError.TooSmallHeightError e => $"Значення {e.Value} занадто мале (мінімум: {e.MinValue})",
		ValidationError.HeightNotDivisibleByError e => $"Значення {e.Value} має бути кратне {e.Divisor}",
		_ => CommonValidationErrorFormatter.Format(err)
	};
}
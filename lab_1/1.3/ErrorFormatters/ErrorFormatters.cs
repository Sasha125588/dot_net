using _1._3.Errors;
using Utils.Common;

namespace _1._3.ErrorFormatters;

public static class ValidationErrorFormatter
{
	public static string Format(IValidationError err) => err switch
	{
		GradeValidationError.EmptyInput => "не введено значення",
		GradeValidationError.OutOfRange => "має бути від 0 до 100",
		_ => CommonValidationErrorFormatter.Format(err)
	};
}

using Utils.Common;

namespace _1._3.Errors;

public abstract record GradeValidationError : IValidationError
{
	public sealed record EmptyInput : GradeValidationError;
	public sealed record OutOfRange(int Value, int Min, int Max) : GradeValidationError;
}

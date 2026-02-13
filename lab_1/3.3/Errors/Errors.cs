using Utils.Common;

namespace _3._3.Errors;

public abstract record ValidationError : IValidationError
{
	public sealed record TooSmallHeightError(int Value, int MinValue) : ValidationError;
	public sealed record HeightNotDivisibleByError(int Value, int Divisor) : ValidationError;
}

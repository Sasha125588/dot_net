namespace _3._3.Errors;

public abstract record ValidationError
{
	public sealed record ParseError(string Input) : ValidationError;

	public sealed record TooSmall(int Value, int MinValue) : ValidationError;

	public sealed record NotDivisibleBy(int Value, int Divisor) : ValidationError;
}
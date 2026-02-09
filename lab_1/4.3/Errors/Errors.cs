namespace _4._3.Errors;

public abstract record ValidationError
{
	public sealed record ParseDateOnlyError(string Input) : ValidationError;

	public sealed record ValidateStartDateAfterEndDate(DateOnly FirstDate, DateOnly SecondDate) : ValidationError;
}
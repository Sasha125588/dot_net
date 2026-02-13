namespace Utils.Common;

public interface IValidationError;

public abstract record CommonValidationError : IValidationError
{
	public sealed record ParseDateOnlyError(string Input) : CommonValidationError;

	public sealed record ParseIntError(string Input) : CommonValidationError;

	public sealed record ParseDoubleError(string Input) : CommonValidationError;

	public sealed record ParseTimeOnlyError(string Input) : CommonValidationError;
}
namespace Utils.Common;

public interface IValidator<T>
{
	Result<T, IValidationError> Validate(string input);
}

public sealed class DateOnlyValidator : IValidator<DateOnly>
{
	public Result<DateOnly, IValidationError> Validate(string input) =>
		DateOnly.TryParse(input, out var value)
			? Result.Ok<DateOnly, IValidationError>(value)
			: Result.Err<DateOnly, IValidationError>(new CommonValidationError.ParseDateOnlyError(input));
}

public sealed class IntValidator : IValidator<int>
{
	public Result<int, IValidationError> Validate(string input) =>
		int.TryParse(input, out var value)
			? Result.Ok<int, IValidationError>(value)
			: Result.Err<int, IValidationError>(new CommonValidationError.ParseIntError(input));
}

public sealed class DoubleValidator : IValidator<double>
{
	public Result<double, IValidationError> Validate(string input) =>
		double.TryParse(input, out var value)
			? Result.Ok<double, IValidationError>(value)
			: Result.Err<double, IValidationError>(new CommonValidationError.ParseDoubleError(input));
}
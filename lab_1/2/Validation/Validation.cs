using _2.Errors;
using Utils;
using Utils.Common;

namespace _2.Validation;

public static class Validator
{
	private static readonly DoubleValidator DoubleValidator = new();

	public static Result<double, IValidationError> ValidateCoordinate(
		string input,
		string pointName,
		string axisName)
	{
		if (string.IsNullOrWhiteSpace(input))
		{
			return Result.Err<double, IValidationError>(
				new InputValidationError.EmptyCoordinate(pointName, axisName));
		}

		return DoubleValidator.Validate(input).MapErr(IValidationError (_) =>
			new InputValidationError.InvalidCoordinate(pointName, axisName, input));
	}
}

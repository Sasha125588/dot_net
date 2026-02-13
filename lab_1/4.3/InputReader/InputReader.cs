using _4._3.ErrorFormatters;
using _4._3.Validation;
using Utils;
using Utils.Common;

namespace _4._3.InputReader;

public static class InputReader
{
	public static (DateOnly StartDate, DateOnly EndDate) Prompt()
	{
		var startDate = PromptUntilValidDate("Введіть дату початку (DD.MM.YYYY): ", Validator.ValidateStartDate);
		var endDate = PromptUntilValidDate("Введіть дату кінця (DD.MM.YYYY): ",
			end => Validator.Validate(end, startDate));
		return (startDate, endDate);
	}

	private static DateOnly PromptUntilValidDate(
		string prompt,
		Func<string, Result<DateOnly, IValidationError>> validator)
		=> ConsoleInputReader.Prompt(prompt, validator, ValidationErrorFormatter.Format);
}

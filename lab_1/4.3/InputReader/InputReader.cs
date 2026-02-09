using _4._3.ErrorFormatters;
using _4._3.Errors;
using Utils;
using _4._3.Validation;

namespace _4._3.InputReader;

public static class InputReader
{
	private static DateOnly PromptForStartDate()
	{
		while (true)
		{
			Console.Write("Введіть дату початку (DD.MM.YYYY): ");
			var input = Console.ReadLine() ?? string.Empty;

			var validatedStartDate = Validator.ValidateStartDate(input);

			switch (validatedStartDate)
			{
				case Ok<DateOnly, ValidationError> ok:
					return ok.Value;
				case Err<DateOnly, ValidationError> err:
					var errMsg = ValidationErrorFormatter.Format(err.Error);

					Console.WriteLine($"❌ {errMsg}");
					Console.WriteLine("Спробуйте ще раз.");
					Console.WriteLine();
					break;
			}
		}
	}

	private static DateOnly PromptForEndDate(DateOnly startDate)
	{
		while (true)
		{
			Console.Write("Введіть дату кінця (DD.MM.YYYY): ");
			var input = Console.ReadLine() ?? string.Empty;

			var validatedEndDate = Validator.ValidateEndDate(input, startDate);

			switch (validatedEndDate)
			{
				case Ok<DateOnly, ValidationError> ok:
					return ok.Value;
				case Err<DateOnly, ValidationError> err:
					var errMsg = ValidationErrorFormatter.Format(err.Error);

					Console.WriteLine($"❌ {errMsg}");
					Console.WriteLine("Спробуйте ще раз.");
					Console.WriteLine();
					break;
			}
		}
	}

	public static (DateOnly StartDate, DateOnly EndDate) PromptUntilValid()
	{
		var startDate = PromptForStartDate();
		var endDate = PromptForEndDate(startDate);
		return (startDate, endDate);
	}
}

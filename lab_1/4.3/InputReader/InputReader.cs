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

			var result = Validator.ValidateStartDate(input);

			if (result.IsOk())
			{
				return result.Unwrap();
			}

			result.Match(
				onOk: _ => { },
				onErr: error =>
				{
					Console.WriteLine($"❌ {error.ToMessage()}");
					Console.WriteLine("Спробуйте ще раз.");
					Console.WriteLine();
				}
			);
		}
	}

	private static DateOnly PromptForEndDate(DateOnly startDate)
	{
		while (true)
		{
			Console.Write("Введіть дату кінця (DD.MM.YYYY): ");
			var input = Console.ReadLine() ?? string.Empty;

			var result = Validator.ValidateEndDate(input, startDate);

			if (result.IsOk())
			{
				return result.Unwrap();
			}

			result.Match(
				onOk: _ => { },
				onErr: error =>
				{
					Console.WriteLine($"❌ {error.ToMessage()}");
					Console.WriteLine("Спробуйте ще раз.");
					Console.WriteLine();
				}
			);
		}
	}

	public static (DateOnly StartDate, DateOnly EndDate) PromptUntilValid()
	{
		var startDate = PromptForStartDate();
		var endDate = PromptForEndDate(startDate);
		return (startDate, endDate);
	}
}

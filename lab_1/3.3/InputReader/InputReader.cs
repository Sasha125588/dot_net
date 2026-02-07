using Utils;
using _3._3.Errors;
using _3._3.Validation;

namespace _3._3.InputReader;

public static class InputReader
{
	private static Result<int, ValidationError> ReadHeight()
	{
		Console.Write("Введіть висоту прапора (мінімум 5, кратне 5): ");
		var input = Console.ReadLine() ?? string.Empty;
		return Validator.ValidateHeight(input);
	}

	public static int PromptUntilValid()
	{
		while (true)
		{
			var result = ReadHeight();

			if (result.IsOk())
			{
				return result.Unwrap();
			}

			result.Match(
				onOk: _ => { },
				onErr: error =>
				{
					Console.WriteLine($"❌ {error.ToMessage()}");
					Console.WriteLine();
				}
			);
		}
	}
}
using _3._3.ErrorFormatters;
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
			switch (ReadHeight())
			{
				case Ok<int, ValidationError> ok:
					return ok.Value;
				case Err<int, ValidationError> err:
					var errMsg = ValidationErrorFormatter.Format(err.Error);

					Console.WriteLine($"❌ {errMsg}");
					Console.WriteLine();
					break;
			}
		}
	}
}
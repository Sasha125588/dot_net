namespace Utils.Common;

public static class ConsoleInputReader
{
	public static T Prompt<T>(
		string prompt,
		IValidator<T> validator,
		Func<IValidationError, string> formatError) =>
		Prompt(prompt, validator.Validate, formatError);

	public static T Prompt<T>(
		string prompt,
		Func<string, Result<T, IValidationError>> validator,
		Func<IValidationError, string> formatError)
	{
		while (true)
		{
			Console.Write(prompt);
			var input = Console.ReadLine() ?? string.Empty;

			switch (validator(input))
			{
				case Ok<T, IValidationError> ok:
					return ok.Value;
				case Err<T, IValidationError> err:
					Console.WriteLine($"❌ {formatError(err.Error)}");
					Console.WriteLine("Спробуйте ще раз.");
					Console.WriteLine();
					break;
			}
		}
	}
}

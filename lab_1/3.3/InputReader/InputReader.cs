using _3._3.ErrorFormatters;
using Utils.Common;
using _3._3.Validation;

namespace _3._3.InputReader;

public static class InputReader
{
	public static int Prompt()
		=> ConsoleInputReader.Prompt(
			"Введіть висоту прапора (мінімум 5, кратне 5): ",
			Validator.Validate,
			ValidationErrorFormatter.Format);
}
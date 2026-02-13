using _1._3.ErrorFormatters;
using _1._3.Validation;
using Utils.Common;

namespace _1._3.InputReader;

public static class InputReader
{
	public static int PromptGrade() =>
		ConsoleInputReader.Prompt(
			"Введіть оцінку (0–100): ",
			input => Validator.Validate(input),
			ValidationErrorFormatter.Format);
}

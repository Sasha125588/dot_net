using _2.ErrorFormatters;
using _2.Validation;
using Utils.Common;

namespace _2.InputReader;

public static class InputReader
{
	public static Point PromptPoint(string pointName)
	{
		var x = PromptCoordinate(pointName, "X");
		var y = PromptCoordinate(pointName, "Y");
		return new Point(x, y);
	}

	private static double PromptCoordinate(string pointName, string axisName) =>
		ConsoleInputReader.Prompt(
			$"[{pointName}]: Введіть координату {axisName}: ",
			input => Validator.ValidateCoordinate(input, pointName, axisName),
			ValidationErrorFormatter.Format);
}

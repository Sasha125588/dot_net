using _2.Errors;
using Utils.Common;

namespace _2.ErrorFormatters;

public static class ValidationErrorFormatter
{
	public static string Format(IValidationError err) => err switch
	{
		InputValidationError.EmptyCoordinate e =>
			$"[ПОМИЛКА]: не введено значення {e.AxisName} для точки {e.PointName}",
		InputValidationError.InvalidCoordinate e =>
			$"[ПОМИЛКА]: некоректне значення {e.AxisName} для точки {e.PointName} ('{e.Input}')",
		_ => CommonValidationErrorFormatter.Format(err)
	};
}

public static class TriangleErrorFormatter
{
	public static string Format(IValidationError err) => err switch
	{
		TriangleError.Degenerate e =>
			$"[ПОМИЛКА]: точки {e.A}, {e.B}, {e.C} не утворюють трикутник",
		_ => "[ПОМИЛКА]: невідома помилка трикутника",
	};
}

namespace _1._3;

using ErrorFormatters;
using Validation;
using Utils;

public static class GradeHelper
{
	private const int ExcellentMin = 90;
	private const int GoodMin = 75;
	private const int SatisfactoryMin = 60;

	public static string ToFivePointScale(int grade)
		=> grade switch
		{
			>= ExcellentMin => "відмінно",
			>= GoodMin => "добре",
			>= SatisfactoryMin => "задовільно",
			_ => "незадовільно"
		};

	public static Result<int, string> TryParseGrade(string? input)
		=> Validator.Validate(input)
			.MapErr(ValidationErrorFormatter.Format);
}

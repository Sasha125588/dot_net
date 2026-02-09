namespace _1._3;

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
	{
		if (string.IsNullOrWhiteSpace(input))
			return Result.Err<int, string>("не введено значення");

		if (!int.TryParse(input, out var value))
			return Result.Err<int, string>("очікується ціле число");

		if (value is < 0 or > 100)
			return Result.Err<int, string>("має бути від 0 до 100");

		return Result.Ok<int, string>(value);
	}
}

using Utils;

namespace _1._3Test;

using _3;
using Xunit;

public class GradeConverterTests
{
	public static TheoryData<int, string> ToFivePointScaleTestData =>
		new()
		{
			{ 100, "відмінно" },
			{ 95, "відмінно" },
			{ 90, "відмінно" },
			{ 89, "добре" },
			{ 80, "добре" },
			{ 75, "добре" },
			{ 74, "задовільно" },
			{ 65, "задовільно" },
			{ 60, "задовільно" },
			{ 59, "незадовільно" },
			{ 50, "незадовільно" },
			{ 0, "незадовільно" }
		};

	[Theory]
	[MemberData(nameof(ToFivePointScaleTestData))]
	public void ToFivePointScale_WithGrade_ReturnsExpectedText(int grade, string expected)
	{
		var actual = GradeHelper.ToFivePointScale(grade);

		Assert.Equal(expected, actual);
	}
}

public class TryParseGradeTests
{
	public static TheoryData<string, int> ValidInputData =>
		new()
		{
			{ "0", 0 },
			{ "50", 50 },
			{ "100", 100 }
		};

	[Theory]
	[MemberData(nameof(ValidInputData))]
	public void TryParseGrade_WithValidInput_ReturnsOk(string input, int expected)
	{
		var result = GradeHelper.TryParseGrade(input);

		Assert.Equal(expected, result.ShouldBeOk());
	}

	[Theory]
	[InlineData(null)]
	[InlineData("")]
	[InlineData("   ")]
	public void TryParseGrade_WithEmptyOrNullInput_ReturnsErr(string? input)
	{
		var result = GradeHelper.TryParseGrade(input);

		var error = result.ShouldBeErr();
		Assert.Equal("не введено значення", error);
	}

	[Theory]
	[InlineData("abc")]
	[InlineData("12.5")]
	[InlineData("1a")]
	public void TryParseGrade_WithNonNumericInput_ReturnsErr(string input)
	{
		var result = GradeHelper.TryParseGrade(input);

		var error = result.ShouldBeErr();
		Assert.Equal($"Не вдалося розпізнати '{input}' як ціле число", error);
	}

	[Theory]
	[InlineData("-1")]
	[InlineData("101")]
	[InlineData("150")]
	public void TryParseGrade_WithOutOfRangeInput_ReturnsErr(string input)
	{
		var result = GradeHelper.TryParseGrade(input);

		var error = result.ShouldBeErr();
		Assert.Equal("має бути від 0 до 100", error);
	}
}

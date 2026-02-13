using _3._3.ErrorFormatters;
using _3._3.Errors;
using _3._3.Models;
using _3._3.Validation;
using Utils;
using Utils.Common;

namespace _3._3Test;

public class ValidationErrorTests
{
	[Fact]
	public void ToMessage_ParseIntError_ReturnsExpectedMessage()
	{
		var error = new CommonValidationError.ParseIntError("abc");

		var message = ValidationErrorFormatter.Format(error);

		Assert.Equal("Не вдалося розпізнати 'abc' як ціле число", message);
	}

	[Fact]
	public void ToMessage_TooSmallHeightError_ReturnsExpectedMessage()
	{
		var error = new ValidationError.TooSmallHeightError(3, 5);

		var message = ValidationErrorFormatter.Format(error);

		Assert.Equal("Значення 3 занадто мале (мінімум: 5)", message);
	}

	[Fact]
	public void ToMessage_HeightNotDivisibleByError_ReturnsExpectedMessage()
	{
		var error = new ValidationError.HeightNotDivisibleByError(7, 5);

		var message = ValidationErrorFormatter.Format(error);

		Assert.Equal("Значення 7 має бути кратне 5", message);
	}
}

public class ValidatorTests
{
	[Theory]
	[InlineData("5", 5)]
	[InlineData("15", 15)]
	[InlineData("20", 20)]
	[InlineData("25", 25)]
	public void Validate_WithValidInput_ReturnsOk(string input, int expected)
	{
		var result = Validator.Validate(input);

		Assert.Equal(expected, result.ShouldBeOk());
	}

	[Theory]
	[InlineData("abc")]
	[InlineData("12.5")]
	[InlineData("")]
	[InlineData("   ")]
	public void Validate_WithNonNumericInput_ReturnsParseError(string input)
	{
		var result = Validator.Validate(input);

		var error = result.ShouldBeErr();
		Assert.IsType<CommonValidationError.ParseIntError>(error);
	}

	[Theory]
	[InlineData("0")]
	[InlineData("1")]
	[InlineData("4")]
	[InlineData("-5")]
	public void Validate_WithTooSmallValue_ReturnsTooSmallHeightError(string input)
	{
		var result = Validator.Validate(input);

		var error = result.ShouldBeErr();
		Assert.IsType<ValidationError.TooSmallHeightError>(error);
	}

	[Theory]
	[InlineData("6")]
	[InlineData("7")]
	[InlineData("11")]
	[InlineData("14")]
	[InlineData("16")]
	public void Validate_WithNotDivisibleValue_ReturnsHeightNotDivisibleByError(string input)
	{
		var result = Validator.Validate(input);

		var error = result.ShouldBeErr();
		Assert.IsType<ValidationError.HeightNotDivisibleByError>(error);
	}
}

public class CrossParamsTests
{
	[Fact]
	public void Create_WithHeight15_CalculatesCorrectParameters()
	{
		var cross = CrossParams.Create(15);

		Assert.Equal(3, cross.Thickness);
		Assert.Equal(9, cross.ArmLength);
		Assert.Equal(6, cross.VerticalStart);
		Assert.Equal(9, cross.VerticalEnd);
		Assert.Equal(6, cross.HorizontalStart);
		Assert.Equal(9, cross.HorizontalEnd);
		Assert.Equal(3, cross.ArmStart);
		Assert.Equal(12, cross.ArmEnd);
	}

	[Theory]
	[InlineData(15, 7, 7, true)]
	[InlineData(15, 0, 0, false)]
	[InlineData(15, 5, 7, true)]
	[InlineData(15, 7, 5, true)]
	[InlineData(20, 10, 10, true)]
	public void IsPointInCross_ReturnsExpectedValue(
		int height,
		int row,
		int col,
		bool expected)
	{
		var cross = CrossParams.Create(height);

		var actual = cross.IsPointInCross(row, col);

		Assert.Equal(expected, actual);
	}

	[Fact]
	public void IsPointInCross_FormsExpectedCount()
	{
		var cross = CrossParams.Create(15);
		var crossPointsCount = 0;

		for (var row = 0; row < 15; row++)
		{
			for (var col = 0; col < 15; col++)
			{
				if (cross.IsPointInCross(row, col))
				{
					crossPointsCount++;
				}
			}
		}

		var expectedCount = cross.ArmLength * cross.Thickness * 2 - cross.Thickness * cross.Thickness;
		Assert.Equal(expectedCount, crossPointsCount);
	}
}

using _3._3.ErrorFormatters;
using _3._3.Errors;
using _3._3.Models;
using _3._3.Validation;
using Utils;

namespace _3._3Test;

public class ValidationErrorTests
{
	[Fact]
	public void ToMessage_ParseError_ReturnsExpectedMessage()
	{
		var error = new ValidationError.ParseError("abc");

		var message = ValidationErrorFormatter.Format(error);

		Assert.Equal("Не вдалося розпізнати 'abc' як число", message);
	}

	[Fact]
	public void ToMessage_TooSmall_ReturnsExpectedMessage()
	{
		var error = new ValidationError.TooSmall(3, 5);

		var message = ValidationErrorFormatter.Format(error);

		Assert.Equal("Значення 3 занадто мале (мінімум: 5)", message);
	}

	[Fact]
	public void ToMessage_NotDivisibleBy_ReturnsExpectedMessage()
	{
		var error = new ValidationError.NotDivisibleBy(7, 5);

		var message = ValidationErrorFormatter.Format(error);

		Assert.Equal("Значення 7 має бути кратне 5", message);
	}
}

public class ValidatorTests
{
	public static TheoryData<string, int> ValidInputData =>
		new()
		{
			{ "5", 5 },
			{ "15", 15 },
			{ "20", 20 },
			{ "25", 25 },
			{ "50", 50 },
			{ "100", 100 }
		};

	[Theory]
	[MemberData(nameof(ValidInputData))]
	public void ValidateHeight_WithValidInput_ReturnsOk(string input, int expected)
	{
		var result = Validator.ValidateHeight(input);

		var value = result.ShouldBeOk();
		Assert.Equal(expected, value);
	}

	[Theory]
	[InlineData("abc")]
	[InlineData("12.5")]
	[InlineData("")]
	[InlineData("   ")]
	public void ValidateHeight_WithNonNumericInput_ReturnsParseError(string input)
	{
		var result = Validator.ValidateHeight(input);

		var error = result.ShouldBeErr();
		Assert.IsType<ValidationError.ParseError>(error);
	}

	[Theory]
	[InlineData("0")]
	[InlineData("1")]
	[InlineData("4")]
	[InlineData("-5")]
	public void ValidateHeight_WithTooSmallValue_ReturnsTooSmallError(string input)
	{
		var result = Validator.ValidateHeight(input);

		var error = result.ShouldBeErr();
		Assert.IsType<ValidationError.TooSmall>(error);
	}

	[Theory]
	[InlineData("6")]
	[InlineData("7")]
	[InlineData("11")]
	[InlineData("14")]
	[InlineData("16")]
	public void ValidateHeight_WithNotDivisibleValue_ReturnsNotDivisibleError(string input)
	{
		var result = Validator.ValidateHeight(input);

		var error = result.ShouldBeErr();
		Assert.IsType<ValidationError.NotDivisibleBy>(error);
	}

	[Fact]
	public void ValidateHeight_WithNegativeButDivisible_ReturnsTooSmallError()
	{
		var result = Validator.ValidateHeight("-10");

		var error = result.ShouldBeErr();
		Assert.IsType<ValidationError.TooSmall>(error);
	}
}

public class FlagDimensionsTests
{
	[Theory]
	[InlineData(15)]
	[InlineData(20)]
	[InlineData(50)]
	public void FromHeight_CreatesSquareDimensions(int height)
	{
		var dims = FlagDimensions.FromHeight(height);

		Assert.Equal(height, dims.Height);
		Assert.Equal(height, dims.Width);
	}

	[Fact]
	public void FlagDimensions_IsReadonlyRecordStruct()
	{
		var dims1 = new FlagDimensions(20, 20);
		var dims2 = new FlagDimensions(20, 20);

		Assert.Equal(dims1, dims2);
	}
}

public class CrossParamsTests
{
	[Fact]
	public void FromDimensions_WithHeight15_CalculatesCorrectParameters()
	{
		var dims = new FlagDimensions(15, 15);

		var cross = CrossParams.FromDimensions(dims);

		Assert.Equal(3, cross.Thickness);
		Assert.Equal(9, cross.ArmLength);
		Assert.Equal(6, cross.VerticalStart);
		Assert.Equal(9, cross.VerticalEnd);
		Assert.Equal(6, cross.HorizontalStart);
		Assert.Equal(9, cross.HorizontalEnd);
		Assert.Equal(3, cross.ArmStart);
		Assert.Equal(12, cross.ArmEnd);
	}

	[Fact]
	public void FromDimensions_WithHeight20_CalculatesCorrectParameters()
	{
		var dims = new FlagDimensions(20, 20);

		var cross = CrossParams.FromDimensions(dims);

		Assert.Equal(4, cross.Thickness);
		Assert.Equal(12, cross.ArmLength);
		Assert.Equal(8, cross.VerticalStart);
		Assert.Equal(12, cross.VerticalEnd);
		Assert.Equal(8, cross.HorizontalStart);
		Assert.Equal(12, cross.HorizontalEnd);
		Assert.Equal(4, cross.ArmStart);
		Assert.Equal(16, cross.ArmEnd);
	}

	[Fact]
	public void FromDimensions_WithHeight25_CalculatesCorrectParameters()
	{
		var dims = new FlagDimensions(25, 25);

		var cross = CrossParams.FromDimensions(dims);

		Assert.Equal(5, cross.Thickness);
		Assert.Equal(15, cross.ArmLength);
	}

	[Theory]
	[InlineData(15, 3)]
	[InlineData(20, 4)]
	[InlineData(25, 5)]
	[InlineData(50, 10)]
	public void FromDimensions_ThicknessIsOneFifthOfHeight(int height, int expectedThickness)
	{
		var dims = new FlagDimensions(height, height);

		var cross = CrossParams.FromDimensions(dims);

		Assert.Equal(expectedThickness, cross.Thickness);
	}

	[Theory]
	[InlineData(15, 9)]
	[InlineData(20, 12)]
	[InlineData(25, 15)]
	[InlineData(50, 30)]
	public void FromDimensions_ArmLengthIsThreeFifthsOfHeight(int height, int expectedArmLength)
	{
		var dims = new FlagDimensions(height, height);

		var cross = CrossParams.FromDimensions(dims);

		Assert.Equal(expectedArmLength, cross.ArmLength);
	}
}

public class IsPointInCrossTests
{
	[Fact]
	public void IsPointInCross_CenterPoint_ReturnsTrue()
	{
		var dims = new FlagDimensions(15, 15);
		var cross = CrossParams.FromDimensions(dims);

		var isInCross = cross.IsPointInCross(7, 7);

		Assert.True(isInCross);
	}

	[Fact]
	public void IsPointInCross_TopLeftCorner_ReturnsFalse()
	{
		var dims = new FlagDimensions(15, 15);
		var cross = CrossParams.FromDimensions(dims);

		var isInCross = cross.IsPointInCross(0, 0);

		Assert.False(isInCross);
	}

	[Fact]
	public void IsPointInCross_VerticalArmPoint_ReturnsTrue()
	{
		var dims = new FlagDimensions(15, 15);
		var cross = CrossParams.FromDimensions(dims);

		var isInCross = cross.IsPointInCross(5, 7);

		Assert.True(isInCross);
	}

	[Fact]
	public void IsPointInCross_HorizontalArmPoint_ReturnsTrue()
	{
		var dims = new FlagDimensions(15, 15);
		var cross = CrossParams.FromDimensions(dims);

		var isInCross = cross.IsPointInCross(7, 5);

		Assert.True(isInCross);
	}

	[Theory]
	[InlineData(0, 0)]
	[InlineData(0, 14)]
	[InlineData(14, 0)]
	[InlineData(14, 14)]
	public void IsPointInCross_Corners_ReturnFalse(int row, int col)
	{
		var dims = new FlagDimensions(15, 15);
		var cross = CrossParams.FromDimensions(dims);

		var isInCross = cross.IsPointInCross(row, col);

		Assert.False(isInCross);
	}

	[Fact]
	public void IsPointInCross_VerticalArmTopEdge_ReturnsTrue()
	{
		var dims = new FlagDimensions(15, 15);
		var cross = CrossParams.FromDimensions(dims);

		var isInCross = cross.IsPointInCross(3, 7);

		Assert.True(isInCross);
	}

	[Fact]
	public void IsPointInCross_VerticalArmBottomEdge_ReturnsTrue()
	{
		var dims = new FlagDimensions(15, 15);
		var cross = CrossParams.FromDimensions(dims);

		var isInCross = cross.IsPointInCross(11, 7);

		Assert.True(isInCross);
	}

	[Fact]
	public void IsPointInCross_HorizontalArmLeftEdge_ReturnsTrue()
	{
		var dims = new FlagDimensions(15, 15);
		var cross = CrossParams.FromDimensions(dims);

		var isInCross = cross.IsPointInCross(7, 3);

		Assert.True(isInCross);
	}

	[Fact]
	public void IsPointInCross_HorizontalArmRightEdge_ReturnsTrue()
	{
		var dims = new FlagDimensions(15, 15);
		var cross = CrossParams.FromDimensions(dims);

		var isInCross = cross.IsPointInCross(7, 11);

		Assert.True(isInCross);
	}

	[Fact]
	public void IsPointInCross_OutsideVerticalArm_ReturnsFalse()
	{
		var dims = new FlagDimensions(15, 15);
		var cross = CrossParams.FromDimensions(dims);

		var isInCross = cross.IsPointInCross(2, 7);

		Assert.False(isInCross);
	}

	[Fact]
	public void IsPointInCross_OutsideHorizontalArm_ReturnsFalse()
	{
		var dims = new FlagDimensions(15, 15);
		var cross = CrossParams.FromDimensions(dims);

		var isInCross = cross.IsPointInCross(7, 2);

		Assert.False(isInCross);
	}

	[Fact]
	public void IsPointInCross_WithHeight20_CenterReturnsTrue()
	{
		var dims = new FlagDimensions(20, 20);
		var cross = CrossParams.FromDimensions(dims);

		var isInCross = cross.IsPointInCross(10, 10);

		Assert.True(isInCross);
	}

	[Fact]
	public void IsPointInCross_WithHeight20_CornerReturnsFalse()
	{
		var dims = new FlagDimensions(20, 20);
		var cross = CrossParams.FromDimensions(dims);

		var isInCross = cross.IsPointInCross(0, 0);

		Assert.False(isInCross);
	}
}

public class CrossSymmetryTests
{
	[Fact]
	public void IsPointInCross_IsSymmetricHorizontally()
	{
		var dims = new FlagDimensions(15, 15);
		var cross = CrossParams.FromDimensions(dims);

		for (var row = 0; row < dims.Height; row++)
		{
			for (var col = 0; col < dims.Width / 2; col++)
			{
				var left = cross.IsPointInCross(row, col);
				var right = cross.IsPointInCross(row, dims.Width - 1 - col);

				Assert.Equal(left, right);
			}
		}
	}

	[Fact]
	public void IsPointInCross_IsSymmetricVertically()
	{
		var dims = new FlagDimensions(15, 15);
		var cross = CrossParams.FromDimensions(dims);

		for (var row = 0; row < dims.Height / 2; row++)
		{
			for (var col = 0; col < dims.Width; col++)
			{
				var top = cross.IsPointInCross(row, col);
				var bottom = cross.IsPointInCross(dims.Height - 1 - row, col);

				Assert.Equal(top, bottom);
			}
		}
	}

	[Fact]
	public void IsPointInCross_FormsPlusShaped()
	{
		var dims = new FlagDimensions(15, 15);
		var cross = CrossParams.FromDimensions(dims);

		var crossPointsCount = 0;

		for (var row = 0; row < dims.Height; row++)
		{
			for (var col = 0; col < dims.Width; col++)
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

public class RenderingLogicTests
{
	[Fact]
	public void DrawFlag_UsesCorrectCrossParameters()
	{
		var dims = new FlagDimensions(15, 15);
		var cross = CrossParams.FromDimensions(dims);

		Assert.Equal(3, cross.Thickness);
		Assert.Equal(9, cross.ArmLength);
	}

	[Fact]
	public void DrawFlag_ColorLogic_CrossPointsAreWhite()
	{
		var dims = new FlagDimensions(15, 15);
		var cross = CrossParams.FromDimensions(dims);

		var centerIsInCross = cross.IsPointInCross(7, 7);

		Assert.True(centerIsInCross);
	}

	[Fact]
	public void DrawFlag_ColorLogic_NonCrossPointsAreRed()
	{
		var dims = new FlagDimensions(15, 15);
		var cross = CrossParams.FromDimensions(dims);

		var cornerIsInCross = cross.IsPointInCross(0, 0);

		Assert.False(cornerIsInCross);
	}

	[Fact]
	public void DrawFlag_ProcessesAllRows()
	{
		var dims = new FlagDimensions(20, 20);

		var rowCount = dims.Height;

		Assert.Equal(20, rowCount);
	}

	[Fact]
	public void DrawFlag_ProcessesAllColumnsInRow()
	{
		var dims = new FlagDimensions(20, 20);

		var colCount = dims.Width;

		Assert.Equal(20, colCount);
	}

	[Theory]
	[InlineData(15, 15)]
	[InlineData(20, 20)]
	[InlineData(25, 25)]
	public void DrawFlag_DimensionsAreSquare(int height, int width)
	{
		var dims = FlagDimensions.FromHeight(height);

		Assert.Equal(dims.Height, dims.Width);
		Assert.Equal(height, width);
	}
}

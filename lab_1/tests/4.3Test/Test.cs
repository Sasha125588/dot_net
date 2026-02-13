using _4._3.ErrorFormatters;
using _4._3.Models;
using _4._3.Validation;
using Utils;
using Utils.Common;

namespace _4._3Test;

public class ValidationErrorTests
{
	[Fact]
	public void ToMessage_ParseDateOnlyError_ReturnsExpectedMessage()
	{
		var error = new CommonValidationError.ParseDateOnlyError("abc");

		var message = ValidationErrorFormatter.Format(error);

		Assert.Equal("Не вдалося розпізнати 'abc' як дату", message);
	}

	[Fact]
	public void ToMessage_ValidateStartDateAfterEndDate_ReturnsExpectedMessage()
	{
		var startDate = new DateOnly(2023, 12, 31);
		var endDate = new DateOnly(2023, 1, 1);
		var error = new DateRange.ValidateStartDateAfterEndDateError(startDate, endDate);

		var message = ValidationErrorFormatter.Format(error);

		Assert.Equal("Дата Початку(31.12.2023) повинна бути раніше за дату Кінця(01.01.2023)", message);
	}
}

public class ValidatorTests
{
	public static TheoryData<string, DateOnly> ValidStartDateData =>
		new()
		{
			{ "01.01.2023", new DateOnly(2023, 1, 1) },
			{ "15.06.2023", new DateOnly(2023, 6, 15) },
			{ "31.12.2023", new DateOnly(2023, 12, 31) },
			{ "29.02.2024", new DateOnly(2024, 2, 29) }
		};

	[Theory]
	[MemberData(nameof(ValidStartDateData))]
	public void ValidateStartDate_WithValidInput_ReturnsOk(string input, DateOnly expected)
	{
		var result = Validator.ValidateStartDate(input);

		var value = result.ShouldBeOk();
		Assert.Equal(expected, value);
	}

	[Theory]
	[InlineData("abc")]
	[InlineData("32.01.2023")]
	[InlineData("01.13.2023")]
	[InlineData("")]
	[InlineData("   ")]
	[InlineData("not-a-date")]
	public void ValidateStartDate_WithInvalidInput_ReturnsParseError(string input)
	{
		var result = Validator.ValidateStartDate(input);

		var error = result.ShouldBeErr();
		Assert.IsType<CommonValidationError.ParseDateOnlyError>(error);
	}

	public static TheoryData<string, DateOnly, DateOnly> ValidEndDateData =>
		new()
		{
			{ "31.12.2023", new DateOnly(2023, 1, 1), new DateOnly(2023, 12, 31) },
			{ "15.06.2024", new DateOnly(2023, 6, 15), new DateOnly(2024, 6, 15) },
			{ "02.01.2023", new DateOnly(2023, 1, 1), new DateOnly(2023, 1, 2) }
		};

	[Theory]
	[MemberData(nameof(ValidEndDateData))]
	public void ValidateEndDate_WithValidInput_ReturnsOk(string input, DateOnly startDate, DateOnly expected)
	{
		var result = Validator.Validate(input, startDate);

		var value = result.ShouldBeOk();
		Assert.Equal(expected, value);
	}

	[Theory]
	[InlineData("abc")]
	[InlineData("32.01.2023")]
	[InlineData("")]
	[InlineData("not-a-date")]
	public void ValidateEndDate_WithInvalidInput_ReturnsParseError(string input)
	{
		var startDate = new DateOnly(2023, 1, 1);

		var result = Validator.Validate(input, startDate);

		var error = result.ShouldBeErr();
		Assert.IsType<CommonValidationError.ParseDateOnlyError>(error);
	}

	[Theory]
	[InlineData("01.01.2023", "01.01.2023")]
	[InlineData("01.01.2023", "31.12.2023")]
	[InlineData("15.06.2023", "15.06.2024")]
	public void ValidateEndDate_WhenEndDateNotAfterStartDate_ReturnsValidationError(
		string endDateInput,
		string startDateInput)
	{
		var startDate = DateOnly.Parse(startDateInput);

		var result = Validator.Validate(endDateInput, startDate);

		var error = result.ShouldBeErr();
		Assert.IsType<DateRange.ValidateStartDateAfterEndDateError>(error);
	}

	[Fact]
	public void ValidateEndDate_WithEndDateBeforeStartDate_ReturnsCorrectError()
	{
		var startDate = new DateOnly(2023, 12, 31);
		var endDateInput = "01.01.2023";

		var result = Validator.Validate(endDateInput, startDate);

		var error = result.ShouldBeErr();
		var validationError = Assert.IsType<DateRange.ValidateStartDateAfterEndDateError>(error);
		Assert.Equal(startDate, validationError.StartDate);
		Assert.Equal(new DateOnly(2023, 1, 1), validationError.EndDate);
	}
}

public class DateRangeTests
{
	[Theory]
	[InlineData("01.01.2023", "31.12.2023", 0)]
	[InlineData("01.01.2023", "01.01.2024", 1)]
	[InlineData("15.06.2020", "15.06.2025", 5)]
	[InlineData("01.01.2020", "31.12.2020", 0)]
	public void YearsBetween_ReturnsCorrectYearDifference(
		string startDateStr,
		string endDateStr,
		int expectedYears)
	{
		var startDate = DateOnly.Parse(startDateStr);
		var endDate = DateOnly.Parse(endDateStr);
		var range = new DateRange(startDate, endDate);

		var years = range.YearsBetween();

		Assert.Equal(expectedYears, years);
	}

	[Fact]
	public void YearsBetween_WithSameDate_ReturnsZero()
	{
		var date = new DateOnly(2023, 6, 15);
		var range = new DateRange(date, date);

		var years = range.YearsBetween();

		Assert.Equal(0, years);
	}

	[Fact]
	public void YearsBetween_WithLeapYearBoundary_CalculatesCorrectly()
	{
		var startDate = new DateOnly(2020, 2, 29);
		var endDate = new DateOnly(2024, 2, 29);
		var range = new DateRange(startDate, endDate);

		var years = range.YearsBetween();

		Assert.Equal(4, years);
	}

	[Fact]
	public void YearsBetween_WhenEndDateBeforeAnniversary_ReturnsOneLess()
	{
		var startDate = new DateOnly(2020, 6, 15);
		var endDate = new DateOnly(2023, 6, 14);
		var range = new DateRange(startDate, endDate);

		var years = range.YearsBetween();

		Assert.Equal(2, years);
	}

	[Fact]
	public void YearsBetween_WhenEndDateOnAnniversary_ReturnsExactYears()
	{
		var startDate = new DateOnly(2020, 6, 15);
		var endDate = new DateOnly(2023, 6, 15);
		var range = new DateRange(startDate, endDate);

		var years = range.YearsBetween();

		Assert.Equal(3, years);
	}

	[Fact]
	public void YearsBetween_WhenEndDateAfterAnniversary_ReturnsExactYears()
	{
		var startDate = new DateOnly(2020, 6, 15);
		var endDate = new DateOnly(2023, 6, 16);
		var range = new DateRange(startDate, endDate);

		var years = range.YearsBetween();

		Assert.Equal(3, years);
	}

	[Fact]
	public void From_CreatesDateRangeWithCorrectDates()
	{
		var startDate = new DateOnly(2023, 1, 1);
		var endDate = new DateOnly(2023, 12, 31);

		var range = DateRange.Create(startDate, endDate).ShouldBeOk();

		Assert.Equal(startDate, range.StartDate);
		Assert.Equal(endDate, range.EndDate);
	}

	[Fact]
	public void DateRange_IsRecordWithValueSemantics()
	{
		var startDate = new DateOnly(2023, 1, 1);
		var endDate = new DateOnly(2023, 12, 31);
		var range1 = new DateRange(startDate, endDate);
		var range2 = new DateRange(startDate, endDate);

		Assert.Equal(range1, range2);
	}
}

public class DateRangeEdgeCasesTests
{
	[Fact]
	public void YearsBetween_WithVeryLongRange_CalculatesCorrectly()
	{
		var startDate = new DateOnly(1900, 1, 1);
		var endDate = new DateOnly(2023, 12, 31);
		var range = new DateRange(startDate, endDate);

		var years = range.YearsBetween();

		Assert.Equal(123, years);
	}

	[Fact]
	public void YearsBetween_WithOneDayDifference_ReturnsZero()
	{
		var startDate = new DateOnly(2023, 6, 15);
		var endDate = new DateOnly(2023, 6, 16);
		var range = new DateRange(startDate, endDate);

		var years = range.YearsBetween();

		Assert.Equal(0, years);
	}

	[Fact]
	public void YearsBetween_WithAlmostOneYear_ReturnsZero()
	{
		var startDate = new DateOnly(2023, 1, 1);
		var endDate = new DateOnly(2023, 12, 31);
		var range = new DateRange(startDate, endDate);

		var years = range.YearsBetween();

		Assert.Equal(0, years);
	}

	[Fact]
	public void YearsBetween_WithExactlyOneYear_ReturnsOne()
	{
		var startDate = new DateOnly(2023, 1, 1);
		var endDate = new DateOnly(2024, 1, 1);
		var range = new DateRange(startDate, endDate);

		var years = range.YearsBetween();

		Assert.Equal(1, years);
	}
}

namespace _4._3.Models;

public sealed record DateRange(DateOnly StartDate, DateOnly EndDate)
{
	public int YearsBetween()
	{
		var years = EndDate.Year - StartDate.Year;

		if (EndDate < StartDate.AddYears(years))
		{
			years--;
		}

		return years;
	}

	public static DateRange From(DateOnly startDate, DateOnly endDate) =>
		new(startDate, endDate);
}

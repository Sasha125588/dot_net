using _4._3.Models;

namespace _4._3.Rendering;

public static class Rendering
{
	public static void Draw(DateRange range)
	{
		Console.WriteLine();
		Console.WriteLine("═══════════════════════════════════════");
		Console.WriteLine($"📅 Дата початку: {range.StartDate:dd.MM.yyyy}");
		Console.WriteLine($"📅 Дата кінця:   {range.EndDate:dd.MM.yyyy}");
		Console.WriteLine("───────────────────────────────────────");
		Console.WriteLine($"⏱️  Кількість років між датами: {range.YearsBetween()}");
		Console.WriteLine("═══════════════════════════════════════");
	}
}

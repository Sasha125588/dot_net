using _3._3.Models;

namespace _3._3.Rendering;

public static class Rendering
{
	private static void SetConsoleColor(this Color color)
	{
		(Console.BackgroundColor, Console.ForegroundColor) = color switch
		{
			Color.Red => (ConsoleColor.Red, ConsoleColor.Red),
			Color.White => (ConsoleColor.White, ConsoleColor.White),
			_ => throw new ArgumentOutOfRangeException(nameof(color))
		};
	}

	private static void DrawPixel(Color color)
	{
		// SetConsoleColor(color); 
		color.SetConsoleColor(); // [ЗАПИТАТИ]
		Console.Write("█");
	}

	private static void DrawRow(int row, FlagDimensions dims, CrossParams cross)
	{
		for (var col = 0; col < dims.Width; col++)
		{
			var color = cross.IsPointInCross(row, col) ? Color.White : Color.Red;
			DrawPixel(color);
		}

		Console.ResetColor();
		Console.WriteLine();
	}

	public static void DrawFlag(FlagDimensions dims)
	{
		var cross = CrossParams.FromDimensions(dims);

		for (var row = 0; row < dims.Height; row++)
		{
			DrawRow(row, dims, cross);
		}

		Console.ResetColor();
	}
}
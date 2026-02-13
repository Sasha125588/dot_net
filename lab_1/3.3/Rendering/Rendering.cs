using _3._3.Models;

namespace _3._3.Rendering;

public static class Rendering
{
	public static void Draw(int height, CrossParams cross)
	{
		for (var row = 0; row < height; row++)
		{
			for (var col = 0; col < height; col++)
			{
				var color = cross.IsPointInCross(row, col) ? ConsoleColor.White : ConsoleColor.Red;

				Console.BackgroundColor = color;
				Console.ForegroundColor = color;
				Console.Write("  ");
			}

			Console.ResetColor();
			Console.WriteLine();
		}

		Console.ResetColor();
	}
}
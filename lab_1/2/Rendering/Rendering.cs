namespace _2.Rendering;

public static class Rendering
{
	public static void Draw(Triangle triangle)
	{
		Console.WriteLine($"Площа: {triangle.Area():F2}");
		Console.WriteLine($"Периметр: {triangle.Perimeter():F2}");
	}
}

// dotnet run --project=./lab_1/2

using Utils;
using _2;

var result = ReadPointFromConsole("A")
	.AndThen(a => ReadPointFromConsole("B")
		.AndThen(b => ReadPointFromConsole("C")
			.AndThen(c => Triangle.Create(a, b, c)
				.MapError(MapTriangleErrorToString))));


result.Match(
	triangle =>
	{
		Console.WriteLine($"Площа: {triangle.Area():F2}");
		Console.WriteLine($"Периметр: {triangle.Perimeter():F2}");
	},
	error => Console.WriteLine(error)
);

return;

Result<Point, string> ReadPointFromConsole(string pointName)
{
	return ReadCoordinate(new Coordinate(pointName, "X"))
		.AndThen(x => ReadCoordinate(new Coordinate(pointName, "Y"))
			.AndThen(y => new Result<Point, string>.Ok(new Point(x, y))));
}

Result<int, string> ReadCoordinate(Coordinate coord)
{
	Console.WriteLine($"[{coord.Name}]: Введіть координату {coord.AxisName}:");
	return ParseCoordinate(Console.ReadLine(), coord);
}

Result<int, string> ParseCoordinate(string? input, Coordinate coord)
{
	if (!int.TryParse(input, out var value))
		return new Result<int, string>.Err($"[ПОМИЛКА]: некоректне значення {coord.AxisName} для точки {coord.Name}");
	return new Result<int, string>.Ok(value);
}

static string MapTriangleErrorToString(TriangleError error) => error switch
{
	TriangleError.Degenerate e =>
		$"[ПОМИЛКА]: точки {e.A}, {e.B}, {e.C} не утворюють трикутник",
	_ => "[ПОМИЛКА]: невідома помилка трикутника"
};

public record Coordinate(string Name, string AxisName);
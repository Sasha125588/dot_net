// dotnet run --project=./lab_1/2

using Utils;
using _2;

var pointResults = new[] { "A", "B", "C" }
	.Select(ReadPointFromConsole)
	.Collect();

var result = pointResults
	.AndThen(points => Triangle.Create(points[0], points[1], points[2])
		.MapErr(MapTriangleErrorToString));


// var result = ReadPointFromConsole("A")
// 	.AndThen(a => ReadPointFromConsole("B")
// 		.AndThen(b => ReadPointFromConsole("C")
// 			.AndThen(c => Triangle.Create(a, b, c)
// 				.MapErr(MapTriangleErrorToString))));


result.Match(
	triangle =>
	{
		Console.WriteLine($"Площа: {triangle.Area():F2}");
		Console.WriteLine($"Периметр: {triangle.Perimeter():F2}");
	},
	error => Console.WriteLine(error)
);

return;

Result<Point, string> ReadPointFromConsole(string pointName) =>
	ReadCoordinate(new Coordinate(pointName, "X"))
		.AndThen(x => ReadCoordinate(new Coordinate(pointName, "Y"))
			.Map(y => new Point(x, y)));


Result<int, string> ReadCoordinate(Coordinate coord)
{
	Console.WriteLine($"[{coord.Name}]: Введіть координату {coord.AxisName}:");
	return ParseCoordinate(Console.ReadLine(), coord);
}

Result<int, string> ParseCoordinate(string? input, Coordinate coord) =>
	int.TryParse(input, out var value)
		? Result.Ok<int, string>(value)
		: Result.Err<int, string>($"[ПОМИЛКА]: некоректне значення {coord.AxisName} для точки {coord.Name}");


static string MapTriangleErrorToString(TriangleError error) => error switch
{
	TriangleError.Degenerate e =>
		$"[ПОМИЛКА]: точки {e.A}, {e.B}, {e.C} не утворюють трикутник",
	_ => "[ПОМИЛКА]: невідома помилка трикутника"
};

public record Coordinate(string Name, string AxisName);
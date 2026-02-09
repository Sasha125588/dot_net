namespace _2;

using Utils;

public class Triangle
{
	public Point A { get; }
	public Point B { get; }
	public Point C { get; }

	private readonly double _ab;
	private readonly double _bc;
	private readonly double _ca;

	private Triangle(Point a, Point b, Point c)
	{
		A = a;
		B = b;
		C = c;

		_ab = A.DistanceTo(B);
		_bc = B.DistanceTo(C);
		_ca = C.DistanceTo(A);
	}

	public static Result<Triangle, TriangleError> Create(Point a, Point b, Point c) =>
		IsValid(a, b, c)
			? Result.Ok<Triangle, TriangleError>(new Triangle(a, b, c))
			: Result.Err<Triangle, TriangleError>(new TriangleError.Degenerate(a, b, c));


	public double Area()
	{
		var p = Perimeter() / 2;
		return Math.Sqrt(p * (p - _ab) * (p - _bc) * (p - _ca));
	}

	public double Perimeter()
		=> _ab + _bc + _ca;

	// public double Area()
	// {
	// 	var a = A.DistanceTo(B);
	// 	var b = B.DistanceTo(C);
	// 	var c = C.DistanceTo(A);
	// 	var p = (a + b + c) / 2;
	// 	return Math.Sqrt(p * (p - a) * (p - b) * (p - c));
	// }

	// public double Perimeter()
	// 	=> A.DistanceTo(B) + B.DistanceTo(C) + C.DistanceTo(A);

	private static bool IsValid(Point a, Point b, Point c)
	{
		const double epsilon = 1e-6;
		var ab = a.DistanceTo(b);
		var bc = b.DistanceTo(c);
		var ca = c.DistanceTo(a);

		return ab + bc - ca > epsilon &&
		       ab + ca - bc > epsilon &&
		       bc + ca - ab > epsilon;
	}
}
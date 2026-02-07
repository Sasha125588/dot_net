namespace _2;

public record Point(double X, double Y)
{
	public double DistanceTo(Point other)
	{
		var dx = X - other.X;
		var dy = Y - other.Y;

		return Math.Sqrt(dx * dx + dy * dy);
	}
}
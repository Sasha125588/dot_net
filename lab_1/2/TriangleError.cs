namespace _2;

public abstract record TriangleError
{
	public sealed record Degenerate(Point A, Point B, Point C) : TriangleError;
}

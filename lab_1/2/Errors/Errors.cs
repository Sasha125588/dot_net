using Utils.Common;

namespace _2.Errors;

public abstract record InputValidationError : IValidationError
{
	public sealed record EmptyCoordinate(string PointName, string AxisName) : InputValidationError;

	public sealed record InvalidCoordinate(string PointName, string AxisName, string Input) : InputValidationError;
}

public abstract record TriangleError : IValidationError
{
	public sealed record Degenerate(Point A, Point B, Point C) : TriangleError;
}
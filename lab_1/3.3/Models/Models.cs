namespace _3._3.Models;

public readonly record struct FlagDimensions(int Height, int Width)
{
	public static FlagDimensions FromHeight(int height) => new(height, height);
}

public enum Color
{
	Red,
	White
}

public readonly record struct CrossParams
{
	public int Thickness { get; init; }
	public int ArmLength { get; init; }
	public int VerticalStart { get; init; }
	public int VerticalEnd { get; init; }
	public int HorizontalStart { get; init; }
	public int HorizontalEnd { get; init; }
	public int ArmStart { get; init; }
	public int ArmEnd { get; init; }

	public static CrossParams FromDimensions(FlagDimensions dims)
	{
		var thickness = dims.Height / 5;
		var armLength = dims.Height * 3 / 5;

		return new CrossParams
		{
			Thickness = thickness,
			ArmLength = armLength,
			VerticalStart = (dims.Height - thickness) / 2,
			VerticalEnd = (dims.Height - thickness) / 2 + thickness,
			HorizontalStart = (dims.Width - thickness) / 2,
			HorizontalEnd = (dims.Width - thickness) / 2 + thickness,
			ArmStart = (dims.Height - armLength) / 2,
			ArmEnd = (dims.Height - armLength) / 2 + armLength
		};
	}

	public bool IsPointInCross(int row, int col) =>
		(col >= HorizontalStart && col < HorizontalEnd && row >= ArmStart && row < ArmEnd) ||
		(row >= VerticalStart && row < VerticalEnd && col >= ArmStart && col < ArmEnd);
}

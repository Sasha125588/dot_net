namespace _3._3.Models;

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

	public static CrossParams Create(int height)
	{
		var thickness = height / 5;
		var armLength = height * 3 / 5;

		return new CrossParams
		{
			Thickness = thickness,
			ArmLength = armLength,

			VerticalStart = (height - thickness) / 2,
			VerticalEnd = (height - thickness) / 2 + thickness,

			HorizontalStart = (height - thickness) / 2,
			HorizontalEnd = (height - thickness) / 2 + thickness,

			ArmStart = (height - armLength) / 2,
			ArmEnd = (height - armLength) / 2 + armLength
		};
	}

	public bool IsPointInCross(int row, int col) =>
		(col >= HorizontalStart && col < HorizontalEnd && row >= ArmStart && row < ArmEnd) ||
		(row >= VerticalStart && row < VerticalEnd && col >= ArmStart && col < ArmEnd);
}

namespace _2Test;

using _2;
using Utils;
using Xunit;

public class PointTests
{
	[Theory]
	[InlineData(0, 0, 3, 0, 3)]
	[InlineData(0, 0, 0, 4, 4)]
	[InlineData(0, 0, 3, 4, 5)]
	[InlineData(-3, 0, 0, 0, 3)]
	[InlineData(-3, -4, 0, 0, 5)]
	public void DistanceTo_WithTwoPoints_ReturnsCorrectDistance(
		int x1, int y1, int x2, int y2, double expected)
	{
		var a = new Point(x1, y1);
		var b = new Point(x2, y2);

		var actual = a.DistanceTo(b);

		Assert.Equal(expected, actual, precision: 5);
	}

	[Fact]
	public void DistanceTo_SamePoint_ReturnsZero()
	{
		var p = new Point(1, 2);
		var actual = p.DistanceTo(p);
		Assert.Equal(0, actual);
	}

	[Fact]
	public void DistanceTo_IsSymmetric()
	{
		var a = new Point(1, 2);
		var b = new Point(4, 6);

		var distanceAB = a.DistanceTo(b);
		var distanceBA = b.DistanceTo(a);

		Assert.Equal(distanceAB, distanceBA, precision: 5);
	}
}

public class TriangleCreateTests
{
	[Fact]
	public void Create_ValidTriangle_ReturnsOk()
	{
		var result = Triangle.Create(
			new Point(0, 0),
			new Point(3, 0),
			new Point(0, 4));

		var triangle = result.ShouldBeOk();

		Assert.True(triangle.Perimeter() > 0);
		Assert.True(triangle.Area() > 0);
	}

	[Theory]
	[InlineData(0, 0, 1, 1, 2, 2)]
	[InlineData(0, 0, 0, 5, 0, 10)]
	[InlineData(5, 5, 5, 5, 5, 5)]
	public void Create_InvalidTriangle_ReturnsErr(
		int ax, int ay,
		int bx, int by,
		int cx, int cy)
	{
		var result = Triangle.Create(
			new Point(ax, ay),
			new Point(bx, by),
			new Point(cx, cy));

		result.ShouldBeErr();
	}
}

public class TriangleAreaPerimeterTests
{
	[Theory]
	[InlineData(0, 0, 3, 0, 0, 4, 6)]
	[InlineData(-1, 0, 1, 0, 0, 2, 2)]
	public void Area_ValidTriangles_ReturnsExpectedValue(
		int ax, int ay,
		int bx, int by,
		int cx, int cy,
		double expectedArea)
	{
		var triangle = Triangle.Create(
				new Point(ax, ay),
				new Point(bx, by),
				new Point(cx, cy))
			.ShouldBeOk();

		var area = triangle.Area();

		Assert.Equal(expectedArea, area, precision: 5);
	}

	[Fact]
	public void Area_IsInvariantToPointOrder()
	{
		var t1 = Triangle.Create(
			new Point(0, 0),
			new Point(3, 0),
			new Point(0, 4)).ShouldBeOk();

		var t2 = Triangle.Create(
			new Point(3, 0),
			new Point(0, 4),
			new Point(0, 0)).ShouldBeOk();

		Assert.Equal(t1.Area(), t2.Area(), precision: 5);
	}

	[Fact]
	public void Perimeter_AlwaysPositive_ForValidTriangle()
	{
		var triangle = Triangle.Create(
				new Point(-2, -1),
				new Point(4, 0),
				new Point(1, 3))
			.ShouldBeOk();

		Assert.True(triangle.Perimeter() > 0);
	}

	[Fact]
	public void Perimeter_DoesNotDependOnPointOrder()
	{
		var p1 = new Point(0, 0);
		var p2 = new Point(3, 0);
		var p3 = new Point(0, 4);

		var t1 = Triangle.Create(p1, p2, p3).ShouldBeOk();
		var t2 = Triangle.Create(p3, p1, p2).ShouldBeOk();

		Assert.Equal(t1.Perimeter(), t2.Perimeter(), precision: 5);
	}
}

public class ResultExtensionsTests
{
	[Fact]
	public void AndThen_WhenOk_CallsBinder()
	{
		var result = Result.Ok<int, string>(10);

		var mapped = result.AndThen(x =>
			Result.Ok<int, string>(x * 2));

		Assert.Equal(20, mapped.ShouldBeOk());
	}

	[Fact]
	public void AndThen_WhenErr_DoesNotCallBinder()
	{
		var called = false;
		var result = Result.Err<int, string>("error");

		var mapped = result.AndThen(x =>
		{
			called = true;
			return Result.Ok<int, string>(x);
		});

		Assert.False(called);
		mapped.ShouldBeErr();
	}

	[Fact]
	public void Match_WithOkResult_ExecutesOnOk()
	{
		var result = Result.Ok<int, string>(42);
		int? capturedValue = null;

		result.Match(
			value => capturedValue = value,
			_ => Assert.Fail("onErr не повинен викликатися")
		);

		Assert.Equal(42, capturedValue);
	}

	[Fact]
	public void Match_WithErrResult_ExecutesOnErr()
	{
		var result = Result.Err<int, string>("помилка");
		string? capturedError = null;

		result.Match(
			_ => Assert.Fail("onOk не повинен викликатися"),
			error => capturedError = error
		);

		Assert.Equal("помилка", capturedError);
	}
}

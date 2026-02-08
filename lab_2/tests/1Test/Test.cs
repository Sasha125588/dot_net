namespace _1Test;

using _1;

public class QuadraticResponseParserTests
{
	private static ChatGptResponse CreateResponse(string content) =>
		new()
		{
			Choices = new[] { new Choices { Message = new Message { Content = content } } }
		};

	[Fact]
	public void Parse_WhenResponseIsNull_ReturnsZeroAndEmptyRoots()
	{
		var count = QuadraticResponseParser.Parse(null, out var x1, out var x2);

		Assert.Equal(0, count);
		Assert.Equal(string.Empty, x1);
		Assert.Equal(string.Empty, x2);
	}

	[Fact]
	public void Parse_WhenContentIsNoRoots_ReturnsZeroAndEmptyRoots()
	{
		var response = CreateResponse("коренів немає");

		var count = QuadraticResponseParser.Parse(response, out var x1, out var x2);

		Assert.Equal(0, count);
		Assert.Equal(string.Empty, x1);
		Assert.Equal(string.Empty, x2);
	}

	[Theory]
	[InlineData("Коренів немає")]
	[InlineData("коренів немає")]
	[InlineData("Дискримінант < 0. Коренів немає.")]
	public void Parse_WhenContentContainsNoRootsPhrase_ReturnsZeroAndEmptyRoots(string content)
	{
		var response = CreateResponse(content);

		var count = QuadraticResponseParser.Parse(response, out var x1, out var x2);

		Assert.Equal(0, count);
		Assert.Equal(string.Empty, x1);
		Assert.Equal(string.Empty, x2);
	}

	[Fact]
	public void Parse_WhenContentIsOneRoot_ReturnsOneAndSameValueForX1AndX2()
	{
		var response = CreateResponse("-1");

		var count = QuadraticResponseParser.Parse(response, out var x1, out var x2);

		Assert.Equal(1, count);
		Assert.Equal("-1", x1);
		Assert.Equal(string.Empty, x2);
	}

	[Fact]
	public void Parse_WhenContentIsTwoRoots_ReturnsTwoAndCorrectX1X2()
	{
		var response = CreateResponse("-2\n3");

		var count = QuadraticResponseParser.Parse(response, out var x1, out var x2);

		Assert.Equal(2, count);
		Assert.Equal("-2", x1);
		Assert.Equal("3", x2);
	}

	[Fact]
	public void Parse_WhenContentHasWhitespace_TrimsRoots()
	{
		var response = CreateResponse("  -1  \n  2  ");

		var count = QuadraticResponseParser.Parse(response, out var x1, out var x2);

		Assert.Equal(2, count);
		Assert.Equal("-1", x1);
		Assert.Equal("2", x2);
	}

	[Fact]
	public void Parse_WhenContentHasEmptyLines_IgnoresEmptyLines()
	{
		var response = CreateResponse("1\n\n2");

		var count = QuadraticResponseParser.Parse(response, out var x1, out var x2);

		Assert.Equal(2, count);
		Assert.Equal("1", x1);
		Assert.Equal("2", x2);
	}

	[Fact]
	public void Parse_WhenChoicesEmpty_ReturnsZeroAndEmptyRoots()
	{
		var response = new ChatGptResponse { Choices = [] };

		var count = QuadraticResponseParser.Parse(response, out var x1, out var x2);

		Assert.Equal(0, count);
		Assert.Equal(string.Empty, x1);
		Assert.Equal(string.Empty, x2);
	}
}


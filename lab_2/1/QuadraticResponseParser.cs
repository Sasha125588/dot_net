namespace _1;

public static class QuadraticResponseParser
{
	public static int Parse(ChatGptResponse? response, out string x1, out string x2)
	{
		var text = response?.Choices.Length > 0
			? response.Choices[0].Message.Content
			: string.Empty;

		if (text.Contains("коренів немає", StringComparison.OrdinalIgnoreCase) || string.IsNullOrEmpty(text))
		{
			x1 = string.Empty;
			x2 = string.Empty;
			return 0;
		}

		var lines = text.Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

		x1 = lines.Length > 0 ? lines[0] : string.Empty;
		x2 = lines.Length > 1 ? lines[1] : string.Empty;

		return lines.Length;
	}
}

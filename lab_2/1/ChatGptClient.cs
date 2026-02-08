using System.Net.Http.Json;

namespace _1;

public sealed class ChatGptClient(HttpClient httpClient)
{
	private const string SystemPrompt =
		"Відповідай тільки коренями рівняння: лише дійсні числа, по одному на рядок. Без пояснень та іншого тексту. " +
		"Якщо дискримінант менше 0 — коренів немає: нічого не виводь або напиши «коренів немає». Не використовуй комплексні числа. " +
		"Якщо корінь один (подвійний) — виведи одне число; якщо два різних — два числа.";

	public async Task<ChatGptResponse?> AskAsync(string prompt)
	{
		var messages = new List<object>
		{
			new { role = "system", content = SystemPrompt },
			new { role = "user", content = prompt }
		};

		var response = await httpClient.PostAsJsonAsync("https://api.openai.com/v1/chat/completions", new
		{
			model = "gpt-5-nano",
			messages
		});

		return await response.Content.ReadFromJsonAsync<ChatGptResponse>();
	}
}

public sealed class ChatGptResponse
{
	public required Choices[] Choices { get; set; }
}

public sealed class Choices
{
	public required Message Message { get; set; }
}

public sealed class Message
{
	public required string Content { get; set; }
}
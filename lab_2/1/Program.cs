// dotnet run --project lab_2/1

using _1;
using DotNetEnv;

Env.TraversePath().Load();

var apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
if (string.IsNullOrEmpty(apiKey))
{
	Console.Error.WriteLine("Помилка: OPENAI_API_KEY не задано. Додайте його в .env у корені проєкту.");
	return 1;
}

var httpClient = new HttpClient();
httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

var client = new ChatGptClient(httpClient);

Console.Write("Введіть a: ");
var a = double.Parse(Console.ReadLine()!);

if (a <= 0)
{
	Console.WriteLine("Коефіцієнт a має бути додатним (більше 0).");
	return 1;
}

Console.Write("Введіть b: ");
var b = double.Parse(Console.ReadLine()!);

Console.Write("Введіть c: ");
var c = double.Parse(Console.ReadLine()!);

Console.WriteLine();


var userPrompt = $"Розв'язати квадратне рівняння {a}x^2 + {b}x + {c} = 0";

var response = await client.AskAsync(userPrompt);

var rootsCount = QuadraticResponseParser.Parse(response: response, out var x1, out var x2);

Console.WriteLine($"Кількість коренів: {rootsCount}");
Console.WriteLine();

var message = rootsCount switch
{
	0 => "Рівняння не має коренів.",
	1 => $"x = {x1}",
	2 => $"x1: {x1}, x2: {x2}",
	_ => throw new InvalidOperationException("Хибна кількість коренів"),
};

Console.WriteLine(message);

return 0;

// dotnet run --project=./lab_1/1.3

using _1._3;
using Utils;

var done = false;
while (!done)
{
	Console.Write("Введіть оцінку (0–100): ");
	GradeHelper.TryParseGrade(Console.ReadLine()).Match(
		grade =>
		{
			Console.WriteLine($"Ваша оцінка: {GradeHelper.ToFivePointScale(grade)}");
			done = true;
		},
		error => Console.WriteLine($"Помилка: {error}. Повторіть ввід."));
}

// dotnet run --project=./lab_1/4.3

using _4._3.InputReader;
using _4._3.Models;
using _4._3.Rendering;

var (startDate, endDate) = InputReader.PromptUntilValid();

var dateRange = DateRange.From(startDate, endDate);

Rendering.DisplayResult(dateRange);

Console.WriteLine();
Console.WriteLine("Натисніть будь-яку клавішу для виходу...");
Console.ReadKey();

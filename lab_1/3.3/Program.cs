// dotnet run --project=./lab_1/3.3

using _3._3.InputReader;
using _3._3.Models;
using _3._3.Rendering;

var height = InputReader.PromptUntilValid();

var dimensions = FlagDimensions.FromHeight(height);

Rendering.DrawFlag(dimensions);

Console.WriteLine("Натисніть будь-яку клавішу для виходу...");
Console.ReadKey();


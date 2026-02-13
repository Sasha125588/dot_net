// dotnet run --project=./lab_1/4.3

using _4._3.InputReader;
using _4._3.ErrorFormatters;
using _4._3.Models;
using _4._3.Rendering;
using Utils;

var (startDate, endDate) = InputReader.Prompt();

DateRange.Create(startDate, endDate).Match(
	onOk: range => Rendering.Draw(range),
	onErr: err => ValidationErrorFormatter.Format(err));

Console.WriteLine();
Console.WriteLine("Натисніть будь-яку клавішу для виходу...");
Console.ReadKey();

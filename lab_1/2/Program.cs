// dotnet run --project=./lab_1/2

using _2;
using _2.ErrorFormatters;
using _2.InputReader;
using _2.Rendering;
using Utils;

var a = InputReader.PromptPoint("A");
var b = InputReader.PromptPoint("B");
var c = InputReader.PromptPoint("C");

Triangle.Create(a, b, c)
	.MapErr(TriangleErrorFormatter.Format)
	.Match(
		Rendering.Draw,
		err => Console.WriteLine(err));
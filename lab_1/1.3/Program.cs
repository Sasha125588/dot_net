// dotnet run --project=./lab_1/1.3

using _1._3;
using _1._3.InputReader;
using _1._3.Rendering;

var grade = InputReader.PromptGrade();
var text = GradeHelper.ToFivePointScale(grade);
Rendering.DisplayGradeText(text);

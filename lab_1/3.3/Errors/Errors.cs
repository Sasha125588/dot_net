namespace _3._3.Errors;

public abstract record ValidationError
{
	public sealed record ParseError(string Input) : ValidationError;

	public sealed record TooSmall(int Value, int MinValue) : ValidationError;

	public sealed record NotDivisibleBy(int Value, int Divisor) : ValidationError;

	public string ToMessage() => this switch
	{
		ParseError e => $"Не вдалося розпізнати '{e.Input}' як число",
		TooSmall e => $"Значення {e.Value} занадто мале (мінімум: {e.MinValue})",
		NotDivisibleBy e => $"Значення {e.Value} має бути кратне {e.Divisor}",
		_ => "Невідома помилка"
	};
}
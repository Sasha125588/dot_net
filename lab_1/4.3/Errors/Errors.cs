namespace _4._3.Errors;

public abstract record ValidationError
{
	public sealed record ParseDateOnlyError(string Input) : ValidationError;

	public sealed record ValidateStartDateAfterEndDate(DateOnly FirstDate, DateOnly SecondDate) : ValidationError;

	public string ToMessage() => this switch
	{
		ParseDateOnlyError e => $"Не вдалося розпізнати '{e.Input}' як дату",
		ValidateStartDateAfterEndDate e => $"Дата {e.FirstDate} повинна бути раніше за дату {e.SecondDate}",
		_ => "Невідома помилка"
	};
}
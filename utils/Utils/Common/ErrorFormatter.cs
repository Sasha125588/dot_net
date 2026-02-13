namespace Utils.Common;

public static class CommonValidationErrorFormatter
{
	public static string Format(IValidationError err) => err switch
	{
		CommonValidationError.ParseDateOnlyError e => $"Не вдалося розпізнати '{e.Input}' як дату",
		CommonValidationError.ParseIntError e => $"Не вдалося розпізнати '{e.Input}' як ціле число",
		CommonValidationError.ParseDoubleError e => $"Не вдалося розпізнати '{e.Input}' як число з плаваючою комою",
		CommonValidationError.ParseTimeOnlyError e => $"Не вдалося розпізнати '{e.Input}' як час",
		_ => "Невідома помилка"
	};
}
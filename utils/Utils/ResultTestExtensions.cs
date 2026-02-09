using Xunit;

namespace Utils;

public static class ResultTestExtensions
{
	public static T ShouldBeOk<T, E>(this Result<T, E> result)
	{
		var ok = Assert.IsType<Ok<T, E>>(result);
		return ok.Value;
	}

	public static E ShouldBeErr<T, E>(this Result<T, E> result)
	{
		var err = Assert.IsType<Err<T, E>>(result);
		return err.Error;
	}
}
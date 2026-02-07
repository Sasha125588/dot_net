using Xunit;

namespace Utils;

public static class ResultTestExtensions
{
	public static T ShouldBeOk<T, E>(this Result<T, E> result)
	{
		var ok = Assert.IsType<Result<T, E>.Ok>(result);
		return ok.Value;
	}

	public static E ShouldBeErr<T, E>(this Result<T, E> result)
	{
		var err = Assert.IsType<Result<T, E>.Err>(result);
		return err.Error;
	}
}
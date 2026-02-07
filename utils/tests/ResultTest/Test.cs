using Utils;

namespace ResultTest;

public class ResultCreationTests
{
	[Fact]
	public void Ok_CreatesOkResult()
	{
		var result = new Result<int, string>.Ok(42);

		Assert.IsType<Result<int, string>.Ok>(result);
		Assert.Equal(42, result.Value);
	}

	[Fact]
	public void Err_CreatesErrResult()
	{
		var result = new Result<int, string>.Err("error");

		Assert.IsType<Result<int, string>.Err>(result);
		Assert.Equal("error", result.Error);
	}

	[Fact]
	public void Ok_WithNull_StoresNull()
	{
		var result = new Result<string?, int>.Ok(null);

		Assert.IsType<Result<string?, int>.Ok>(result);
		Assert.Null(result.Value);
	}

	[Fact]
	public void Err_WithNull_StoresNull()
	{
		var result = new Result<int, string?>.Err(null);

		Assert.IsType<Result<int, string?>.Err>(result);
		Assert.Null(result.Error);
	}
}

public class IsOkIsErrTests
{
	[Fact]
	public void IsOk_WithOkResult_ReturnsTrue()
	{
		var result = new Result<int, string>.Ok(42);

		Assert.True(result.IsOk());
	}

	[Fact]
	public void IsOk_WithErrResult_ReturnsFalse()
	{
		var result = new Result<int, string>.Err("error");

		Assert.False(result.IsOk());
	}

	[Fact]
	public void IsErr_WithErrResult_ReturnsTrue()
	{
		var result = new Result<int, string>.Err("error");

		Assert.True(result.IsErr());
	}

	[Fact]
	public void IsErr_WithOkResult_ReturnsFalse()
	{
		var result = new Result<int, string>.Ok(42);

		Assert.False(result.IsErr());
	}
}

public class AndThenTests
{
	[Fact]
	public void AndThen_WithOkResult_CallsBinder()
	{
		var result = new Result<int, string>.Ok(10);

		var mapped = result.AndThen(x =>
			new Result<int, string>.Ok(x * 2));

		Assert.True(mapped.IsOk());
		Assert.Equal(20, ((Result<int, string>.Ok)mapped).Value);
	}

	[Fact]
	public void AndThen_WithErrResult_DoesNotCallBinder()
	{
		var binderCalled = false;
		var result = new Result<int, string>.Err("error");

		var mapped = result.AndThen(x =>
		{
			binderCalled = true;
			return new Result<int, string>.Ok(x * 2);
		});

		Assert.False(binderCalled);
		Assert.True(mapped.IsErr());
		Assert.Equal("error", ((Result<int, string>.Err)mapped).Error);
	}

	[Fact]
	public void AndThen_CanChainMultipleTimes()
	{
		var result = new Result<int, string>.Ok(5);

		var final = result
			.AndThen(x => new Result<int, string>.Ok(x * 2))
			.AndThen(x => new Result<int, string>.Ok(x + 3))
			.AndThen(x => new Result<int, string>.Ok(x - 1));

		Assert.True(final.IsOk());
		Assert.Equal(12, ((Result<int, string>.Ok)final).Value);
	}

	[Fact]
	public void AndThen_StopsOnFirstError()
	{
		var result = new Result<int, string>.Ok(5);

		var final = result
			.AndThen(x => new Result<int, string>.Ok(x * 2))
			.AndThen(_ => new Result<int, string>.Err("failed"))
			.AndThen(x => new Result<int, string>.Ok(x + 100));

		Assert.True(final.IsErr());
		Assert.Equal("failed", ((Result<int, string>.Err)final).Error);
	}

	[Fact]
	public void AndThen_CanChangeValueType()
	{
		var result = new Result<int, string>.Ok(42);

		var mapped = result.AndThen(x =>
			new Result<string, string>.Ok($"Value: {x}"));

		Assert.True(mapped.IsOk());
		Assert.Equal("Value: 42", ((Result<string, string>.Ok)mapped).Value);
	}
}

public class MapTests
{
	[Fact]
	public void Map_WithOkResult_TransformsValue()
	{
		var result = new Result<int, string>.Ok(10);

		var mapped = result.Map(x => x * 2);

		Assert.True(mapped.IsOk());
		Assert.Equal(20, ((Result<int, string>.Ok)mapped).Value);
	}

	[Fact]
	public void Map_WithErrResult_DoesNotCallMapper()
	{
		var mapperCalled = false;
		var result = new Result<int, string>.Err("error");

		var mapped = result.Map(x =>
		{
			mapperCalled = true;
			return x * 2;
		});

		Assert.False(mapperCalled);
		Assert.True(mapped.IsErr());
		Assert.Equal("error", ((Result<int, string>.Err)mapped).Error);
	}

	[Fact]
	public void Map_CanChangeValueType()
	{
		var result = new Result<int, string>.Ok(42);

		var mapped = result.Map(x => $"Number: {x}");

		Assert.True(mapped.IsOk());
		Assert.Equal("Number: 42", ((Result<string, string>.Ok)mapped).Value);
	}

	[Fact]
	public void Map_CanChainMultipleTimes()
	{
		var result = new Result<int, string>.Ok(5);

		var final = result
			.Map(x => x * 2)
			.Map(x => x + 3)
			.Map(x => x.ToString());

		Assert.True(final.IsOk());
		Assert.Equal("13", ((Result<string, string>.Ok)final).Value);
	}
}

public class MapErrorTests
{
	[Fact]
	public void MapError_WithErrResult_TransformsError()
	{
		var result = new Result<int, string>.Err("error");

		var mapped = result.MapError(e => e.ToUpper());

		Assert.True(mapped.IsErr());
		Assert.Equal("ERROR", ((Result<int, string>.Err)mapped).Error);
	}

	[Fact]
	public void MapError_WithOkResult_DoesNotCallMapper()
	{
		var mapperCalled = false;
		var result = new Result<int, string>.Ok(42);

		var mapped = result.MapError(e =>
		{
			mapperCalled = true;
			return e.ToUpper();
		});

		Assert.False(mapperCalled);
		Assert.True(mapped.IsOk());
		Assert.Equal(42, ((Result<int, string>.Ok)mapped).Value);
	}

	[Fact]
	public void MapError_CanChangeErrorType()
	{
		var result = new Result<int, string>.Err("404");

		var mapped = result.MapError(e => int.Parse(e));

		Assert.True(mapped.IsErr());
		Assert.Equal(404, ((Result<int, int>.Err)mapped).Error);
	}
}

public class MatchTests
{
	[Fact]
	public void Match_WithOkResult_ExecutesOnOk()
	{
		var result = new Result<int, string>.Ok(42);
		int? capturedValue = null;
		string? capturedError = null;

		result.Match(
			value => capturedValue = value,
			error => capturedError = error
		);

		Assert.Equal(42, capturedValue);
		Assert.Null(capturedError);
	}

	[Fact]
	public void Match_WithErrResult_ExecutesOnErr()
	{
		var result = new Result<int, string>.Err("error");
		int? capturedValue = null;
		string? capturedError = null;

		result.Match(
			value => capturedValue = value,
			error => capturedError = error
		);

		Assert.Null(capturedValue);
		Assert.Equal("error", capturedError);
	}

	[Fact]
	public void Match_OnlyExecutesOneCallback()
	{
		var result = new Result<int, string>.Ok(42);
		var okCalled = false;
		var errCalled = false;

		result.Match(
			_ => okCalled = true,
			_ => errCalled = true
		);

		Assert.True(okCalled);
		Assert.False(errCalled);
	}
}

public class ValueOrTests
{
	[Fact]
	public void ValueOr_WithOkResult_ReturnsValue()
	{
		var result = new Result<int, string>.Ok(42);

		var value = result.ValueOr(0);

		Assert.Equal(42, value);
	}

	[Fact]
	public void ValueOr_WithErrResult_ReturnsFallback()
	{
		var result = new Result<int, string>.Err("error");

		var value = result.ValueOr(99);

		Assert.Equal(99, value);
	}

	[Fact]
	public void ValueOr_WithNullFallback_ReturnsNull()
	{
		var result = new Result<string?, int>.Err(404);

		var value = result.ValueOr(null);

		Assert.Null(value);
	}
}

public class UnwrapTests
{
	[Fact]
	public void Unwrap_WithOkResult_ReturnsValue()
	{
		var result = new Result<int, string>.Ok(42);

		var value = result.Unwrap();

		Assert.Equal(42, value);
	}

	[Fact]
	public void Unwrap_WithErrResult_ThrowsException()
	{
		var result = new Result<int, string>.Err("error");

		var ex = Assert.Throws<InvalidOperationException>(() => result.Unwrap());

		Assert.Contains("Called Unwrap on Err", ex.Message);
		Assert.Contains("error", ex.Message);
	}
}

public class UnwrapOrTests
{
	[Fact]
	public void UnwrapOr_WithOkResult_ReturnsValue()
	{
		var result = new Result<int, string>.Ok(42);

		var value = result.UnwrapOr(0);

		Assert.Equal(42, value);
	}

	[Fact]
	public void UnwrapOr_WithErrResult_ReturnsDefault()
	{
		var result = new Result<int, string>.Err("error");

		var value = result.UnwrapOr(99);

		Assert.Equal(99, value);
	}

	[Fact]
	public void UnwrapOr_BehavesLikeValueOr()
	{
		var okResult = new Result<int, string>.Ok(42);
		var errResult = new Result<int, string>.Err("error");

		Assert.Equal(okResult.ValueOr(0), okResult.UnwrapOr(0));
		Assert.Equal(errResult.ValueOr(99), errResult.UnwrapOr(99));
	}
}

public class ExpectTests
{
	[Fact]
	public void Expect_WithOkResult_ReturnsValue()
	{
		var result = new Result<int, string>.Ok(42);

		var value = result.Expect("Should have value");

		Assert.Equal(42, value);
	}

	[Fact]
	public void Expect_WithErrResult_ThrowsWithCustomMessage()
	{
		var result = new Result<int, string>.Err("error");

		var ex = Assert.Throws<InvalidOperationException>(() =>
			result.Expect("Custom error message"));

		Assert.Contains("Custom error message", ex.Message);
		Assert.Contains("error", ex.Message);
	}

	[Fact]
	public void Expect_MessageAppearsBeforeError()
	{
		var result = new Result<int, string>.Err("actual error");

		var ex = Assert.Throws<InvalidOperationException>(() =>
			result.Expect("Expected message"));

		Assert.StartsWith("Expected message:", ex.Message);
	}
}

public class ComplexChainTests
{
	[Fact]
	public void ComplexChain_AllOk_ReturnsTransformedValue()
	{
		var result = new Result<int, string>.Ok(5);

		var final = result
			.Map(x => x * 2)
			.AndThen(x => new Result<int, string>.Ok(x + 3))
			.Map(x => x.ToString())
			.AndThen(s => new Result<string, string>.Ok($"Result: {s}"));

		Assert.True(final.IsOk());
		Assert.Equal("Result: 13", ((Result<string, string>.Ok)final).Value);
	}

	[Fact]
	public void ComplexChain_WithError_StopsAtError()
	{
		var result = new Result<int, string>.Ok(5);
		var callCount = 0;

		var final = result
			.Map(x =>
			{
				callCount++;
				return x * 2;
			})
			.AndThen(_ => new Result<int, string>.Err("failed"))
			.Map(x =>
			{
				callCount++;
				return x + 100;
			})
			.AndThen(x =>
			{
				callCount++;
				return new Result<int, string>.Ok(x);
			});

		Assert.Equal(1, callCount);
		Assert.True(final.IsErr());
		Assert.Equal("failed", ((Result<int, string>.Err)final).Error);
	}

	[Fact]
	public void ComplexChain_WithMapError_TransformsError()
	{
		var result = new Result<int, string>.Ok(5);

		var final = result
			.AndThen(_ => new Result<int, string>.Err("error"))
			.MapError(e => e.ToUpper())
			.MapError(e => $"[{e}]");

		Assert.True(final.IsErr());
		Assert.Equal("[ERROR]", ((Result<int, string>.Err)final).Error);
	}
}

public class EdgeCaseTests
{
	[Fact]
	public void Result_WithComplexTypes_WorksCorrectly()
	{
		var result = new Result<List<int>, Dictionary<string, string>>.Ok([1, 2, 3]);

		Assert.True(result.IsOk());
		Assert.Equal(3, result.Value.Count);
	}

	[Fact]
	public void Result_WithRecordTypes_WorksCorrectly()
	{
		var record = new TestRecord(42, "test");
		var result = new Result<TestRecord, string>.Ok(record);

		var value = result.Unwrap();

		Assert.Equal(42, value.Id);
		Assert.Equal("test", value.Name);
	}

	[Fact]
	public void Result_WithValueTuples_WorksCorrectly()
	{
		var result = new Result<(int, string), string>.Ok((42, "answer"));

		var (id, text) = result.Unwrap();

		Assert.Equal(42, id);
		Assert.Equal("answer", text);
	}

	[Fact]
	public void Map_WithIdentityFunction_ReturnsSameValue()
	{
		var result = new Result<int, string>.Ok(42);

		var mapped = result.Map(x => x);

		Assert.Equal(42, mapped.Unwrap());
	}

	[Fact]
	public void AndThen_WithIdentityBinder_ReturnsSameValue()
	{
		var result = new Result<int, string>.Ok(42);

		var bound = result.AndThen(x => new Result<int, string>.Ok(x));

		Assert.Equal(42, bound.Unwrap());
	}
}

internal sealed record TestRecord(int Id, string Name);
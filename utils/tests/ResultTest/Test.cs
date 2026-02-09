using Utils;

namespace ResultTest;

public class ResultCreationTests
{
	[Fact]
	public void Ok_CreatesOkResult()
	{
		var result = Result.Ok<int, string>(42);

		Assert.IsType<Ok<int, string>>(result);
		Assert.Equal(42, result.Value);
	}

	[Fact]
	public void Err_CreatesErrResult()
	{
		var result = Result.Err<int, string>("error");

		Assert.IsType<Err<int, string>>(result);
		Assert.Equal("error", result.Error);
	}

	[Fact]
	public void Ok_WithNull_StoresNull()
	{
		var result = Result.Ok<string?, int>(null);

		Assert.IsType<Ok<string?, int>>(result);
		Assert.Null(result.Value);
	}

	[Fact]
	public void Err_WithNull_StoresNull()
	{
		var result = Result.Err<int, string?>(null);

		Assert.IsType<Err<int, string?>>(result);
		Assert.Null(result.Error);
	}
}

public class ResultDeconstructionTests
{
	[Fact]
	public void Ok_DeconstructsToValue()
	{
		var result = Result.Ok<string?, int>("value");

		result.Deconstruct(out var value);

		Assert.Equal("value", value);
	}

	[Fact]
	public void Err_DeconstructsToError()
	{
		var result = Result.Err<int, string?>("error");

		result.Deconstruct(out var error);

		Assert.Equal("error", error);
	}
}

public class IsOkIsErrTests
{
	[Fact]
	public void IsOk_WithOkResult_ReturnsTrue()
	{
		var result = Result.Ok<int, string>(42);

		Assert.True(result.IsOk());
	}

	[Fact]
	public void IsOk_WithErrResult_ReturnsFalse()
	{
		var result = Result.Err<int, string>("error");

		Assert.False(result.IsOk());
	}

	[Fact]
	public void IsErr_WithErrResult_ReturnsTrue()
	{
		var result = Result.Err<int, string>("error");

		Assert.True(result.IsErr());
	}

	[Fact]
	public void IsErr_WithOkResult_ReturnsFalse()
	{
		var result = Result.Ok<int, string>(42);

		Assert.False(result.IsErr());
	}
}

public class AndThenTests
{
	[Fact]
	public void AndThen_WithOkResult_CallsBinder()
	{
		var result = new Ok<int, string>(10);

		var mapped = result.AndThen(x =>
			new Ok<int, string>(x * 2));

		Assert.True(mapped.IsOk());
		Assert.Equal(20, ((Ok<int, string>)mapped).Value);
	}

	[Fact]
	public void AndThen_WithErrResult_DoesNotCallBinder()
	{
		var binderCalled = false;
		var result = Result.Err<int, string>("error");

		var mapped = result.AndThen(x =>
		{
			binderCalled = true;
			return new Ok<int, string>(x * 2);
		});

		Assert.False(binderCalled);
		Assert.True(mapped.IsErr());
		Assert.Equal("error", ((Err<int, string>)mapped).Error);
	}

	[Fact]
	public void AndThen_CanChainMultipleTimes()
	{
		var result = new Ok<int, string>(5);

		var final = result
			.AndThen(x => new Ok<int, string>(x * 2))
			.AndThen(x => new Ok<int, string>(x + 3))
			.AndThen(x => new Ok<int, string>(x - 1));

		Assert.True(final.IsOk());
		Assert.Equal(12, ((Ok<int, string>)final).Value);
	}

	[Fact]
	public void AndThen_StopsOnFirstError()
	{
		var result = new Ok<int, string>(5);

		var final = result
			.AndThen(x => new Ok<int, string>(x * 2))
			.AndThen(_ => Result.Err<int, string>("failed"))
			.AndThen(x => new Ok<int, string>(x + 100));

		Assert.True(final.IsErr());
		Assert.Equal("failed", ((Err<int, string>)final).Error);
	}

	[Fact]
	public void AndThen_CanChangeValueType()
	{
		var result = Result.Ok<int, string>(42);

		var mapped = result.AndThen(x =>
			Result.Ok<string, string>($"Value: {x}"));

		Assert.True(mapped.IsOk());
		Assert.Equal("Value: 42", ((Ok<string, string>)mapped).Value);
	}
}

public class MapTests
{
	[Fact]
	public void Map_WithOkResult_TransformsValue()
	{
		var result = new Ok<int, string>(10);

		var mapped = result.Map(x => x * 2);

		Assert.True(mapped.IsOk());
		Assert.Equal(20, ((Ok<int, string>)mapped).Value);
	}

	[Fact]
	public void Map_WithErrResult_DoesNotCallMapper()
	{
		var mapperCalled = false;
		var result = Result.Err<int, string>("error");

		var mapped = result.Map(x =>
		{
			mapperCalled = true;
			return x * 2;
		});

		Assert.False(mapperCalled);
		Assert.True(mapped.IsErr());
		Assert.Equal("error", ((Err<int, string>)mapped).Error);
	}

	[Fact]
	public void Map_CanChangeValueType()
	{
		var result = Result.Ok<int, string>(42);

		var mapped = result.Map(x => $"Number: {x}");

		Assert.True(mapped.IsOk());
		Assert.Equal("Number: 42", ((Ok<string, string>)mapped).Value);
	}

	[Fact]
	public void Map_CanChainMultipleTimes()
	{
		var result = new Ok<int, string>(5);

		var final = result
			.Map(x => x * 2)
			.Map(x => x + 3)
			.Map(x => x.ToString());

		Assert.True(final.IsOk());
		Assert.Equal("13", ((Ok<string, string>)final).Value);
	}
}

public class MapErrorTests
{
	[Fact]
	public void MapError_WithErrResult_TransformsError()
	{
		var result = Result.Err<int, string>("error");

		var mapped = result.MapErr(e => e.ToUpper());

		Assert.True(mapped.IsErr());
		Assert.Equal("ERROR", ((Err<int, string>)mapped).Error);
	}

	[Fact]
	public void MapError_WithOkResult_DoesNotCallMapper()
	{
		var mapperCalled = false;
		var result = Result.Ok<int, string>(42);

		var mapped = result.MapErr(e =>
		{
			mapperCalled = true;
			return e.ToUpper();
		});

		Assert.False(mapperCalled);
		Assert.True(mapped.IsOk());
		Assert.Equal(42, ((Ok<int, string>)mapped).Value);
	}

	[Fact]
	public void MapError_CanChangeErrorType()
	{
		var result = Result.Err<int, string>("404");

		var mapped = result.MapErr(e => int.Parse(e));

		Assert.True(mapped.IsErr());
		Assert.Equal(404, ((Err<int, int>)mapped).Error);
	}
}

public class MatchTests
{
	[Fact]
	public void Match_WithOkResult_ExecutesOnOk()
	{
		var result = Result.Ok<int, string>(42);
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
		var result = Result.Err<int, string>("error");
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
		var result = Result.Ok<int, string>(42);
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
		var result = Result.Ok<int, string>(42);

		var value = result.ValueOr(0);

		Assert.Equal(42, value);
	}

	[Fact]
	public void ValueOr_WithErrResult_ReturnsFallback()
	{
		var result = Result.Err<int, string>("error");

		var value = result.ValueOr(99);

		Assert.Equal(99, value);
	}

	[Fact]
	public void ValueOr_WithNullFallback_ReturnsNull()
	{
		var result = Result.Err<string?, int>(404);

		var value = result.ValueOr(null);

		Assert.Null(value);
	}
}

public class UnwrapTests
{
	[Fact]
	public void Unwrap_WithOkResult_ReturnsValue()
	{
		var result = Result.Ok<int, string>(42);

		var value = result.Unwrap();

		Assert.Equal(42, value);
	}

	[Fact]
	public void Unwrap_WithErrResult_ThrowsException()
	{
		var result = Result.Err<int, string>("error");

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
		var result = Result.Ok<int, string>(42);

		var value = result.UnwrapOr(0);

		Assert.Equal(42, value);
	}

	[Fact]
	public void UnwrapOr_WithErrResult_ReturnsDefault()
	{
		var result = Result.Err<int, string>("error");

		var value = result.UnwrapOr(99);

		Assert.Equal(99, value);
	}

	[Fact]
	public void UnwrapOr_BehavesLikeValueOr()
	{
		var okResult = Result.Ok<int, string>(42);
		var errResult = Result.Err<int, string>("error");

		Assert.Equal(okResult.ValueOr(0), okResult.UnwrapOr(0));
		Assert.Equal(errResult.ValueOr(99), errResult.UnwrapOr(99));
	}
}

public class ExpectTests
{
	[Fact]
	public void Expect_WithOkResult_ReturnsValue()
	{
		var result = Result.Ok<int, string>(42);

		var value = result.Expect("Should have value");

		Assert.Equal(42, value);
	}

	[Fact]
	public void Expect_WithErrResult_ThrowsWithCustomMessage()
	{
		var result = Result.Err<int, string>("error");

		var ex = Assert.Throws<InvalidOperationException>(() =>
			result.Expect("Custom error message"));

		Assert.Contains("Custom error message", ex.Message);
		Assert.Contains("error", ex.Message);
	}

	[Fact]
	public void Expect_MessageAppearsBeforeError()
	{
		var result = Result.Err<int, string>("actual error");

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
		var result = new Ok<int, string>(5);

		var final = result
			.Map(x => x * 2)
			.AndThen(x => new Ok<int, string>(x + 3))
			.Map(x => x.ToString())
			.AndThen(s => Result.Ok<string, string>($"Result: {s}"));

		Assert.True(final.IsOk());
		Assert.Equal("Result: 13", ((Ok<string, string>)final).Value);
	}

	[Fact]
	public void ComplexChain_WithError_StopsAtError()
	{
		var result = new Ok<int, string>(5);
		var callCount = 0;

		var final = result
			.Map(x =>
			{
				callCount++;
				return x * 2;
			})
			.AndThen(_ => Result.Err<int, string>("failed"))
			.Map(x =>
			{
				callCount++;
				return x + 100;
			})
			.AndThen(x =>
			{
				callCount++;
				return new Ok<int, string>(x);
			});

		Assert.Equal(1, callCount);
		Assert.True(final.IsErr());
		Assert.Equal("failed", ((Err<int, string>)final).Error);
	}

	[Fact]
	public void ComplexChain_WithMapError_TransformsError()
	{
		var result = new Ok<int, string>(5);

		var final = result
			.AndThen(_ => Result.Err<int, string>("error"))
			.MapErr(e => e.ToUpper())
			.MapErr(e => $"[{e}]");

		Assert.True(final.IsErr());
		Assert.Equal("[ERROR]", ((Err<int, string>)final).Error);
	}
}

public class EdgeCaseTests
{
	[Fact]
	public void Result_WithComplexTypes_WorksCorrectly()
	{
		var result = Result.Ok<List<int>, Dictionary<string, string>>([1, 2, 3]);

		Assert.True(result.IsOk());
		Assert.Equal(3, result.Value.Count);
	}

	[Fact]
	public void Result_WithRecordTypes_WorksCorrectly()
	{
		var record = new TestRecord(42, "test");
		var result = Result.Ok<TestRecord, string>(record);

		var value = result.Unwrap();

		Assert.Equal(42, value.Id);
		Assert.Equal("test", value.Name);
	}

	[Fact]
	public void Result_WithValueTuples_WorksCorrectly()
	{
		var result = Result.Ok<(int, string), string>((42, "answer"));

		var (id, text) = result.Unwrap();

		Assert.Equal(42, id);
		Assert.Equal("answer", text);
	}

	[Fact]
	public void Map_WithIdentityFunction_ReturnsSameValue()
	{
		var result = Result.Ok<int, string>(42);

		var mapped = result.Map(x => x);

		Assert.Equal(42, mapped.Unwrap());
	}

	[Fact]
	public void AndThen_WithIdentityBinder_ReturnsSameValue()
	{
		var result = Result.Ok<int, string>(42);

		var bound = result.AndThen(x => new Ok<int, string>(x));

		Assert.Equal(42, bound.Unwrap());
	}
}

public class AndTests
{
	[Fact]
	public void And_WithOkResult_ReturnsOther()
	{
		var result = Result.Ok<int, string>(42);
		var other = Result.Ok<string, string>("success");

		var combined = result.And(other);

		Assert.True(combined.IsOk());
		Assert.Equal("success", combined.Unwrap());
	}

	[Fact]
	public void And_WithErrResult_ReturnsError()
	{
		var result = Result.Err<int, string>("first error");
		var other = Result.Ok<string, string>("success");

		var combined = result.And(other);

		Assert.True(combined.IsErr());
		Assert.Equal("first error", ((Err<string, string>)combined).Error);
	}

	[Fact]
	public void And_WithOkAndErr_ReturnsSecondError()
	{
		var result = Result.Ok<int, string>(42);
		var other = Result.Err<string, string>("second error");

		var combined = result.And(other);

		Assert.True(combined.IsErr());
		Assert.Equal("second error", ((Err<string, string>)combined).Error);
	}

	[Fact]
	public void And_CanChangeValueType()
	{
		var result = Result.Ok<int, string>(42);
		var other = Result.Ok<string, string>("success");

		var combined = result.And(other);

		Assert.True(combined.IsOk());
		Assert.Equal("success", combined.Unwrap());
	}
}

public class OrTests
{
	[Fact]
	public void Or_WithOkResult_ReturnsFirst()
	{
		var result = Result.Ok<int, string>(42);
		var fallback = Result.Ok<int, string>(99);

		var combined = result.Or(fallback);

		Assert.True(combined.IsOk());
		Assert.Equal(42, combined.Unwrap());
	}

	[Fact]
	public void Or_WithErrResult_ReturnsFallback()
	{
		var result = Result.Err<int, string>("error");
		var fallback = Result.Ok<int, string>(99);

		var combined = result.Or(fallback);

		Assert.True(combined.IsOk());
		Assert.Equal(99, combined.Unwrap());
	}

	[Fact]
	public void Or_WithBothErr_ReturnsSecondError()
	{
		var result = Result.Err<int, string>("first error");
		var fallback = Result.Err<int, string>("second error");

		var combined = result.Or(fallback);

		Assert.True(combined.IsErr());
		Assert.Equal("second error", ((Err<int, string>)combined).Error);
	}

	[Fact]
	public void Or_WithBothOk_ReturnsFirst()
	{
		var result = Result.Ok<int, string>(42);
		var fallback = Result.Ok<int, string>(99);

		var combined = result.Or(fallback);

		Assert.Equal(42, combined.Unwrap());
	}
}

public class OrElseTests
{
	[Fact]
	public void OrElse_WithOkResult_ReturnsOriginalValue()
	{
		var result = Result.Ok<int, string>(42);
		var fallbackCalled = false;

		var combined = result.OrElse(_ =>
		{
			fallbackCalled = true;
			return Result.Ok<int, int>(99);
		});

		Assert.False(fallbackCalled);
		Assert.True(combined.IsOk());
		Assert.Equal(42, combined.Unwrap());
	}

	[Fact]
	public void OrElse_WithErrResult_CallsFallback()
	{
		var result = Result.Err<int, string>("error");

		var combined = result.OrElse(e =>
			Result.Ok<int, int>(e.Length));

		Assert.True(combined.IsOk());
		Assert.Equal(5, combined.Unwrap());
	}

	[Fact]
	public void OrElse_CanChangeErrorType()
	{
		var result = Result.Err<int, string>("404");

		var combined = result.OrElse(e =>
			Result.Err<int, int>(int.Parse(e)));

		Assert.True(combined.IsErr());
		Assert.Equal(404, ((Err<int, int>)combined).Error);
	}

	[Fact]
	public void OrElse_FallbackReceivesOriginalError()
	{
		var result = Result.Err<int, string>("original error");
		string? capturedError = null;

		result.OrElse(e =>
		{
			capturedError = e;
			return Result.Ok<int, int>(0);
		});

		Assert.Equal("original error", capturedError);
	}
}

public class MatchWithReturnTests
{
	[Fact]
	public void MatchWithReturn_WithOkResult_ReturnsOnOkValue()
	{
		var result = Result.Ok<int, string>(42);

		var output = result.Match(
			value => $"Success: {value}",
			error => $"Error: {error}"
		);

		Assert.Equal("Success: 42", output);
	}

	[Fact]
	public void MatchWithReturn_WithErrResult_ReturnsOnErrValue()
	{
		var result = Result.Err<int, string>("failure");

		var output = result.Match(
			value => $"Success: {value}",
			error => $"Error: {error}"
		);

		Assert.Equal("Error: failure", output);
	}

	[Fact]
	public void MatchWithReturn_CanReturnDifferentType()
	{
		var result = Result.Ok<int, string>(42);

		var output = result.Match(
			value => value > 0,
			_ => false
		);

		Assert.True(output);
	}

	[Fact]
	public void MatchWithReturn_OnlyExecutesOneCallback()
	{
		var result = Result.Ok<int, string>(42);
		var okCalled = false;
		var errCalled = false;

		result.Match(
			value =>
			{
				okCalled = true;
				return value;
			},
			_ =>
			{
				errCalled = true;
				return 0;
			}
		);

		Assert.True(okCalled);
		Assert.False(errCalled);
	}
}

public class MapOrTests
{
	[Fact]
	public void MapOr_WithOkResult_ReturnsTransformedValue()
	{
		var result = Result.Ok<int, string>(42);

		var output = result.MapOr("default", x => $"Value: {x}");

		Assert.Equal("Value: 42", output);
	}

	[Fact]
	public void MapOr_WithErrResult_ReturnsFallback()
	{
		var result = Result.Err<int, string>("error");

		var output = result.MapOr("default", x => $"Value: {x}");

		Assert.Equal("default", output);
	}

	[Fact]
	public void MapOr_WithErrResult_DoesNotCallMapper()
	{
		var result = Result.Err<int, string>("error");
		var mapperCalled = false;

		var output = result.MapOr(99, x =>
		{
			mapperCalled = true;
			return x * 2;
		});

		Assert.False(mapperCalled);
		Assert.Equal(99, output);
	}

	[Fact]
	public void MapOr_CanReturnDifferentType()
	{
		var result = Result.Ok<string, int>("42");

		var output = result.MapOr(0, s => int.Parse(s));

		Assert.Equal(42, output);
	}
}

public class MapOrElseTests
{
	[Fact]
	public void MapOrElse_WithOkResult_CallsMapper()
	{
		var result = Result.Ok<int, string>(42);

		var output = result.MapOrElse(
			e => e.Length,
			x => x * 2
		);

		Assert.Equal(84, output);
	}

	[Fact]
	public void MapOrElse_WithErrResult_CallsFallbackMapper()
	{
		var result = Result.Err<int, string>("error");

		var output = result.MapOrElse(
			e => e.Length,
			x => x * 2
		);

		Assert.Equal(5, output);
	}

	[Fact]
	public void MapOrElse_OnlyCallsOneMapper()
	{
		var result = Result.Ok<int, string>(42);
		var mapperCalled = false;
		var fallbackCalled = false;

		result.MapOrElse(
			_ =>
			{
				fallbackCalled = true;
				return 0;
			},
			x =>
			{
				mapperCalled = true;
				return x;
			}
		);

		Assert.True(mapperCalled);
		Assert.False(fallbackCalled);
	}

	[Fact]
	public void MapOrElse_FallbackReceivesError()
	{
		var result = Result.Err<int, string>("test error");
		string? capturedError = null;

		result.MapOrElse(
			e =>
			{
				capturedError = e;
				return 0;
			},
			x => x
		);

		Assert.Equal("test error", capturedError);
	}
}

public class UnwrapOrElseTests
{
	[Fact]
	public void UnwrapOrElse_WithOkResult_ReturnsValue()
	{
		var result = Result.Ok<int, string>(42);
		var fallbackCalled = false;

		var value = result.UnwrapOrElse(_ =>
		{
			fallbackCalled = true;
			return 0;
		});

		Assert.False(fallbackCalled);
		Assert.Equal(42, value);
	}

	[Fact]
	public void UnwrapOrElse_WithErrResult_CallsFallback()
	{
		var result = Result.Err<int, string>("error");

		var value = result.UnwrapOrElse(e => e.Length);

		Assert.Equal(5, value);
	}

	[Fact]
	public void UnwrapOrElse_FallbackReceivesError()
	{
		var result = Result.Err<int, string>("test error");
		string? capturedError = null;

		result.UnwrapOrElse(e =>
		{
			capturedError = e;
			return 0;
		});

		Assert.Equal("test error", capturedError);
	}

	[Fact]
	public void UnwrapOrElse_CanComputeFallbackFromError()
	{
		var result = Result.Err<int, string>("404");

		var value = result.UnwrapOrElse(e => int.Parse(e));

		Assert.Equal(404, value);
	}
}

public class CollectTests
{
	[Fact]
	public void Collect_WithAllOk_ReturnsListOfValues()
	{
		var results = new[]
		{
			Result.Ok<int, string>(1),
			Result.Ok<int, string>(2),
			Result.Ok<int, string>(3)
		};

		var collected = results.Collect();

		Assert.True(collected.IsOk());
		var list = collected.Unwrap();
		Assert.Equal(3, list.Count);
		Assert.Equal(1, list[0]);
		Assert.Equal(2, list[1]);
		Assert.Equal(3, list[2]);
	}

	[Fact]
	public void Collect_WithOneErr_ReturnsFirstError()
	{
		var results = new Result<int, string>[]
		{
			Result.Ok<int, string>(1),
			Result.Err<int, string>("error 1"),
			Result.Ok<int, string>(3),
			Result.Err<int, string>("error 2")
		};

		var collected = results.Collect();

		Assert.True(collected.IsErr());
		Assert.Equal("error 1", ((Err<System.Collections.Immutable.ImmutableList<int>, string>)collected).Error);
	}

	[Fact]
	public void Collect_WithEmptySequence_ReturnsEmptyList()
	{
		var results = Array.Empty<Result<int, string>>();

		var collected = results.Collect();

		Assert.True(collected.IsOk());
		var list = collected.Unwrap();
		Assert.Empty(list);
	}

	[Fact]
	public void Collect_StopsAtFirstError()
	{
		var callCount = 0;
		var results = new[]
		{
			Result.Ok<int, string>(1),
			Result.Err<int, string>("error"),
			CreateResultWithSideEffect()
		};

		Result<int, string> CreateResultWithSideEffect()
		{
			callCount++;
			return Result.Ok<int, string>(3);
		}

		var collected = results.Collect();

		Assert.Equal(1, callCount);
		Assert.True(collected.IsErr());
	}

	[Fact]
	public void Collect_ReturnsImmutableList()
	{
		var results = new[]
		{
			Result.Ok<int, string>(1),
			Result.Ok<int, string>(2)
		};

		var collected = results.Collect();

		Assert.True(collected.IsOk());
		var list = collected.Unwrap();
		Assert.IsAssignableFrom<System.Collections.Immutable.ImmutableList<int>>(list);
	}

	[Fact]
	public void Collect_WithComplexTypes_WorksCorrectly()
	{
		var results = new[]
		{
			Result.Ok<TestRecord, string>(new TestRecord(1, "first")),
			Result.Ok<TestRecord, string>(new TestRecord(2, "second"))
		};

		var collected = results.Collect();

		Assert.True(collected.IsOk());
		var list = collected.Unwrap();
		Assert.Equal(2, list.Count);
		Assert.Equal(1, list[0].Id);
		Assert.Equal("second", list[1].Name);
	}
}

public class CombinedOperationsTests
{
	[Fact]
	public void MapAndMapErr_CanTransformBothPaths()
	{
		var okResult = Result.Ok<int, string>(42);
		var errResult = Result.Err<int, string>("error");

		var okMapped = okResult
			.Map(x => x * 2)
			.MapErr(e => e.ToUpper());

		var errMapped = errResult
			.Map(x => x * 2)
			.MapErr(e => e.ToUpper());

		Assert.Equal(84, okMapped.Unwrap());
		Assert.Equal("ERROR", ((Err<int, string>)errMapped).Error);
	}

	[Fact]
	public void AndThenWithOr_ProvidesFallback()
	{
		var result = Result.Ok<int, string>(5);

		var final = result
			.AndThen(_ => Result.Err<int, string>("failed"))
			.Or(Result.Ok<int, string>(99));

		Assert.Equal(99, final.Unwrap());
	}

	[Fact]
	public void MapOrElseWithAndThen_ComplexChain()
	{
		var result = Result.Ok<int, string>(10);

		var output = result
			.AndThen(x => Result.Ok<int, string>(x * 2))
			.MapOrElse(
				_ => -1,
				x => x + 5
			);

		Assert.Equal(25, output);
	}

	[Fact]
	public void CollectWithMap_TransformsAndCollects()
	{
		var results = new[]
		{
			Result.Ok<int, string>(1),
			Result.Ok<int, string>(2),
			Result.Ok<int, string>(3)
		};

		var collected = results
			.Select(r => r.Map(x => x * 2))
			.Collect();

		Assert.True(collected.IsOk());
		var list = collected.Unwrap();
		Assert.Equal(new[] { 2, 4, 6 }, list);
	}
}

internal sealed record TestRecord(int Id, string Name);
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Sequences;
using DragonSpark.Model.Sequences.Memory;
using NetFabric.Hyperlinq;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Queries
{
	class Class4 {}

	public interface IEvaluate<in T, TResult> : ISelecting<IAsyncEnumerable<T>, TResult> {}

	sealed class ToArray<T> : IEvaluate<T, Array<T>>
	{
		public static ToArray<T> Default { get; } = new ToArray<T>();

		ToArray() {}

		public async ValueTask<Array<T>> Get(IAsyncEnumerable<T> parameter) => await parameter.ToArrayAsync();
	}

	sealed class ToLease<T> : IEvaluate<T, Lease<T>>
	{
		public static ToLease<T> Default { get; } = new ToLease<T>();

		ToLease() {}

		public async ValueTask<Lease<T>> Get(IAsyncEnumerable<T> parameter)
		{
			var owner  = await parameter.AsAsyncValueEnumerable().ToArrayAsync(ArrayPool<T>.Shared);
			var result = owner.AsLease();
			return result;
		}
	}

	sealed class ToList<T> : IEvaluate<T, List<T>>
	{
		public static ToList<T> Default { get; } = new ToList<T>();

		ToList() {}

		public ValueTask<List<T>> Get(IAsyncEnumerable<T> parameter) => parameter.ToListAsync();
	}

	sealed class ToDictionary<T, TKey> : IEvaluate<T, Dictionary<TKey, T>> where TKey : notnull
	{
		readonly Func<T, TKey> _key;

		public ToDictionary(Func<T, TKey> key) => _key = key;

		public ValueTask<Dictionary<TKey, T>> Get(IAsyncEnumerable<T> parameter) => parameter.ToDictionaryAsync(_key);
	}

	sealed class ToDictionary<T, TKey, TValue> : IEvaluate<T, Dictionary<TKey, TValue>> where TKey : notnull
	{
		readonly Func<T, TKey>   _key;
		readonly Func<T, TValue> _value;

		public ToDictionary(Func<T, TKey> key, Func<T, TValue> value)
		{
			_key        = key;
			_value = value;
		}

		public ValueTask<Dictionary<TKey, TValue>> Get(IAsyncEnumerable<T> parameter)
			=> parameter.ToDictionaryAsync(_key, _value);
	}

	sealed class Single<T> : IEvaluate<T, T>
	{
		public static Single<T> Default { get; } = new Single<T>();

		Single() {}

		public ValueTask<T> Get(IAsyncEnumerable<T> parameter) => parameter.SingleAsync(); // ISSUE: https://github.com/NetFabric/NetFabric.Hyperlinq/issues/375
	}

	sealed class SingleOrDefault<T> : IEvaluate<T, T?>
	{
		public static SingleOrDefault<T> Default { get; } = new ();

		SingleOrDefault() {}

		public ValueTask<T?> Get(IAsyncEnumerable<T> parameter) => parameter.SingleOrDefaultAsync();
	}

	sealed class First<T> : IEvaluate<T, T>
	{
		public static First<T> Default { get; } = new ();

		First() {}

		public ValueTask<T> Get(IAsyncEnumerable<T> parameter) => parameter.FirstAsync();
	}

	sealed class FirstOrDefault<T> : IEvaluate<T, T?>
	{
		public static FirstOrDefault<T> Default { get; } = new FirstOrDefault<T>();

		FirstOrDefault() {}

		public ValueTask<T?> Get(IAsyncEnumerable<T> parameter) => parameter.FirstOrDefaultAsync();
	}

	sealed class Any<T> : IEvaluate<T, bool>
	{
		public static Any<T> Default { get; } = new Any<T>();

		Any() {}

		public ValueTask<bool> Get(IAsyncEnumerable<T> parameter) => parameter.AnyAsync();
	}




	public class Evaluate<TIn, T, TResult> : ISelecting<TIn, TResult>
	{
		readonly IInvoke<TIn, T>       _invoke;
		readonly IEvaluate<T, TResult> _evaluate;

		public Evaluate(IInvoke<TIn, T> invoke, IEvaluate<T, TResult> evaluate)
		{
			_invoke   = invoke;
			_evaluate = evaluate;
		}

		public async ValueTask<TResult> Get(TIn parameter)
		{
			await using var invocation = _invoke.Get(parameter);
			var             result     = await _evaluate.Get(invocation.Elements);
			return result;
		}
	}
}
using DragonSpark.Compose;
using DragonSpark.Model;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Sequences;
using Microsoft.EntityFrameworkCore;
using NetFabric.Hyperlinq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Queries
{
	class Class4 {}

	public interface IEvaluate<in T, TResult> : ISelecting<IAsyncEnumerable<T>, TResult> {}

	sealed class ToArray<T> : IEvaluate<T, Array<T>>
	{
		public static ToArray<T> Default { get; } = new ToArray<T>();

		ToArray() {}

		public async ValueTask<Array<T>> Get(IAsyncEnumerable<T> parameter)
			=> await parameter.AsAsyncValueEnumerable().ToArrayAsync();
	}

	public class EvaluateToArray<TContext, TIn, T> : Evaluate<TIn, T, Array<T>> 
		where TContext : DbContext where T : class
	{
		public EvaluateToArray(IContexts<TContext> contexts, IQuery<TIn, T> query)
			: this(new Invoke<TContext, TIn, T>(contexts, query)) {}

		public EvaluateToArray(IInvoke<TIn, T> invoke) : base(invoke, ToArray<T>.Default) {}
	}

	public class EvaluateToArray<TContext, T> : Evaluate<T, Array<T>> where TContext : DbContext
	{
		public EvaluateToArray(IContexts<TContext> contexts, IQuery<T> query)
			: this(new Invoke<TContext, T>(contexts, query)) {}

		public EvaluateToArray(IInvoke<None, T> invoke) : base(invoke, ToArray<T>.Default) {}
	}

	public class Evaluate<T, TResult> : IResulting<TResult>
	{
		readonly IInvoke<None, T>       _invoke;
		readonly IEvaluate<T, TResult> _evaluate;

		protected Evaluate(IInvoke<None, T> invoke, IEvaluate<T, TResult> evaluate)
		{
			_invoke   = invoke;
			_evaluate = evaluate;
		}

		public async ValueTask<TResult> Get()
		{
			await using var invocation = _invoke.Get();
			var             result     = await _evaluate.Get(invocation.Elements);
			return result;
		}
	}

	public class Evaluate<TIn, T, TResult> : ISelecting<TIn, TResult>
	{
		readonly IInvoke<TIn, T>       _invoke;
		readonly IEvaluate<T, TResult> _evaluate;

		protected Evaluate(IInvoke<TIn, T> invoke, IEvaluate<T, TResult> evaluate)
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

	/*
	public class Result<T, TResult> : IResulting<TResult>
	{
		readonly ISessions                _sessions;
		readonly Await<ISession, TResult> _materialize;

		public Result(IQuery<T> query, IMaterializer<T, TResult> materializer)
			: this(Sessions.Default, query.Then().Select(materializer).Then()) {}

		public Result(ISessions sessions, Await<ISession, TResult> materialize)
		{
			_sessions    = sessions;
			_materialize = materialize;
		}

		public async ValueTask<TResult> Get()
		{
			await using var session = _sessions.Get();
			var             result  = await _materialize(session);
			return result;
		}
	}
	*/

	/*
	public class Result<TIn, TOut, TResult> : ISelecting<TIn, TResult> where TOut : class
	{
		readonly ISessions                    _sessions;
		readonly IQuery<TIn, TOut>            _query;
		readonly IMaterializer<TOut, TResult> _materializer;

		protected Result(ISessions sessions, IQuery<TIn, TOut> query, IMaterializer<TOut, TResult> materializer)
		{
			_sessions     = sessions;
			_query        = query;
			_materializer = materializer;
		}

		public async ValueTask<TResult> Get(TIn parameter)
		{
			await using var session = await _sessions.Await();
			var             query   = _query.Get(new In<TIn>(session, parameter));
			var             result  = await _materializer.Await(query);
			return result;
		}
	}
	*/
	/*public class Result<TIn, TOut, TResult> : ISelecting<TIn, TResult> where TOut : class
	{
		readonly IQuery<TIn, TOut>            _query;
		readonly IMaterializer<TOut, TResult> _materializer;

		protected Result(IQuery<TIn, TOut> query, IMaterializer<TOut, TResult> materializer)
		{
			_query        = query;
			_materializer = materializer;
		}

		public async ValueTask<TResult> Get(TIn parameter)
		{
			await using var session = _query.Get(parameter);
			var             result  = await _materializer.Await(session.Subject);
			return result;
		}
}

public class ToArrayResult<TIn, T> : Result<TIn, T, Array<T>> where T : class
{
	public ToArrayResult(IQuery<TIn, T> query) : base(query, DefaultToArray<T>.Default) {}
}

public class ToListResult<TIn, T> : Result<TIn, T, List<T>> where T : class
{
	public ToListResult(IQuery<TIn, T> query) : base(query, DefaultToList<T>.Default) {}
}

public class SingleResult<TIn, T> : Result<TIn, T, T> where T : class
{
	public SingleResult(IQuery<TIn, T> query) : base(query, SingleMaterializer<T>.Default) {}
}

public class FirstResult<TIn, T> : Result<TIn, T, T> where T : class
{
	public FirstResult(IQuery<TIn, T> query) : base(query, FirstMaterializer<T>.Default) {}
}

public class SingleOrDefaultResult<TIn, T> : Result<TIn, T, T?> where T : class
{
	public SingleOrDefaultResult(IQuery<TIn, T> query) : base(query, SingleOrDefaultMaterializer<T>.Default) {}
}

public class FirstOrDefaultResult<TIn, T> : Result<TIn, T, T?> where T : class
{
	public FirstOrDefaultResult(IQuery<TIn, T> query) : base(query, FirstOrDefaultMaterializer<T>.Default) {}
}

public class ToDictionaryResult<TIn, TKey, T> : Result<TIn, T, IReadOnlyDictionary<TKey, T>>
	where TKey : notnull
	where T : class
{
	public ToDictionaryResult(IQuery<TIn, T> query, Func<T, TKey> key)
		: this(query, new DictionaryMaterializer<T, TKey>(key)) {}

	protected ToDictionaryResult(IQuery<TIn, T> query, IMaterializer<T, IReadOnlyDictionary<TKey, T>> materializer)
		: base(query, materializer) {}
}

public class ToDictionaryResult<TIn, T, TKey, TValue> : Result<TIn, T, IReadOnlyDictionary<TKey, TValue>>
	where TKey : notnull where T : class
{
	public ToDictionaryResult(IQuery<TIn, T> query, Func<T, TKey> key, Func<T, TValue> value)
		: this(query, new DictionaryMaterializer<T, TKey, TValue>(key, value)) {}

	protected ToDictionaryResult(IQuery<TIn, T> query,
	                             IMaterializer<T, IReadOnlyDictionary<TKey, TValue>> materializer)
		: base(query, materializer) {}
}

public class AnyResult<TIn, T> : Result<TIn, T, bool> where T : class
{
	public AnyResult(IQuery<TIn, T> query) : base(query, DefaultAny<T>.Default) {}
}*/
}
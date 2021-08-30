using DragonSpark.Model.Sequences;
using DragonSpark.Model.Sequences.Memory;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace DragonSpark.Application.Entities.Queries.Evaluation
{
	class Class1 {}

	public class EvaluateToArray<TIn, TContext, T> : Evaluate<TIn, T, Array<T>> where TContext : DbContext
	{
		public EvaluateToArray(IContexts<TContext> contexts, IQuery<TIn, T> query)
			: this(new Invoke<TContext, TIn, T>(contexts, query)) {}

		public EvaluateToArray(IInvoke<TIn, T> invoke) : base(invoke, ToArray<T>.Default) {}
	}
	/*public class EvaluateToArray<TContext, T> : EvaluateToArray<TContext, None, T> where TContext : DbContext
	{
		public EvaluateToArray(IContexts<TContext> contexts, IQuery<T> query)
			: this(new Invoke<TContext, T>(contexts, query)) {}

		public EvaluateToArray(IInvoke<None, T> invoke) : base(invoke) {}
	}*/

	/*public class EvaluateToLease<TContext, T> : EvaluateToLease<TContext, None, T> where TContext : DbContext
	{
		public EvaluateToLease(IContexts<TContext> contexts, IQuery<T> query)
			: this(new Invoke<TContext, T>(contexts, query)) {}

		public EvaluateToLease(IInvoke<None, T> invoke) : base(invoke) {}
	}*/

	public class EvaluateToLease<TIn, TContext, T> : Evaluate<TIn, T, Lease<T>> where TContext : DbContext
	{
		public EvaluateToLease(IContexts<TContext> contexts, IQuery<TIn, T> query)
			: this(new Invoke<TContext, TIn, T>(contexts, query)) {}

		public EvaluateToLease(IInvoke<TIn, T> invoke) : base(invoke, ToLease<T>.Default) {}
	}

	/*public class EvaluateToList<TContext, T> : EvaluateToList<TContext, None, T> where TContext : DbContext
	{
		public EvaluateToList(IContexts<TContext> contexts, IQuery<T> query)
			: this(new Invoke<TContext, T>(contexts, query)) {}

		public EvaluateToList(IInvoke<None, T> invoke) : base(invoke) {}
	}*/

	public class EvaluateToList<TIn, TContext, T> : Evaluate<TIn, T, List<T>> where TContext : DbContext
	{
		public EvaluateToList(IContexts<TContext> contexts, IQuery<TIn, T> query)
			: this(new Invoke<TContext, TIn, T>(contexts, query)) {}

		public EvaluateToList(IInvoke<TIn, T> invoke) : base(invoke, ToList<T>.Default) {}
	}

	/*public class EvaluateToDictionary<TContext, T, TKey> : EvaluateToDictionary<TContext, None, T, TKey>
		where TKey : notnull
		where TContext : DbContext
	{
		public EvaluateToDictionary(IContexts<TContext> contexts, IQuery<T> query, Func<T, TKey> key)
			: this(new Invoke<TContext, T>(contexts, query), new ToDictionary<T, TKey>(key)) {}

		public EvaluateToDictionary(IInvoke<None, T> invoke, IEvaluate<T, Dictionary<TKey, T>> evaluate)
			: base(invoke, evaluate) {}
	}*/

	public class EvaluateToMap<TIn, TContext, T, TKey> : Evaluate<TIn, T, Dictionary<TKey, T>>
		where TKey : notnull
		where TContext : DbContext
	{
		public EvaluateToMap(IContexts<TContext> contexts, IQuery<TIn, T> query, Func<T, TKey> key)
			: this(new Invoke<TContext, TIn, T>(contexts, query), new ToDictionary<T, TKey>(key)) {}

		public EvaluateToMap(IInvoke<TIn, T> invoke, IEvaluate<T, Dictionary<TKey, T>> evaluate)
			: base(invoke, evaluate) {}
	}

	/*public class EvaluateToSelectedDictionary<TContext, T, TKey, TValue>
		: EvaluateToSelectedDictionary<TContext, None, T, TKey, TValue>
		where TKey : notnull
		where TContext : DbContext
	{
		public EvaluateToSelectedDictionary(IContexts<TContext> contexts, IQuery<T> query,
		                                    IEvaluate<T, Dictionary<TKey, TValue>> evaluate)
			: this(new Invoke<TContext, T>(contexts, query), evaluate) {}

		public EvaluateToSelectedDictionary(IInvoke<None, T> invoke, IEvaluate<T, Dictionary<TKey, TValue>> evaluate)
			: base(invoke, evaluate) {}
	}*/

	public class EvaluateToMappedSelection<TIn, TContext, T, TKey, TValue>
		: Evaluate<TIn, T, Dictionary<TKey, TValue>>
		where TKey : notnull
		where TContext : DbContext
	{
		public EvaluateToMappedSelection(IContexts<TContext> contexts, IQuery<TIn, T> query,
		                            IEvaluate<T, Dictionary<TKey, TValue>> evaluate)
			: this(new Invoke<TContext, TIn, T>(contexts, query), evaluate) {}

		public EvaluateToMappedSelection(IInvoke<TIn, T> invoke, IEvaluate<T, Dictionary<TKey, TValue>> evaluate)
			: base(invoke, evaluate) {}
	}

	public class EvaluateToSingle<TIn, TContext, T> : Evaluate<TIn, T, T> where TContext : DbContext
	{
		public EvaluateToSingle(IContexts<TContext> contexts, IQuery<TIn, T> query)
			: this(new Invoke<TContext, TIn, T>(contexts, query)) {}

		public EvaluateToSingle(IInvoke<TIn, T> invoke) : base(invoke, Single<T>.Default) {}
	}

	public class EvaluateToSingleOrDefault<TIn, TContext, T> : Evaluate<TIn, T, T?> where TContext : DbContext
	{
		public EvaluateToSingleOrDefault(IContexts<TContext> contexts, IQuery<TIn, T> query)
			: this(new Invoke<TContext, TIn, T>(contexts, query)) {}

		public EvaluateToSingleOrDefault(IInvoke<TIn, T> invoke) : base(invoke, SingleOrDefault<T>.Default) {}
	}

	public class EvaluateToFirst<TIn, TContext, T> : Evaluate<TIn, T, T> where TContext : DbContext
	{
		public EvaluateToFirst(IContexts<TContext> contexts, IQuery<TIn, T> query)
			: this(new Invoke<TContext, TIn, T>(contexts, query)) {}

		public EvaluateToFirst(IInvoke<TIn, T> invoke) : base(invoke, First<T>.Default) {}
	}

	public class EvaluateToFirstOrDefault<TIn, TContext, T> : Evaluate<TIn, T, T?> where TContext : DbContext
	{
		public EvaluateToFirstOrDefault(IContexts<TContext> contexts, IQuery<TIn, T> query)
			: this(new Invoke<TContext, TIn, T>(contexts, query)) {}

		public EvaluateToFirstOrDefault(IInvoke<TIn, T> invoke) : base(invoke, FirstOrDefault<T>.Default) {}
	}

	public class EvaluateToAny<TIn, TContext, T> : Evaluate<TIn, T, bool> where TContext : DbContext
	{
		public EvaluateToAny(IContexts<TContext> contexts, IQuery<TIn, T> query)
			: this(new Invoke<TContext, TIn, T>(contexts, query)) {}

		public EvaluateToAny(IInvoke<TIn, T> invoke) : base(invoke, Any<T>.Default) {}
	}
}
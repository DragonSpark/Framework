using DragonSpark.Model;
using DragonSpark.Model.Sequences;
using DragonSpark.Model.Sequences.Memory;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace DragonSpark.Application.Entities.Queries.Evaluation
{
	class Class1 {}

	public class EvaluateToArray<TContext, T> : EvaluateToArray<None, TContext, T> where TContext : DbContext
	{
		public EvaluateToArray(IContexts<TContext> contexts, IQuery<T> query)
			: this(new Invoke<TContext, T>(contexts, query)) {}

		public EvaluateToArray(IInvoke<None, T> invoke) : base(invoke) {}
	}

	public class EvaluateToArray<TIn, TContext, T> : Evaluate<TIn, T, Array<T>> where TContext : DbContext
	{
		public EvaluateToArray(IContexts<TContext> contexts, IQuery<TIn, T> query)
			: this(new Invoke<TContext, TIn, T>(contexts, query)) {}

		public EvaluateToArray(IInvoke<TIn, T> invoke) : base(invoke, ToArray<T>.Default) {}
	}

	public class EvaluateToLease<TContext, T> : EvaluateToLease<None, TContext, T> where TContext : DbContext
	{
		public EvaluateToLease(IContexts<TContext> contexts, IQuery<T> query)
			: this(new Invoke<TContext, T>(contexts, query)) {}

		public EvaluateToLease(IInvoke<None, T> invoke) : base(invoke) {}
	}

	public class EvaluateToLease<TIn, TContext, T> : Evaluate<TIn, T, Lease<T>> where TContext : DbContext
	{
		public EvaluateToLease(IContexts<TContext> contexts, IQuery<TIn, T> query)
			: this(new Invoke<TContext, TIn, T>(contexts, query)) {}

		public EvaluateToLease(IInvoke<TIn, T> invoke) : base(invoke, ToLease<T>.Default) {}
	}

	public class EvaluateToList<TContext, T> : EvaluateToList<None, TContext, T> where TContext : DbContext
	{
		public EvaluateToList(IContexts<TContext> contexts, IQuery<T> query)
			: this(new Invoke<TContext, T>(contexts, query)) {}

		public EvaluateToList(IInvoke<None, T> invoke) : base(invoke) {}
	}

	public class EvaluateToList<TIn, TContext, T> : Evaluate<TIn, T, List<T>> where TContext : DbContext
	{
		public EvaluateToList(IContexts<TContext> contexts, IQuery<TIn, T> query)
			: this(new Invoke<TContext, TIn, T>(contexts, query)) {}

		public EvaluateToList(IInvoke<TIn, T> invoke) : base(invoke, ToList<T>.Default) {}
	}

	public class EvaluateToMap<TContext, T, TKey> : EvaluateToMap<None, TContext, T, TKey>
		where TKey : notnull
		where TContext : DbContext
	{
		public EvaluateToMap(IContexts<TContext> contexts, IQuery<T> query, Func<T, TKey> key)
			: this(new Invoke<TContext, T>(contexts, query), new ToDictionary<T, TKey>(key)) {}

		public EvaluateToMap(IInvoke<None, T> invoke, IEvaluate<T, Dictionary<TKey, T>> evaluate)
			: base(invoke, evaluate) {}
	}

	public class EvaluateToMap<TIn, TContext, T, TKey> : Evaluate<TIn, T, Dictionary<TKey, T>>
		where TKey : notnull
		where TContext : DbContext
	{
		public EvaluateToMap(IContexts<TContext> contexts, IQuery<TIn, T> query, Func<T, TKey> key)
			: this(new Invoke<TContext, TIn, T>(contexts, query), new ToDictionary<T, TKey>(key)) {}

		public EvaluateToMap(IInvoke<TIn, T> invoke, IEvaluate<T, Dictionary<TKey, T>> evaluate)
			: base(invoke, evaluate) {}
	}

	public class EvaluateToMappedSelection<TContext, T, TKey, TValue>
		: EvaluateToMappedSelection<None, TContext, T, TKey, TValue>
		where TKey : notnull
		where TContext : DbContext
	{
		public EvaluateToMappedSelection(IContexts<TContext> contexts, IQuery<T> query,
		                                 IEvaluate<T, Dictionary<TKey, TValue>> evaluate)
			: this(new Invoke<TContext, T>(contexts, query), evaluate) {}

		public EvaluateToMappedSelection(IInvoke<None, T> invoke, IEvaluate<T, Dictionary<TKey, TValue>> evaluate)
			: base(invoke, evaluate) {}
	}

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

	public class EvaluateToSingle<TContext, T> : EvaluateToSingle<None, TContext, T> where TContext : DbContext
	{
		public EvaluateToSingle(IContexts<TContext> contexts, IQuery<T> query)
			: this(new Invoke<TContext, T>(contexts, query)) {}

		public EvaluateToSingle(IInvoke<None, T> invoke) : base(invoke) {}
	}

	public class EvaluateToSingle<TIn, TContext, T> : Evaluate<TIn, T, T> where TContext : DbContext
	{
		public EvaluateToSingle(IContexts<TContext> contexts, IQuery<TIn, T> query)
			: this(new Invoke<TContext, TIn, T>(contexts, query)) {}

		public EvaluateToSingle(IInvoke<TIn, T> invoke) : base(invoke, Single<T>.Default) {}
	}

	public class EvaluateToSingleOrDefault<TContext, T> : EvaluateToSingleOrDefault<None, TContext, T>
		where TContext : DbContext
	{
		public EvaluateToSingleOrDefault(IContexts<TContext> contexts, IQuery<T> query)
			: this(new Invoke<TContext, T>(contexts, query)) {}

		public EvaluateToSingleOrDefault(IInvoke<None, T> invoke) : base(invoke) {}
	}

	public class EvaluateToSingleOrDefault<TIn, TContext, T> : Evaluate<TIn, T, T?> where TContext : DbContext
	{
		public EvaluateToSingleOrDefault(IContexts<TContext> contexts, IQuery<TIn, T> query)
			: this(new Invoke<TContext, TIn, T>(contexts, query)) {}

		public EvaluateToSingleOrDefault(IInvoke<TIn, T> invoke) : base(invoke, SingleOrDefault<T>.Default) {}
	}

	public class EvaluateToFirst<TContext, T> : EvaluateToFirst<None, TContext, T> where TContext : DbContext
	{
		public EvaluateToFirst(IContexts<TContext> contexts, IQuery<T> query)
			: this(new Invoke<TContext, T>(contexts, query)) {}

		public EvaluateToFirst(IInvoke<None, T> invoke) : base(invoke) {}
	}

	public class EvaluateToFirst<TIn, TContext, T> : Evaluate<TIn, T, T> where TContext : DbContext
	{
		public EvaluateToFirst(IContexts<TContext> contexts, IQuery<TIn, T> query)
			: this(new Invoke<TContext, TIn, T>(contexts, query)) {}

		public EvaluateToFirst(IInvoke<TIn, T> invoke) : base(invoke, First<T>.Default) {}
	}

	public class EvaluateToFirstOrDefault<TContext, T> : EvaluateToFirstOrDefault<None, TContext, T>
		where TContext : DbContext
	{
		public EvaluateToFirstOrDefault(IContexts<TContext> contexts, IQuery<T> query)
			: this(new Invoke<TContext, T>(contexts, query)) {}

		public EvaluateToFirstOrDefault(IInvoke<None, T> invoke) : base(invoke) {}
	}

	public class EvaluateToFirstOrDefault<TIn, TContext, T> : Evaluate<TIn, T, T?> where TContext : DbContext
	{
		public EvaluateToFirstOrDefault(IContexts<TContext> contexts, IQuery<TIn, T> query)
			: this(new Invoke<TContext, TIn, T>(contexts, query)) {}

		public EvaluateToFirstOrDefault(IInvoke<TIn, T> invoke) : base(invoke, FirstOrDefault<T>.Default) {}
	}

	public class EvaluateToAny<TContext, T> : EvaluateToAny<None, TContext, T> where TContext : DbContext
	{
		public EvaluateToAny(IContexts<TContext> contexts, IQuery<T> query)
			: this(new Invoke<TContext, T>(contexts, query)) {}

		public EvaluateToAny(IInvoke<None, T> invoke) : base(invoke) {}
	}

	public class EvaluateToAny<TIn, TContext, T> : Evaluate<TIn, T, bool> where TContext : DbContext
	{
		public EvaluateToAny(IContexts<TContext> contexts, IQuery<TIn, T> query)
			: this(new Invoke<TContext, TIn, T>(contexts, query)) {}

		public EvaluateToAny(IInvoke<TIn, T> invoke) : base(invoke, Any<T>.Default) {}
	}
}
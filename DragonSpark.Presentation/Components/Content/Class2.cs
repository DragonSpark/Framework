﻿using DragonSpark.Compose;
using DragonSpark.Diagnostics;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection.Conditions;
using Microsoft.EntityFrameworkCore;
using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Policy = Polly.Policy;

namespace DragonSpark.Presentation.Components.Content
{
	class Class2 {}

	// TODO: Organize:

	public sealed class ApplicationContentPolicy : DragonSpark.Diagnostics.Policy
	{
		public static ApplicationContentPolicy Default { get; } = new ApplicationContentPolicy();

		ApplicationContentPolicy()
			: base(Policy.Handle<InvalidOperationException>(IsApplicationContentException.Default.Get)) {}
	}

	sealed class IsApplicationContentException : ICondition<InvalidOperationException>
	{
		public static IsApplicationContentException Default { get; } = new IsApplicationContentException();

		IsApplicationContentException()
			: this("A second operation was started on this context before a previous operation completed. This is usually caused by different threads concurrently using the same instance of DbContext.") {}

		readonly string _message;

		public IsApplicationContentException(string message) => _message = message;

		public bool Get(InvalidOperationException parameter) => parameter.Message.StartsWith(_message);
	}

	public sealed class DurableApplicationContentPolicy : DeferredSingleton<IAsyncPolicy>
	{
		public static DurableApplicationContentPolicy Default { get; } = new DurableApplicationContentPolicy();

		DurableApplicationContentPolicy()
			: base(ApplicationContentPolicy.Default.Then().Select(ApplicationContentRetryPolicy.Default)) {}
	}

	public sealed class ApplicationContentRetryPolicy : RetryPolicy
	{
		public static ApplicationContentRetryPolicy Default { get; } = new ApplicationContentRetryPolicy();

		ApplicationContentRetryPolicy() : base(10, new LinearRetryStrategy(TimeSpan.FromMilliseconds(50)).Get) {}
	}

	sealed class Any<T> : ISelecting<IQueryable<T>, bool>
	{
		readonly IAsyncPolicy _policy;

		public Any(IAsyncPolicy policy) => _policy = policy;

		public ValueTask<bool> Get(IQueryable<T> parameter)
			=> _policy.ExecuteAsync(parameter.Any).ToOperation();
	}

	public sealed class Count<T> : ISelecting<IQueryable<T>, int>
	{
		readonly IAsyncPolicy _policy;

		public Count(IAsyncPolicy policy) : this(policy, new LongCount<T>(policy)) {}

		public Count(IAsyncPolicy policy, ISelecting<IQueryable<T>, long> @long)
		{
			Long    = @long;
			_policy = policy;
		}

		public ISelecting<IQueryable<T>, long> Long { get; }

		public ValueTask<int> Get(IQueryable<T> parameter)
			=> _policy.ExecuteAsync(parameter.Count).ToOperation();
	}

	sealed class LongCount<T> : ISelecting<IQueryable<T>, long>
	{
		readonly IAsyncPolicy _policy;

		public LongCount(IAsyncPolicy policy) => _policy = policy;

		public ValueTask<long> Get(IQueryable<T> parameter)
			=> _policy.ExecuteAsync(parameter.LongCount).ToOperation();
	}

	public sealed class Materialize<T>
	{
		public Materialize(IAsyncPolicy policy) : this(new ToList<T>(policy), new ToArray<T>(policy)) {}

		public Materialize(ToList<T> toList, ToArray<T> toArray)
		{
			ToList  = toList;
			ToArray = toArray;
		}

		public ToList<T> ToList { get; }

		public ToArray<T> ToArray { get; }
	}

	public sealed class ToArray<T> : ISelecting<IQueryable<T>, T[]>
	{
		readonly IAsyncPolicy _policy;

		public ToArray(IAsyncPolicy policy) => _policy = policy;

		public ValueTask<T[]> Get(IQueryable<T> parameter)
			=> _policy.ExecuteAsync(parameter.ToArray).ToOperation();
	}

	public sealed class ToList<T> : ISelecting<IQueryable<T>, List<T>>
	{
		readonly IAsyncPolicy _policy;

		public ToList(IAsyncPolicy policy) => _policy = policy;

		public ValueTask<List<T>> Get(IQueryable<T> parameter)
			=> _policy.ExecuteAsync(parameter.ToList).ToOperation();
	}

	public static class ContentQueryExtensions
	{
		public static Task<int> Count<T>(this IQueryable<T> @this) => @this.CountAsync();

		public static Task<long> LongCount<T>(this IQueryable<T> @this) => @this.LongCountAsync();

		public static Task<T[]> ToArray<T>(this IQueryable<T> @this) => @this.ToArrayAsync();

		public static Task<List<T>> ToList<T>(this IQueryable<T> @this) => @this.ToListAsync();

		public static Task<bool> Any<T>(this IQueryable<T> @this) => @this.AnyAsync();
	}

	public sealed class ApplicationQuery<T>
	{
		public static ApplicationQuery<T> Default { get; } = new ApplicationQuery<T>();

		ApplicationQuery() : this(DurableApplicationContentPolicy.Default.Get()) {}

		public ApplicationQuery(IAsyncPolicy policy) : this(new Any<T>(policy), new Materialize<T>(policy),
		                                                    new Count<T>(policy)) {}

		public ApplicationQuery(ISelecting<IQueryable<T>, bool> any, Materialize<T> materialize,
		                        Count<T> count)
		{
			Any         = any;
			Materialize = materialize;
			Count       = count;
		}

		public ISelecting<IQueryable<T>, bool> Any { get; }

		public Materialize<T> Materialize { get; }
		public Count<T> Count { get; }
	}
}
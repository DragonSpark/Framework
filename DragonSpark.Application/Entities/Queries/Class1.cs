using DragonSpark.Application.Entities.Diagnostics;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using Microsoft.EntityFrameworkCore;
using Polly;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Queries
{
	class Class1 {}

	sealed class Any<T> : ISelecting<IQueryable<T>, bool>
	{
		readonly IAsyncPolicy _policy;

		public Any(IAsyncPolicy policy) => _policy = policy;

		public ValueTask<bool> Get(IQueryable<T> parameter) => _policy.ExecuteAsync(parameter.Any).ToOperation();
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
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Queries
{
	class Class1 {}

	public interface IQuerying<in T, TResult> : ISelecting<IQueryable<T>, TResult> {}

	class Querying<T, TResult> : Selecting<IQueryable<T>, TResult>, IQuerying<T, TResult>
	{
		public Querying(ISelect<IQueryable<T>, ValueTask<TResult>> select) : base(select) {}

		public Querying(Func<IQueryable<T>, ValueTask<TResult>> select) : base(select) {}
	}

	public interface IAny<in T> : IQuerying<T, bool> {}

	class Any<T> : Querying<T, bool>, IAny<T>
	{
		public Any(ISelect<IQueryable<T>, ValueTask<bool>> select) : base(select) {}

		public Any(Func<IQueryable<T>, ValueTask<bool>> select) : base(select) {}
	}

	public interface ICount<in T> : IQuerying<T, uint> {}

	class Count<T> : Querying<T, uint>, ICount<T>
	{
		public Count(ISelect<IQueryable<T>, ValueTask<uint>> select) : base(select) {}

		public Count(Func<IQueryable<T>, ValueTask<uint>> select) : base(select) {}
	}

	public interface ILargeCount<in T> : IQuerying<T, ulong> {}

	class LargeCount<T> : Querying<T, ulong>, ILargeCount<T>
	{
		public LargeCount(ISelect<IQueryable<T>, ValueTask<ulong>> select) : base(select) {}

		public LargeCount(Func<IQueryable<T>, ValueTask<ulong>> select) : base(select) {}
	}

	public interface IToArray<T> : IQuerying<T, Array<T>> {}

	class ToArray<T> : Selecting<IQueryable<T>, Array<T>>, IToArray<T>
	{
		public ToArray(ISelect<IQueryable<T>, ValueTask<Array<T>>> select) : base(select) {}

		public ToArray(Func<IQueryable<T>, ValueTask<Array<T>>> select) : base(select) {}
	}

	public interface IToList<T> : IQuerying<T, List<T>> {}

	class ToList<T> : Querying<T, List<T>>, IToList<T>
	{
		public ToList(ISelect<IQueryable<T>, ValueTask<List<T>>> select) : base(select) {}

		public ToList(Func<IQueryable<T>, ValueTask<List<T>>> select) : base(select) {}
	}

	public sealed class DefaultEntityQuery<T> : EntityQuery<T>
	{
		public static DefaultEntityQuery<T> Default { get; } = new DefaultEntityQuery<T>();

		DefaultEntityQuery() : base(DefaultAny<T>.Default, Counting<T>.Default, Materialize<T>.Default) {}
	}

	public class EntityQuery<T>
	{
		public EntityQuery(IAny<T> any, Counting<T> counting, Materialize<T> materialize)
		{
			Any         = any;
			Counting    = counting;
			Materialize = materialize;
		}

		public IAny<T> Any { get; }

		public Counting<T> Counting { get; }

		public Materialize<T> Materialize { get; }
	}

	sealed class DefaultAny<T> : IAny<T>
	{
		public static DefaultAny<T> Default { get; } = new DefaultAny<T>();

		DefaultAny() {}

		public ValueTask<bool> Get(IQueryable<T> parameter) => parameter.AnyAsync().ToOperation();
	}

	public sealed class Counting<T> : Selecting<IQueryable<T>, uint>, ICount<T>
	{
		public static Counting<T> Default { get; } = new Counting<T>();

		Counting() : this(DefaultCount<T>.Default, DefaultLargeCount<T>.Default) {}

		public Counting(ICount<T> count, ILargeCount<T> large) : base(count) => Large = large;

		public ILargeCount<T> Large { get; }
	}

	sealed class DefaultCount<T> : ICount<T>
	{
		public static DefaultCount<T> Default { get; } = new DefaultCount<T>();

		DefaultCount() {}

		public async ValueTask<uint> Get(IQueryable<T> parameter)
		{
			var count  = await parameter.CountAsync().ConfigureAwait(false);
			var result = count.Grade();
			return result;
		}
	}

	sealed class DefaultLargeCount<T> : ILargeCount<T>
	{
		public static DefaultLargeCount<T> Default { get; } = new DefaultLargeCount<T>();

		DefaultLargeCount() {}

		public async ValueTask<ulong> Get(IQueryable<T> parameter)
		{
			var count  = await parameter.LongCountAsync().ConfigureAwait(false);
			var result = count.Grade();
			return result;
		}
	}

	public sealed class Materialize<T>
	{
		public static Materialize<T> Default { get; } = new Materialize<T>();

		Materialize() : this(DefaultToList<T>.Default, DefaultToArray<T>.Default) {}

		public Materialize(IToList<T> toList, IToArray<T> toArray)
		{
			ToList  = toList;
			ToArray = toArray;
		}

		public IToList<T> ToList { get; }

		public IToArray<T> ToArray { get; }
	}

	sealed class DefaultToArray<T> : IToArray<T>
	{
		public static DefaultToArray<T> Default { get; } = new DefaultToArray<T>();

		DefaultToArray() {}

		public async ValueTask<Array<T>> Get(IQueryable<T> parameter)
			=> await parameter.ToArrayAsync().ConfigureAwait(false);
	}

	sealed class DefaultToList<T> : IToList<T>
	{
		public static DefaultToList<T> Default { get; } = new DefaultToList<T>();

		DefaultToList() {}

		public ValueTask<List<T>> Get(IQueryable<T> parameter) => parameter.ToList().ToOperation();
	}

	/*public static class ContentQueryExtensions
	{
		public static Task<int> Count<T>(this IQueryable<T> @this) => @this.CountAsync();

		public static Task<long> LongCount<T>(this IQueryable<T> @this) => @this.LongCountAsync();

		public static Task<T[]> ToArray<T>(this IQueryable<T> @this) => @this.ToArrayAsync();

		public static Task<List<T>> ToList<T>(this IQueryable<T> @this) => @this.ToListAsync();

		public static Task<bool> Any<T>(this IQueryable<T> @this) => @this.AnyAsync();
	}*/

	/*public sealed class ApplicationQuery<T>
	{
		public static ApplicationQuery<T> Default { get; } = new ApplicationQuery<T>();

		ApplicationQuery() : this(DurableApplicationContentPolicy.Default.Get()) {}

		public ApplicationQuery(IAsyncPolicy policy) : this(new DefaultAny<T>(policy), new Materialize<T>(policy),
		                                                    new Counting<T>(policy)) {}

		public ApplicationQuery(ISelecting<IQueryable<T>, bool> any, Materialize<T> materialize,
		                        Counting<T> counting)
		{
			Any         = any;
			Materialize = materialize;
			Counting    = counting;
		}

		public ISelecting<IQueryable<T>, bool> Any { get; }

		public Materialize<T> Materialize { get; }
		public Counting<T> Counting { get; }
	}*/
}
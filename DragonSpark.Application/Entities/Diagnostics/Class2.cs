using DragonSpark.Application.Entities.Queries;
using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using Polly;
using System.Linq;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Diagnostics
{
	class Class2 {}

	public sealed class DurableEntityQuery<T> : EntityQuery<T>
	{
		public static DurableEntityQuery<T> Default { get; } = new DurableEntityQuery<T>();

		DurableEntityQuery()
			: this(PolicyAwareEntityQueries<T>.Default.Get(DurableApplicationContentPolicy.Default.Get())) {}

		public DurableEntityQuery(EntityQuery<T> source) : this(source.Any, source.Counting, source.Materialize) {}

		public DurableEntityQuery(IAny<T> any, Counting<T> counting, Materialize<T> materialize) :
			base(any, counting, materialize) {}
	}

	sealed class PolicyAwareEntityQueries<T> : ISelect<IAsyncPolicy, EntityQuery<T>>
	{
		public static PolicyAwareEntityQueries<T> Default { get; } = new PolicyAwareEntityQueries<T>();

		PolicyAwareEntityQueries() : this(DefaultEntityQuery<T>.Default) {}

		readonly EntityQuery<T> _prototype;

		public PolicyAwareEntityQueries(EntityQuery<T> prototype) => _prototype = prototype;

		public EntityQuery<T> Get(IAsyncPolicy parameter)
		{
			var any = new Any<T>(_prototype.Any.With(parameter));
			var counting = new Counting<T>(new Count<T>(_prototype.Counting.With(parameter)),
			                               new LargeCount<T>(_prototype.Counting.Large));
			var materialize = new Materialize<T>(new ToList<T>(_prototype.Materialize.ToList.With(parameter)),
			                                     new ToArray<T>(_prototype.Materialize.ToArray.With(parameter)));
			var result = new EntityQuery<T>(any, counting, materialize);
			return result;
		}
	}

	public static class QueryingExtensions
	{
		public static IQuerying<T, TResult> With<T, TResult>(this IQuerying<T, TResult> @this, IAsyncPolicy policy)
			=> new PolicyAwareQuerying<T, TResult>(@this, policy);
	}

	sealed class PolicyAwareQuerying<T, TResult> : IQuerying<T, TResult>

	{
		readonly IQuerying<T, TResult> _previous;
		readonly IAsyncPolicy          _policy;

		public PolicyAwareQuerying(IQuerying<T, TResult> previous, IAsyncPolicy policy)
		{
			_previous = previous;
			_policy   = policy;
		}

		public ValueTask<TResult> Get(IQueryable<T> parameter)
			=> _policy.ExecuteAsync(_previous.Get(parameter).AsTask).ToOperation();
	}
}
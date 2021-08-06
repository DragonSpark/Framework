using DragonSpark.Application.Entities.Queries;
using DragonSpark.Application.Entities.Queries.Materialization;
using DragonSpark.Model.Selection;
using Polly;

namespace DragonSpark.Application.Entities.Diagnostics
{
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
			                               new LargeCount<T>(_prototype.Counting.Large.With(parameter)));
			var materialize = new Materializers<T>(new ToList<T>(_prototype.Materializers.ToList.With(parameter)),
			                                     new ToArray<T>(_prototype.Materializers.ToArray.With(parameter)));
			var result = new EntityQuery<T>(any, counting, materialize);
			return result;
		}
	}
}
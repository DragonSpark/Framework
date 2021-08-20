using DragonSpark.Application.Entities.Queries.Materialize;
using DragonSpark.Model.Selection;
using Polly;

namespace DragonSpark.Application.Entities.Diagnostics
{
	sealed class PolicyAwareEntityQueries<T> : ISelect<IAsyncPolicy, Materialization<T>>
	{
		public static PolicyAwareEntityQueries<T> Default { get; } = new PolicyAwareEntityQueries<T>();

		PolicyAwareEntityQueries() : this(DefaultMaterialization<T>.Default) {}

		readonly Materialization<T> _prototype;

		public PolicyAwareEntityQueries(Materialization<T> prototype) => _prototype = prototype;

		public Materialization<T> Get(IAsyncPolicy parameter)
		{
			var any = new Any<T>(_prototype.Any.With(parameter));
			var counting = new Counting<T>(new Count<T>(_prototype.Counting.With(parameter)),
			                               new LargeCount<T>(_prototype.Counting.Large.With(parameter)));
			var materialize = new Sequences<T>(new ToList<T>(_prototype.Sequences.ToList.With(parameter)),
			                                     new ToArray<T>(_prototype.Sequences.ToArray.With(parameter)));
			var result = new Materialization<T>(any, counting, materialize);
			return result;
		}
	}
}
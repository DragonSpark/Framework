using DragonSpark.Application.Entities.Queries.Materialization;

namespace DragonSpark.Application.Entities.Queries
{
	public sealed class DefaultEntityQuery<T> : EntityQuery<T>
	{
		public static DefaultEntityQuery<T> Default { get; } = new DefaultEntityQuery<T>();

		DefaultEntityQuery() : base(DefaultAny<T>.Default, Counting<T>.Default, Materializers<T>.Default) {}
	}
}
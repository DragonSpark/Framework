using DragonSpark.Model.Operations;

namespace DragonSpark.SyncfusionRendering.Queries;

sealed class BodyQuery<T> : Alterings<Parameter<T>>, IQuery<T>
{
	public static BodyQuery<T> Default { get; } = new();

	BodyQuery() : base(Search<T>.Default, Where<T>.Default, Filter<T>.Default, Sort<T>.Default) {}
}
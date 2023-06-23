using DragonSpark.Model.Operations.Selection;

namespace DragonSpark.SyncfusionRendering.Queries;

public sealed class BodyQuery<T> : Alterings<Parameter<T>>, IQuery<T>
{
	public static BodyQuery<T> Default { get; } = new();

	BodyQuery() : base(Search<T>.Default, Where<T>.Default, Sort<T>.Default, Filter<T>.Default) {}

	public BodyQuery(params IAltering<Parameter<T>>[] instances) : base(instances) {}
}
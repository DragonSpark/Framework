using DragonSpark.Model.Operations.Selection;

namespace DragonSpark.SyncfusionRendering.Queries;

public sealed class ComposeBody<T> : Alterings<Parameter<T>>, IQuery<T>
{
	public static ComposeBody<T> Default { get; } = new();

	ComposeBody() : base(Search<T>.Default, Where<T>.Default, Sort<T>.Default, Filter<T>.Default) {}

	public ComposeBody(params IAltering<Parameter<T>>[] instances) : base(instances) {}
}
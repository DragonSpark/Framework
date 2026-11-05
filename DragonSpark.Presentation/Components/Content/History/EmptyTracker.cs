using DragonSpark.Model;

namespace DragonSpark.Presentation.Components.Content.History;

public class EmptyTracker<TIn, TOut> : ITracker<TIn, TOut>
{
	public TOut? Get(TIn parameter) => default;

	public void Execute(Pair<TIn, TOut?> parameter) {}
}
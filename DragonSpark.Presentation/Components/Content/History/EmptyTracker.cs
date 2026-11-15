using DragonSpark.Model;

namespace DragonSpark.Presentation.Components.Content.History;

public class EmptyTracker<TIn, TOut> : ITracker<TIn, TOut>
{
	public void Execute(Pair<TIn, TOut?> parameter) {}

	public bool TryPop(TIn parameter, out TOut? result)
	{
		result = default;
		return false;
	}
}
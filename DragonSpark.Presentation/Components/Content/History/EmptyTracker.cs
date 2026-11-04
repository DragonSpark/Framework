using DragonSpark.Model;

namespace DragonSpark.Presentation.Components.Content.History;

public class EmptyTracker<TIn, TOut> : ITracker<TIn, TOut>
{
	readonly Func<TIn, string> _key;
	readonly byte              _size;

	public EmptyTracker(Func<TIn, string> key, byte size)
	{
		_key  = key;
		_size = size;
	}

	public TOut? Get(TIn parameter) => default;

	public void Execute(Pair<TIn, TOut?> parameter) {}
}
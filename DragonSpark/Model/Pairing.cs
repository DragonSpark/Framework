using DragonSpark.Model.Results;

namespace DragonSpark.Model;

public class Pairing<TKey, TValue> : Instance<Pair<TKey, TValue>>, IPair<TKey, TValue>
{
	public Pairing(TKey key, TValue value) : base(Pairs.Create(key, value)) {}
}
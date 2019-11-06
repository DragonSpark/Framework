using DragonSpark.Model.Results;

namespace DragonSpark.Runtime
{
	public interface IPair<TKey, TValue> : IResult<Pair<TKey, TValue>> {}
}
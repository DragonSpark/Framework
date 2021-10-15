using DragonSpark.Model.Results;

namespace DragonSpark.Model;

public interface IPair<TKey, TValue> : IResult<Pair<TKey, TValue>> {}
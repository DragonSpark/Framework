using DragonSpark.Model.Results;

namespace DragonSpark.Model.Operations.Results;

public class Resulting<T> : Result<ValueTask<T>>, IResulting<T>
{
	public Resulting(IResult<ValueTask<T>> result) : base(result) {}

	public Resulting(Func<ValueTask<T>> source) : base(source) {}
}
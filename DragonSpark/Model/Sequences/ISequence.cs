using DragonSpark.Model.Results;

namespace DragonSpark.Model.Sequences
{
	public interface ISequence<T> : IResult<Store<T>> {}
}
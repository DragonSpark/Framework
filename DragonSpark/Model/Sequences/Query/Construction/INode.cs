using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;

namespace DragonSpark.Model.Sequences.Query.Construction
{
	public interface INode<in _, T> : IResult<ISelect<_, T[]>>,
	                                  ISelect<IPartition, INode<_, T>>,
	                                  ISelect<IBodyBuilder<T>, INode<_, T>>
	{
		INode<_, TTo> Get<TTo>(IContents<T, TTo> parameter);

		ISelect<_, TTo> Get<TTo>(IReduce<T, TTo> parameter);
	}
}
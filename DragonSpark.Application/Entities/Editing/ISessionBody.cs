using DragonSpark.Model.Operations;

namespace DragonSpark.Application.Entities.Editing
{
	public interface ISessionBody<in TIn, TOut, in TSave> : ISelecting<TIn, TOut?>, IOperation<TSave> {}
}
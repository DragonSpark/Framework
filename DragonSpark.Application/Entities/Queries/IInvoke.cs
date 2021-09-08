using DragonSpark.Model.Operations;

namespace DragonSpark.Application.Entities.Queries
{
	public interface IInvoke<in TIn, T> : ISelecting<TIn, Invoke<T>> {}
}
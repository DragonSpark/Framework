using DragonSpark.Model.Operations;

namespace DragonSpark.Application.Entities.Queries.Compiled
{
	public interface IInvoke<in TIn, T> : ISelecting<TIn, Invoke<T>> {}
}
using DragonSpark.Model.Selection;

namespace DragonSpark.Application.Entities.Queries
{
	public interface IInvoke<in TIn, T> : ISelect<TIn, Invocation<T>> {}
}
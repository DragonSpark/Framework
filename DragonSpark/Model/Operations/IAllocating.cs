using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations
{
	public interface IAllocatedResult<T> : IResult<Task<T>> {}

	public interface IAllocating<T> : IAllocating<None, T> {}

	public interface IAllocating<in T, TOut> : ISelect<T, Task<TOut>> {}
}
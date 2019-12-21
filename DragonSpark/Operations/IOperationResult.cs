using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using System.Threading.Tasks;

namespace DragonSpark.Operations
{
	public interface IOperationResult<in TIn, TOut> : ISelect<TIn, ValueTask<TOut>> {}

	public interface IOperationResult<T> : IResult<ValueTask<T>> {}
}
using System.Threading.Tasks;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;

namespace DragonSpark.Operations
{
	public interface IOperation<in TIn, TOut> : ISelect<TIn, ValueTask<TOut>> {}

	public interface IOperation<T> : IResult<ValueTask<T>> {}
}
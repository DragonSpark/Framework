using DragonSpark.Model.Results;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations;

public class Accepting<T> : IOperation<T>
{
	readonly IResult<ValueTask> _previous;

	public Accepting(IResult<ValueTask> previous) => _previous = previous;

	public ValueTask Get(T parameter) => _previous.Get();
}
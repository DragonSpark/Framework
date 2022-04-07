using DragonSpark.Compose;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations;

public class Disperse<T> : IOperation<T>
{
	readonly IOperation<T> _previous;

	public Disperse(IOperation<T> previous) => _previous = previous;

	public ValueTask Get(T parameter)
	{
		Task.Run(_previous.Then().Bind(parameter).Then().Allocate());
		return ValueTask.CompletedTask;
	}
}
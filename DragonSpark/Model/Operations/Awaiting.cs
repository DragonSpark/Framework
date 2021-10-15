using DragonSpark.Compose;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations;

public class Awaiting<T> : IOperation<T>
{
	readonly Await<T> _await;

	public Awaiting(IOperation<T> operation) : this(operation.Await) {}

	public Awaiting(Await<T> await) => _await = @await;

	public async ValueTask Get(T parameter) => await _await(parameter);
}
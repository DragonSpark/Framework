using System.Threading.Tasks;

namespace DragonSpark.Model.Operations;

public class AwaitResult<T> : IResulting<T>
{
	readonly AwaitOf<T> _await;

	public AwaitResult(AwaitOf<T> await) => _await = @await;

	public async ValueTask<T> Get()
	{
		var @await = await _await();
		return @await;
	}
}
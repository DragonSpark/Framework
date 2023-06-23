using DragonSpark.Compose;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations.Results;

public class ConfiguringResult<T> : IResulting<T>
{
	readonly AwaitOf<T> _select;
	readonly Await<T>   _configure;

	public ConfiguringResult(IResulting<T> select, IOperation<T> operation) : this(select.Await, operation.Await) {}

	public ConfiguringResult(AwaitOf<T> select, Await<T> configure)
	{
		_select    = select;
		_configure = configure;
	}

	public async ValueTask<T> Get()
	{
		var result = await _select();
		await _configure(result);
		return result;
	}
}
using DragonSpark.Compose;
using DragonSpark.Model;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Transactions;

public class AppendedTransaction : ITransaction
{
	readonly ITransaction _first;
	readonly ITransaction _second;

	public AppendedTransaction(ITransaction first, ITransaction second)
	{
		_first  = first;
		_second = second;
	}

	public void Execute(None parameter)
	{
		_first.Execute(parameter);
		_second.Execute(parameter);
	}

	public async ValueTask Get()
	{
		await _first.Await();
		await _second.Await();
	}

	public async ValueTask DisposeAsync()
	{
		await _first.DisposeAsync().ConfigureAwait(false);
		await _second.DisposeAsync().ConfigureAwait(false);
	}
}
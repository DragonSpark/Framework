using DragonSpark.Compose;
using DragonSpark.Model;
using JetBrains.Annotations;
using System.Threading;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Entities.Transactions;

[MustDisposeResource]
public class AppendedTransaction(ITransaction first, ITransaction second) : ITransaction
{
	public void Execute(None parameter)
	{
		first.Execute(parameter);
		second.Execute(parameter);
	}

	public async ValueTask Get(CancellationToken parameter)
	{
		await first.Off(parameter);
		await second.Off(parameter);
	}

	public async ValueTask DisposeAsync()
	{
		await first.DisposeAsync().Off();
		await second.DisposeAsync().Off();
	}
}

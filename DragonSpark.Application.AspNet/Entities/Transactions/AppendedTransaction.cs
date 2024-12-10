using System.Threading.Tasks;
using DragonSpark.Compose;
using DragonSpark.Model;
using JetBrains.Annotations;

namespace DragonSpark.Application.AspNet.Entities.Transactions;

[MustDisposeResource]
public class AppendedTransaction(ITransaction first, ITransaction second) : ITransaction
{
	public void Execute(None parameter)
	{
		first.Execute(parameter);
		second.Execute(parameter);
	}

	public async ValueTask Get()
	{
		await first.Await();
		await second.Await();
	}

	public async ValueTask DisposeAsync()
	{
		await first.DisposeAsync().Await();
		await second.DisposeAsync().Await();
	}
}

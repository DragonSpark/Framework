using DragonSpark.Compose;
using DragonSpark.Model.Operations.Selection;
using DragonSpark.Model.Selection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Security.Identity;

sealed class AttachContext<T> : ISelecting<AttachContextInput<T>, UserManager<T>> where T : class
{
	public static AttachContext<T> Default { get; } = new();

	AttachContext() : this(GetStore<T>.Default) {}

	readonly ISelect<UserManager<T>, DbContext> _existing;

	public AttachContext(ISelect<UserManager<T>, DbContext> existing)
	{
		_existing = existing;
	}

	public async ValueTask<UserManager<T>> Get(AttachContextInput<T> parameter)
	{
		var (manager, context) = parameter;

		var transaction = context.Database.CurrentTransaction?.GetDbTransaction();
		if (transaction is not null)
		{
			var existing = _existing.Get(manager).Database;
			existing.SetDbConnection(context.Database.GetDbConnection());
			await existing.UseTransactionAsync(transaction).Await();
		}

		return manager;
	}
}
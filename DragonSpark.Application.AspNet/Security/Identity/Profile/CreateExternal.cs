using DragonSpark.Application.AspNet.Entities.Transactions;
using Microsoft.AspNetCore.Identity;

namespace DragonSpark.Application.AspNet.Security.Identity.Profile;

sealed class CreateExternal<T> : Transacting<ExternalLoginInfo, CreateUserResult<T>>, ICreateExternal<T>
	where T : IdentityUser
{
	public CreateExternal(CreateNewExternal<T> previous, ServiceScopedDatabaseTransactions database)
		: base(previous, database) {}
}
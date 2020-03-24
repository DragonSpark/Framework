using DragonSpark.Model.Operations;
using System.Security.Claims;

namespace DragonSpark.Application.Security.Identity
{
	public interface IClaimTransactions<T> : IOperation<(Synchronization<T> User, Transactions<Claim> Transactions)>
		where T : IdentityUser {}
}
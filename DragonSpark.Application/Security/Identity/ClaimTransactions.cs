using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity
{
	sealed class ClaimTransactions<T> : IClaimTransactions<T> where T : IdentityUser
	{
		readonly UserManager<T> _users;

		public ClaimTransactions(UserManager<T> users) => _users = users;

		public async ValueTask Get((Synchronization<T> User, Transactions<Claim> Transactions) parameter)
		{
			var ((_, (user, _), _), transactions) = parameter;

			if (transactions.Add.Length > 0)
			{
				await _users.AddClaimsAsync(user!, transactions.Add.Open());
			}

			foreach (var (existing, input) in transactions.Update.Open())
			{
				await _users.ReplaceClaimAsync(user!, existing, input);
			}

			if (transactions.Delete.Length > 0)
			{
				await _users.RemoveClaimsAsync(user!, transactions.Delete.Open());
			}
		}
	}
}
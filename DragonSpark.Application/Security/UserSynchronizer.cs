using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security
{
	public class UserSynchronizer<T> : IUserSynchronizer<T> where T : IdentityUser
	{
		readonly UserManager<T>        _users;
		readonly ITransactional<Claim> _transactional;
		readonly IClaims _claims;

		public UserSynchronizer(UserManager<T> users, IClaims claims)
			: this(users, ClaimsTransactional.Default, claims) {}

		public UserSynchronizer(UserManager<T> users, ITransactional<Claim> transactional, IClaims claims)
		{
			_users         = users;
			_transactional = transactional;
			_claims = claims;
		}

		public async ValueTask<bool> Get((Stored<T> Stored, ClaimsPrincipal Source) parameter)
		{
			var ((user, principal), source) = parameter;

			var transactions = _transactional.Get((_claims.Get(principal.Claims), _claims.Get(source.Claims)));

			if (transactions.Add.Length > 0)
			{
				await _users.AddClaimsAsync(user, transactions.Add.Open());
			}

			foreach (var (existing, input) in transactions.Update.Open())
			{
				await _users.ReplaceClaimAsync(user, existing, input);
			}

			if (transactions.Delete.Length > 0)
			{
				await _users.RemoveClaimsAsync(user, transactions.Delete.Open());
			}

			var result = transactions.Any();
			return result;
		}
	}
}
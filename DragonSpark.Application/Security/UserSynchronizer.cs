using DragonSpark.Compose;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security
{
	public class UserSynchronizer<T> : IUserSynchronizer<T> where T : IdentityUser
	{
		readonly UserManager<T>        _users;
		readonly ITransactional<Claim> _transactional;
		readonly Func<Claim, bool>     _where;

		public UserSynchronizer(UserManager<T> users, Func<Claim, bool> where)
			: this(users, ClaimsTransactional.Default, where) {}

		public UserSynchronizer(UserManager<T> users, ITransactional<Claim> transactional, Func<Claim, bool> where)
		{
			_users         = users;
			_transactional = transactional;
			_where         = where;
		}

		public async ValueTask<bool> Get((Stored<T> Stored, ClaimsPrincipal Source) parameter)
		{
			var ((user, principal), source) = parameter;

			var transactions = _transactional.Get((principal.Claims.Where(_where).Result(),
			                                       source.Claims.Where(_where).Result()));

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
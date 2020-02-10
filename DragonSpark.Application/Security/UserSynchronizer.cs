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
		readonly ITransactional<Claim> _transactional;
		readonly Func<Claim, bool>     _where;

		public UserSynchronizer(Func<Claim, bool> where) : this(ClaimsTransactional.Default, where) {}

		public UserSynchronizer(ITransactional<Claim> transactional, Func<Claim, bool> where)
		{
			_transactional = transactional;
			_where         = where;
		}

		public async ValueTask<bool> Get((ClaimsPrincipal Source, Destination<T> Destination) parameter)
		{
			var (source, (manager, user, principal)) = parameter;

			var transactions = _transactional.Get((source.Claims.Where(_where).Result(),
			                                       principal.Claims.Where(_where).Result()));

			if (transactions.Add.Length > 0)
			{
				await manager.AddClaimsAsync(user, transactions.Add.Open());
			}

			foreach (var (existing, input) in transactions.Update.Open())
			{
				await manager.ReplaceClaimAsync(user, existing, input);
			}

			if (transactions.Delete.Length > 0)
			{
				await manager.RemoveClaimsAsync(user, transactions.Delete.Open());
			}

			return transactions.Any();
		}
	}
}
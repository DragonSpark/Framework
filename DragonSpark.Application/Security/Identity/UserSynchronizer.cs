using System.Security.Claims;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity
{
	public class UserSynchronizer<T> : IUserSynchronizer<T> where T : IdentityUser
	{
		readonly ITransactional<Claim> _transactional;
		readonly IClaims               _claims;
		readonly IClaimTransactions<T> _transactions;

		public UserSynchronizer(IClaims claims, IClaimTransactions<T> transactions)
			: this(ClaimsTransactional.Default, claims, transactions) {}

		public UserSynchronizer(ITransactional<Claim> transactional, IClaims claims, IClaimTransactions<T> transactions)
		{
			_transactional = transactional;
			_claims        = claims;
			_transactions = transactions;
		}

		public async ValueTask<bool> Get(Synchronization<T> parameter)
		{
			var (_, (_, principal), source) = parameter;

			var transactions = _transactional.Get((_claims.Get(principal.Claims), _claims.Get(source.Claims)));

			await _transactions.Get((parameter, transactions));

			var result = transactions.Any();
			return result;
		}
	}
}
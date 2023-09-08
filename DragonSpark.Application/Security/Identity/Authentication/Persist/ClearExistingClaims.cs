using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using NetFabric.Hyperlinq;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity.Authentication.Persist;

sealed class ClearExistingClaims<T> : IOperation<PersistInput<T>> where T : IdentityUser
{
	readonly IUsers<T> _users;

	public ClearExistingClaims(IUsers<T> users) => _users = users;

	public async ValueTask Get(PersistInput<T> parameter)
	{
		var (user, array) = parameter;

		var       claims  = array.Open();
		using var users   = _users.Get();
		var       subject = await users.Subject.FindByIdAsync(user.Id.ToString());
		var       verify  = subject.Verify();
		using var types   = claims.AsValueEnumerable().Select(x => x.Type).ToArray(ArrayPool<string>.Shared, true);
		var       remove  = new List<Claim>();
		var list = await users.Subject.GetClaimsAsync(verify).ConfigureAwait(false);
		foreach (var claim in list.AsValueEnumerable())
		{
			if (types.Contains(claim.Type))
			{
				remove.Add(claim);
			}
		}

		await users.Subject.RemoveClaimsAsync(verify, remove).ConfigureAwait(false);
	}
}
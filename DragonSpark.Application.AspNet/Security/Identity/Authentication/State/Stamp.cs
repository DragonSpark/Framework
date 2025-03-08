using DragonSpark.Compose;
using DragonSpark.Model.Operations.Selection;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Security.Identity.Authentication.State;

sealed class Stamp<T> : ISelecting<T, string?> where T : IdentityUser
{
	readonly IUsers<T> _users;

	public Stamp(IUsers<T> users) => _users = users;

	public async ValueTask<string?> Get(T parameter)
	{
		using var users   = _users.Get();
		var       manager = users.Subject;
		return manager.SupportsUserSecurityStamp
			       ? await manager.GetSecurityStampAsync(parameter).Off()
			       : null;
	}
}
using DragonSpark.Model.Operations;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity.Profile
{
	public interface ILocateUser<T> : ISelecting<ExternalLoginInfo, T?> where T : IdentityUser {}

	sealed class LocateUser<T> : ILocateUser<T> where T : IdentityUser
	{
		readonly IUsers<T> _users;

		public LocateUser(IUsers<T> users) => _users = users;

		public async ValueTask<T?> Get(ExternalLoginInfo parameter)
		{
			await using var users = _users.Get();
			var result = await users.Subject.FindByLoginAsync(parameter.LoginProvider, parameter.ProviderKey)
			                        .ConfigureAwait(false);
			return result;
		}
	}
}
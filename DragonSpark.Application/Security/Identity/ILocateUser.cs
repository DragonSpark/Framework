using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity
{
	public interface ILocateUser<T> : ISelecting<ExternalLoginInfo, T> where T : IdentityUser {}

	sealed class LocateUser<T> : ILocateUser<T> where T : IdentityUser
	{
		readonly UserManager<T> _users;

		public LocateUser(UserManager<T> users) => _users = users;

		public ValueTask<T> Get(ExternalLoginInfo parameter)
			=> _users.FindByLoginAsync(parameter.LoginProvider, parameter.ProviderKey).ToOperation();
	}
}
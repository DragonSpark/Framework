using DragonSpark.Compose;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security
{
	public sealed class AddLoginOperation<T> : IIdentityOperation<T> where T : class
	{
		readonly UserManager<T> _users;

		public AddLoginOperation(UserManager<T> users) => _users = users;

		public ValueTask<IdentityResult> Get((ExternalLoginInfo Login, T User) parameter)
			=> _users.AddLoginAsync(parameter.User, parameter.Login).ToOperation();
	}
}
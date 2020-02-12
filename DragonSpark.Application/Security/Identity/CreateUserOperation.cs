using DragonSpark.Compose;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity
{
	sealed class CreateUserOperation<T> : IIdentityOperation<T> where T : class
	{
		readonly UserManager<T> _users;

		public CreateUserOperation(UserManager<T> users) => _users = users;

		public ValueTask<IdentityResult> Get((ExternalLoginInfo Login, T User) parameter)
			=> _users.CreateAsync(parameter.User).ToOperation();
	}
}
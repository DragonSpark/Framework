using DragonSpark.Compose;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity.Model.Authenticators
{
	sealed class AddLogin<T> : IAddLogin<T> where T : class
	{
		readonly UserManager<T> _users;

		public AddLogin(UserManager<T> users) => _users = users;

		public ValueTask<IdentityResult> Get(Login<T> parameter)
		{
			var (information, user) = parameter;
			return _users.AddLoginAsync(user, information).ToOperation();
		}
	}
}
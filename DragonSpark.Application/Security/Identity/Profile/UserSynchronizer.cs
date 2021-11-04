using DragonSpark.Compose;
using JetBrains.Annotations;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity.Profile;

[UsedImplicitly]
sealed class UserSynchronizer<T> : IUserSynchronizer<T> where T : IdentityUser
{
	public static UserSynchronizer<T> Default { get; } = new();

	UserSynchronizer() {}

	public ValueTask<bool> Get(Login<T> parameter) => false.ToOperation();
}
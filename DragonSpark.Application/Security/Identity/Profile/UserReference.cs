using DragonSpark.Compose;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity.Profile;

sealed class UserReference<T> : IUserReference<T> where T : IdentityUser
{
	public static UserReference<T> Default { get; } = new();

	UserReference() {}

	public ValueTask<T> Get(T parameter) => parameter.Reference().To<T>().ToOperation();
}
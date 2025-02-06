using DragonSpark.Reflection.Members;
using Microsoft.AspNetCore.Identity;

namespace DragonSpark.Application.AspNet.Security.Identity;

sealed class UserManagerStore<T> : PropertyAccessor<UserManager<T>, IUserStore<T>> where T : class
{
	public static UserManagerStore<T> Default { get; } = new();

	UserManagerStore() : base("Store") {}
}
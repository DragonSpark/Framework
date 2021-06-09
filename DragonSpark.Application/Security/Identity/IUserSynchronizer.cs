using DragonSpark.Model.Operations;

namespace DragonSpark.Application.Security.Identity
{
	public interface IUserSynchronizer<T> : IDepending<Login<T>> where T : IdentityUser {}
}
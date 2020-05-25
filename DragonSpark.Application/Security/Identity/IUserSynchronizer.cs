using DragonSpark.Model.Operations;

namespace DragonSpark.Application.Security.Identity
{
	public interface IUserSynchronizer<T> : IDepending<Synchronization<T>> where T : IdentityUser {}
}
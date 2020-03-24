using DragonSpark.Model.Operations;

namespace DragonSpark.Application.Security.Identity
{
	public interface IUserSynchronizer<T> : IOperationResult<Synchronization<T>, bool> where T : IdentityUser {}
}
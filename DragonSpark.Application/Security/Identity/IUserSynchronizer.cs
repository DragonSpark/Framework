using DragonSpark.Model.Operations;
using Microsoft.AspNetCore.Identity;

namespace DragonSpark.Application.Security.Identity
{
	public interface IUserSynchronizer<T> : IOperationResult<Synchronization<T>, bool> where T : IdentityUser {}
}
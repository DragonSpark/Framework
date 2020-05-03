using DragonSpark.Model.Operations;

namespace DragonSpark.Application.Security.Identity
{
	public interface IMarkModified<in T> : IOperationResult<T, int> where T : IdentityUser {}
}
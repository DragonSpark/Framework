using DragonSpark.Model.Operations;

namespace DragonSpark.Application.Security.Identity
{
	public interface IMarkModified<in T> : ISelecting<T, uint> where T : IdentityUser {}
}
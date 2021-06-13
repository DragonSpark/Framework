using DragonSpark.Model.Operations;

namespace DragonSpark.Application.Security.Identity
{
	public interface IRefreshAuthentication<in T> : IOperation<T> {}
}
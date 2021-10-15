using DragonSpark.Model.Operations;

namespace DragonSpark.Application.Security.Identity.Authentication;

public interface IRefreshAuthentication<in T> : IOperation<T> {}
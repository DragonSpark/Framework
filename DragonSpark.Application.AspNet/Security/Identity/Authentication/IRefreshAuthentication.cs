using DragonSpark.Model.Operations;

namespace DragonSpark.Application.AspNet.Security.Identity.Authentication;

public interface IRefreshAuthentication<in T> : IOperation<T>;
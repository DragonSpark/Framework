using DragonSpark.Model.Operations;

namespace DragonSpark.Application.AspNet.Security.Identity.Authentication.Initialization;

public interface IProcessAuthentication<T> : IOperation<AuthenticationState<T>> where T : IdentityUser;
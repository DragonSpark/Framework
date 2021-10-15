using DragonSpark.Model.Results;

namespace DragonSpark.Application.Security.Identity.Authentication;

public interface IAuthentications<T> : IResult<AuthenticationSession<T>> where T : class {}
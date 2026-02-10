namespace DragonSpark.Application.AspNet.Security.Identity.Authentication.Bearer;

public readonly record struct ValidateSignInInput<T>(AuthenticationSession<T> Session, SignInInput<T> Input)
    where T : class;
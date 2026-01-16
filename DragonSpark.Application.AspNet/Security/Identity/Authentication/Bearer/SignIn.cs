using DragonSpark.Compose;

namespace DragonSpark.Application.AspNet.Security.Identity.Authentication.Bearer;

public class SignIn<T> : SignInBase<T> where T : class
{
    protected SignIn(IAuthentications<T> signin, IComposeClaims<T> claims)
        : base(Is.Always<ValidateSignInInput<T>>().Operation().Out().AsStop().Out(), signin, claims) {}
}
using DragonSpark.Compose;

namespace DragonSpark.Application.AspNet.Security.Identity.Authentication.Passwordless;

public class SignIn<T> : SignInBase<T> where T : class
{
    protected SignIn(IAuthentications<T> signin)
        : base(Is.Always<ValidateSignInInput<T>>().Operation().Out().AsStop().Out(), signin) {}
}
namespace DragonSpark.Application.AspNet.Security.Identity.Authentication.Bearer;

public class SignInWithCode<T> : SignInBase<T>, ISignInWithCode<T> where T : class
{
    protected SignInWithCode(IAuthentications<T> signin, IComposeClaims<T> claims)
        : base(ValidateCode<T>.Default, signin, claims) {}
}
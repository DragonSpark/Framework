using DragonSpark.Application.AspNet.Navigation;

namespace DragonSpark.Application.AspNet.Security.Identity.Model;

public sealed class SignOutReturnPath : ReturnPath
{
    public SignOutReturnPath(SignOutPath path) : base(path) {}
}
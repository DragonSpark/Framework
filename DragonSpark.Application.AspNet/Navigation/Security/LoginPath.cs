using DragonSpark.Application.Navigation;

namespace DragonSpark.Application.AspNet.Navigation.Security;

public sealed class LoginPath : TemplatedPath
{
    public LoginPath(LoginPathTemplate path) : base(path) {}
}
using DragonSpark.Application.AspNet.Security.Identity.Model;

namespace DragonSpark.Application.AspNet.Navigation.Security;

public sealed class LoginPathRoot : Text.Text
{
    public LoginPathRoot(IdentityAreaPaths paths) : base(paths.LoginPath) {}
}
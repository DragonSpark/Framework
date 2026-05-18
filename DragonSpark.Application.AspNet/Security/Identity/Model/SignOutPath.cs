namespace DragonSpark.Application.AspNet.Security.Identity.Model;

public sealed class SignOutPath : Text.Text
{
    public SignOutPath(IdentityAreaPaths paths) : base(paths.LogOutPath) {}
}
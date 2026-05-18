using DragonSpark.Application.AspNet.Security.Identity.Model;

namespace DragonSpark.Application.AspNet.Navigation.Security;

public sealed class ExternalLoginPath : DragonSpark.Text.Text
{
    public ExternalLoginPath(IdentityAreaPaths paths) : base(paths.ExternalLoginPath) {}
}
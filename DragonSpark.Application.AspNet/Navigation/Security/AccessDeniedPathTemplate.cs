using DragonSpark.Application.AspNet.Security.Identity.Model;

namespace DragonSpark.Application.AspNet.Navigation.Security;

public sealed class AccessDeniedPathTemplate : Text.Text
{
    public AccessDeniedPathTemplate(IdentityAreaPaths paths) : base($"{paths.AccessDeniedPath}?ReturnUrl={{0}}") {}
}
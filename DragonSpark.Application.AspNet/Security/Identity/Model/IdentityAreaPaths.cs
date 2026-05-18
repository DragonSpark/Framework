namespace DragonSpark.Application.AspNet.Security.Identity.Model;

public sealed class IdentityAreaPaths
{
    public string LoginPath { get; set; } = "/Identity/Account/Login";
    public string ExternalLoginPath { get; set; } = "/Identity/Account/ExternalLogin";
    public string LogOutPath { get; set; } = "/Identity/Account/LogOut";
    public string AccessDeniedPath { get; set; } = "/Identity/Account/AccessDenied";
}
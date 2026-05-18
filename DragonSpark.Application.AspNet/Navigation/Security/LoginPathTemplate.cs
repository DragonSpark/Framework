namespace DragonSpark.Application.AspNet.Navigation.Security;

public sealed class LoginPathTemplate : Text.Text
{
    public LoginPathTemplate(LoginPathRoot path) : base($"{path}?ReturnUrl={{0}}") {}
}
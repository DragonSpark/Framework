namespace DragonSpark.Application.AspNet.Navigation.Security;

public sealed class DefaultExternalLogin : ExternalLogin
{
	public DefaultExternalLogin(CurrentPath @return) : base(@return) {}
}
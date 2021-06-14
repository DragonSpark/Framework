using DragonSpark.Application.Navigation;

namespace DragonSpark.Presentation.Security.Identity
{
	public sealed class DefaultExternalLogin : ExternalLogin
	{
		public DefaultExternalLogin(CurrentPath @return) : base(@return) {}
	}
}
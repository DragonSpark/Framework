namespace DragonSpark.Presentation.Security.Identity
{
	public sealed class DefaultCurrentExternalLogin : CurrentExternalLogin
	{
		public DefaultCurrentExternalLogin(DefaultExternalLogin @select, CurrentAuthenticationMethod current)
			: base(@select, current) {}
	}
}
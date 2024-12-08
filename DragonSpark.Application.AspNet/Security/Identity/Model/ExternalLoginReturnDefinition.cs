namespace DragonSpark.Application.AspNet.Security.Identity.Model;

sealed class ExternalLoginReturnDefinition : PagePathDefinition
{
	public static ExternalLoginReturnDefinition Default { get; } = new();

	ExternalLoginReturnDefinition() : base("./ExternalLogin", "Callback", ReturnUrlValue.Default) {}
}
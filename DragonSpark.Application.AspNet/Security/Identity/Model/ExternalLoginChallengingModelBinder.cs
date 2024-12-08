namespace DragonSpark.Application.AspNet.Security.Identity.Model;

sealed class ExternalLoginChallengingModelBinder : ChallengingModelBinder
{
	public ExternalLoginChallengingModelBinder(ExternalLoginReturnLocation @return) : base(@return) {}
}
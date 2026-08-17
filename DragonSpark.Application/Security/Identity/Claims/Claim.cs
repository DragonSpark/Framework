namespace DragonSpark.Application.Security.Identity.Claims;

public class Claim : Text.Text
{
	protected Claim(string name) : base($"{ClaimNamespace.Default}:{name}") {}
}
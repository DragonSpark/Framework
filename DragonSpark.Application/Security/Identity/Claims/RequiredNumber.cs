namespace DragonSpark.Application.Security.Identity.Claims;

public class RequiredNumber : RequiredClaim<uint>
{
	protected RequiredNumber(string claim) : base(claim, uint.Parse) {}
}
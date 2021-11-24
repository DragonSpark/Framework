using DragonSpark.Application.Security.Identity.Claims.Actions;

namespace DragonSpark.Identity.Mixcloud.Claims;

public class IsProfessionalAccountAction : ClaimAction
{
	public static IsProfessionalAccountAction Default { get; } = new();

	IsProfessionalAccountAction() : base(IsProfessionalAccount.Default, "is_pro", "boolean") {}
}
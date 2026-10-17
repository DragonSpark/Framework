using DragonSpark.Model.Results;
using System.Security.Claims;

namespace DragonSpark.Entra;

sealed class MarkerClaim : Instance<Claim>
{
	public static MarkerClaim Default { get; } = new();

	MarkerClaim() : base(new("EasyAuthTransformed", "true")) {}
}
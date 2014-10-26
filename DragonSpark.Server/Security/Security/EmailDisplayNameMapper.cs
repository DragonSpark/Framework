using System.Security.Claims;
using DragonSpark.Extensions;
using ClaimTypes = System.IdentityModel.Claims.ClaimTypes;

namespace DragonSpark.Security
{
	public class EmailDisplayNameMapper : ClaimsMapperBase<UserProfile>
	{
		protected override void PerformMapping( Claim claim, UserProfile userProfile )
		{
			userProfile.DisplayName.Null( () =>
			{
				userProfile.DisplayName = claim.Value;
			});
		}

		protected override string TargetClaimType
		{
			get { return ClaimTypes.Email; }
		}
	}
}
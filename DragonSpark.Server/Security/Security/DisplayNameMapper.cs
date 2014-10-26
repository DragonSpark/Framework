using DragonSpark.Extensions;
using System.Security.Claims;
using ClaimTypes = System.IdentityModel.Claims.ClaimTypes;

namespace DragonSpark.Security
{
	public class DisplayNameMapper : ClaimsMapperBase<UserProfile>
    {
        protected override void PerformMapping( Claim claim, UserProfile userProfile )
        {
			userProfile.DisplayName.Null( () =>
			{
				userProfile.DisplayName = claim.Value;

				// Check for first name/lastname:
				var parts = claim.Value.ToStringArray( ' ' );
				switch ( parts.Length )
				{
					case 2:
						userProfile.FirstName = parts[ 0 ];
						userProfile.LastName = parts[ 1 ];
						break;
				}
			});
        }

        protected override string TargetClaimType
        {
            get { return ClaimTypes.Name; }
        }
    }
}
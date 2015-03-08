using DragonSpark.Extensions;
using System.Security.Claims;
using ClaimTypes = System.IdentityModel.Claims.ClaimTypes;

namespace DragonSpark.Application.Communication.Security
{
    public class DisplayNameMapper : ClaimsMapperBase<ApplicationUser>
    {
        protected override void PerformMapping( Claim claim, ApplicationUser user )
        {
            user.DisplayName = claim.Value;

            // Check for first name/lastname:
            var parts = claim.Value.ToStringArray( ' ' );
            switch ( parts.Length )
            {
                case 2:
                    user.FirstName = parts[ 0 ];
                    user.LastName = parts[ 1 ];
                    break;
            }
        }

        protected override string TargetClaimType
        {
            get { return ClaimTypes.Name; }
        }
    }
}
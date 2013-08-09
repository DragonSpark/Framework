using System.Security.Claims;
using ClaimTypes = System.IdentityModel.Claims.ClaimTypes;

namespace DragonSpark.Application.Communication.Security
{
    public class EmailMapper : ClaimsMapperBase<Entity.Notifications.ApplicationUser>
    {
        protected override void PerformMapping( Claim claim, Entity.Notifications.ApplicationUser user )
        {
            user.EmailAddress = claim.Value;
        }

        protected override string TargetClaimType
        {
            get { return ClaimTypes.Email; }
        }
    }
}
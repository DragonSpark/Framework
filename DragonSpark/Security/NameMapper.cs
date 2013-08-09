using System.Diagnostics;
using System.Security.Claims;
using ClaimTypes = System.IdentityModel.Claims.ClaimTypes;

namespace DragonSpark.Security
{
    public class NameMapper : ClaimsMapperBase<ApplicationUser>
    {
        protected override void PerformMapping( Claim claim, ApplicationUser applicationUser )
        {
            // user.Name = user.AsTo<IIdentity,string>( x => x.DetermineUniqueName() );
	        Debugger.Break();
        }

        protected override string TargetClaimType
        {
            get { return ClaimTypes.NameIdentifier; }
        }
    }
}
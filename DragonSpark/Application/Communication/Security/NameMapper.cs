using DragonSpark.Extensions;
using System.Security.Claims;
using System.Security.Principal;
using System.ServiceModel.DomainServices.Server.ApplicationServices;
using ClaimTypes = System.IdentityModel.Claims.ClaimTypes;

namespace DragonSpark.Application.Communication.Security
{
    public class NameMapper : ClaimsMapperBase<IUser>
    {
        protected override void PerformMapping( Claim claim, IUser user )
        {
            user.Name = user.AsTo<IIdentity,string>( x => x.DetermineUniqueName() );
        }

        protected override string TargetClaimType
        {
            get { return ClaimTypes.NameIdentifier; }
        }
    }
}
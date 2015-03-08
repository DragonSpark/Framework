using DragonSpark.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.ServiceModel.DomainServices.Server.ApplicationServices;

namespace DragonSpark.Application.Communication.Security
{
    public class ClaimsProcessor : IClaimsProcessor
    {
        readonly string identityProviderName;
        readonly IEnumerable<IClaimsMapper> mappers;

        public ClaimsProcessor( string identityProviderName, IEnumerable<IClaimsMapper> mappers )
        {
            this.identityProviderName = identityProviderName;
            this.mappers = mappers;
        }

        public string IdentityProviderName
        {
            get { return identityProviderName; }
        }

        public void Process( ClaimsIdentity identity, IUser user )
        {
            identity.Claims.Apply( x => mappers.Where( y => y.Matches( x, user ) ).Apply( y => y.Map( x, user ) ) );
        }
    }
}
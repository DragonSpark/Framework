using System.Security.Claims;
using System.ServiceModel.DomainServices.Server.ApplicationServices;

namespace DragonSpark.Application.Communication.Security
{
    public interface IClaimsMapper
    {
        bool Matches( Claim claim, IUser user );
        void Map( Claim claim, IUser user );
    }
}
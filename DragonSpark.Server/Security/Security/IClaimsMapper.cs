using System.Security.Claims;

namespace DragonSpark.Server.Legacy.Security.Security
{
    public interface IClaimsMapper
    {
        bool Matches( Claim claim, UserProfile user );
        void Map( Claim claim, UserProfile user );
    }
}
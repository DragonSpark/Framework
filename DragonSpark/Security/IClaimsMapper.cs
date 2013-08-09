using System.Security.Claims;

namespace DragonSpark.Security
{
    public interface IClaimsMapper
    {
        bool Matches( Claim claim, UserProfile user );
        void Map( Claim claim, UserProfile user );
    }
}
using System.Security.Claims;

namespace DragonSpark.Server.Legacy.Security.Security
{
    public interface IClaimsProcessor
    {
        string IdentityProviderName { get; }
        void Process( ClaimsIdentity identity, UserProfile user );
    }
}
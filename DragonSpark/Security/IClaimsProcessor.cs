using System.Security.Claims;

namespace DragonSpark.Security
{
    public interface IClaimsProcessor
    {
        string IdentityProviderName { get; }
        void Process( ClaimsIdentity identity, UserProfile user );
    }
}
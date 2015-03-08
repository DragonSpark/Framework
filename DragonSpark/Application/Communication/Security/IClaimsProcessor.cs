using System.Security.Claims;
using System.ServiceModel.DomainServices.Server.ApplicationServices;

namespace DragonSpark.Application.Communication.Security
{
    public interface IClaimsProcessor
    {
        string IdentityProviderName { get; }
        void Process( ClaimsIdentity identity, IUser user );
    }
}
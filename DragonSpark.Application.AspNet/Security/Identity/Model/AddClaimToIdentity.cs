using DragonSpark.Application.AspNet.Security.Identity.Authentication;

namespace DragonSpark.Application.AspNet.Security.Identity.Model;

public class AddClaimToIdentity<T> : AddClaimToCurrent<T> where T : IdentityUser
{
    protected AddClaimToIdentity(ICurrent<T> current, IAuthentications<T> sessions, string claim) 
        : base(current, new AddClaim<T>(sessions, claim)) {}
}
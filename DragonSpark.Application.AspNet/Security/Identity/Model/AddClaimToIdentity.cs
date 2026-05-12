namespace DragonSpark.Application.AspNet.Security.Identity.Model;

public class AddClaimToIdentity<T> : AddClaimToCurrent<T> where T : IdentityUser
{
    protected AddClaimToIdentity(ICurrent<T> current, IUsers<T> users, string claim) 
        : base(current, new AddClaim<T>(users, claim)) {}
}
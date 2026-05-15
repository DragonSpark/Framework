namespace DragonSpark.Application.AspNet.Security.Identity.Claims.Policy;

public class UserNamesPolicy : AddPolicyConfiguration
{
    protected UserNamesPolicy(string name, params string[] names) : base(name, new RequireUserNames(names)) {}
}
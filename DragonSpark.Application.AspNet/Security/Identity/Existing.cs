using DragonSpark.Application.AspNet.Entities;
using DragonSpark.Application.AspNet.Entities.Editing;

namespace DragonSpark.Application.AspNet.Security.Identity;

public class Existing<T> : EditExisting<T> where T : IdentityUser
{
	protected Existing(IEnlistedScopes scopes) : base(scopes) {}
}
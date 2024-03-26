using DragonSpark.Application.Entities;
using DragonSpark.Application.Entities.Editing;

namespace DragonSpark.Application.Security.Identity;

public class Existing<T> : EditExisting<T> where T : IdentityUser
{
	protected Existing(IEnlistedContexts contexts) : base(contexts) {}
}
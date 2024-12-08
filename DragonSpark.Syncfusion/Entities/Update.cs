using DragonSpark.Application.AspNet.Entities;

namespace DragonSpark.SyncfusionRendering.Entities;

public sealed class Update<T> : Application.AspNet.Entities.Editing.Update<T> where T : class
{
	public Update(IEnlistedScopes scopes) : base(scopes) {}
}
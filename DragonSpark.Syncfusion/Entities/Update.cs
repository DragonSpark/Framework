using DragonSpark.Application.Entities;

namespace DragonSpark.SyncfusionRendering.Entities;

public sealed class Update<T> : DragonSpark.Application.Entities.Editing.Update<T> where T : class
{
	public Update(IEnlistedScopes scopes) : base(scopes) {}
}
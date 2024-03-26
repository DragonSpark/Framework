using DragonSpark.Compose;

namespace DragonSpark.Application.Entities.Editing;

public sealed class SaveAndCommit<T> : Update<T> where T : class
{
	public SaveAndCommit(IEnlistedScopes scopes)
		: base(new CommitAwareEdits<T, T>(scopes, A.Self<T>().Then().Operation().Out())) {}
}
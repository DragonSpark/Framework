using DragonSpark.Compose;

namespace DragonSpark.Application.Entities.Editing;

public sealed class SaveAndCommit<T> : Update<T> where T : class
{
	public SaveAndCommit(IEnlistedContexts contexts)
		: base(new CommitAwareEdits<T, T>(contexts, A.Self<T>().Then().Operation().Out())) {}
}
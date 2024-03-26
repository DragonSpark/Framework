using DragonSpark.Compose;
using System;

namespace DragonSpark.Application.Entities.Editing;

public class SaveMany<T> : Modify<Memory<T>> where T : class
{
	public SaveMany(IEnlistedContexts contexts) : base(contexts, UpdateMany<T>.Default.Then().Operation()) {}
}
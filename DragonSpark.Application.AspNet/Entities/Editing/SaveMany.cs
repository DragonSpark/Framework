using DragonSpark.Compose;
using System;

namespace DragonSpark.Application.AspNet.Entities.Editing;

public class SaveMany<T> : Modify<Memory<T>> where T : class
{
	public SaveMany(IEnlistedScopes scopes) : base(scopes, UpdateMany<T>.Default.Then().Operation()) {}
}
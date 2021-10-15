using DragonSpark.Compose;
using System;

namespace DragonSpark.Application.Entities.Editing;

public class SaveMany : Modify<Memory<object>>
{
	public SaveMany(IEnlistedScopes scopes) : base(scopes, UpdateMany.Default.Then().Operation().Out()) {}
}

public class SaveMany<T> : Modify<Memory<T>> where T : class
{
	public SaveMany(IEnlistedScopes scopes) : base(scopes, UpdateMany<T>.Default.Then().Operation()) {}
}

sealed class UpdateMany<T> : IModify<Memory<T>> where T : class
{
	public static UpdateMany<T> Default { get; } = new UpdateMany<T>();

	UpdateMany() {}

	public void Execute(Edit<Memory<T>> parameter)
	{
		var (context, memory) = parameter;
		for (var i = 0; i < memory.Length; i++)
		{
			context.Update(memory.Span[i]);
		}
	}
}
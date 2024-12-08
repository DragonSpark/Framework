using System;

namespace DragonSpark.Application.AspNet.Entities.Editing;

sealed class UpdateMany<T> : IModify<Memory<T>> where T : class
{
	public static UpdateMany<T> Default { get; } = new();

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
using System;

namespace DragonSpark.Application.Entities.Editing;

sealed class UpdateMany : IModify<Memory<object>>
{
	public static UpdateMany Default { get; } = new();

	UpdateMany() {}

	public void Execute(Edit<Memory<object>> parameter)
	{
		var (context, memory) = parameter;
		for (var i = 0; i < memory.Length; i++)
		{
			context.Update(memory.Span[i]);
		}
	}
}
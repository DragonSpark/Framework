using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Stop;
using DragonSpark.Model.Results;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

sealed class Step<T> : IStopAware<uint>
{
	readonly StepInput<T>         _input;
	readonly IMutable<DbContext?> _logical;

	public Step(StepInput<T> input) : this(input, LogicalContext.Default) {}

	public Step(StepInput<T> input, IMutable<DbContext?> logical)
	{
		_input   = input;
		_logical = logical;
	}

	public async ValueTask Get(Stop<uint> parameter)
	{
		var (offset, stop)                                                  = parameter;
		var (logger, workspaces, entities, size, total, processor, results) = _input;

		await using var workspace = workspaces.Get();
		using var       _         = _logical.Assigned(workspace.Destination);
		var             query     = entities(workspace.Source).Skip(offset.Degrade()).Take(size);
		var             page      = await processor.Off(new(new(logger, workspace, query, offset, size, total), stop));

		results.Push(page);
	}
}
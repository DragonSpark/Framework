using DragonSpark.Application.AspNet.Entities.Migration.Migrators.Processors;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Stop;
using DragonSpark.Model.Results;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

sealed class Page<T> : IStopAware<uint>
{
	readonly IEntityProcessor<T>  _processor;
	readonly PageInput<T>         _input;
	readonly IMutable<DbContext?> _logical;

	public Page(IEntityProcessor<T> processor, PageInput<T> input) : this(processor, input, LogicalContext.Default) {}

	public Page(IEntityProcessor<T> processor, PageInput<T> input, IMutable<DbContext?> logical)
	{
		_processor = processor;
		_input     = input;
		_logical   = logical;
	}

	public async ValueTask Get(Stop<uint> parameter)
	{
		var (offset, stop)                                       = parameter;
		var (logger, workspaces, entities, size, total, results) = _input;

		await using var workspace = workspaces.Get();
		using var       _         = _logical.Assigned(workspace.Destination);
		var             query     = entities(workspace.Source);
		var             page      = await _processor.Off(new(new(logger, workspace, query, offset, size, total), stop));

		results.Push(page);
	}
}
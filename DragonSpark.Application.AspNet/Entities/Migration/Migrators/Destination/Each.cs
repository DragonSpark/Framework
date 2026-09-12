using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Stop;
using DragonSpark.Model.Results;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators.Destination;

sealed class Each<TFrom, TTo> : IStopAware<TFrom> where TFrom : class where TTo : class
{
	readonly WriterInput<TFrom>   _input;
	readonly IElement<TFrom, TTo> _element;
	readonly IMutable<DbContext?> _logical;

	public Each(WriterInput<TFrom> input, IElement<TFrom, TTo> element) : this(input, element, LogicalContext.Default) {}

	public Each(WriterInput<TFrom> input, IElement<TFrom, TTo> element, IMutable<DbContext?> logical)
	{
		_input   = input;
		_element = element;
		_logical = logical;
	}

	public async ValueTask Get(Stop<TFrom> parameter)
	{
		var (subject, stop)                  = parameter;
		var (entities, page, writer) = _input;
		var workspace = entities.Get();
		var (source, destination) = workspace;
		using var _ = _logical.Assigned(destination);
		try
		{
			await _element.Off(new(new(entities, workspace, page, entities.Origin.Entry(subject)), stop));
			await writer.WriteAsync(destination, stop).Off();
			await source.DisposeAsync().Off();
		}
		catch
		{
			await workspace.DisposeAsync().Off();
			throw;
		}
	}
}
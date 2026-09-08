using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Stop;
using DragonSpark.Model.Results;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators.Destination;

sealed class Each<TFrom, TTo> : IStopAware<TFrom> where TFrom : class where TTo : class
{
	readonly EachInput<TFrom>     _paged;
	readonly IElement<TFrom, TTo> _element;
	readonly IMutable<DbContext?> _logical;

	public Each(EachInput<TFrom> paged, IElement<TFrom, TTo> element) : this(paged, element, LogicalContext.Default) {}

	public Each(EachInput<TFrom> paged, IElement<TFrom, TTo> element, IMutable<DbContext?> logical)
	{
		_paged   = paged;
		_element = element;
		_logical = logical;
	}

	public async ValueTask Get(Stop<TFrom> parameter)
	{
		var (subject, stop)                    = parameter;
		var (origin, workspaces, page, writer) = _paged;
		var workspace = workspaces.Get();
		var (source, destination) = workspace;
		using var _ = _logical.Assigned(destination);
		try
		{
			await _element.Off(new(new(workspaces, workspace, page, origin.Entry(subject)), stop));
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
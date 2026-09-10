using DragonSpark.Application.AspNet.Entities.Migration.Migrators.Destination;
using DragonSpark.Application.AspNet.Entities.Migration.Migrators.Processors;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

public class EntityMigratorBase<TFrom, TTo> : Instance<EntityTypeMapping>, IEntityMigrator
	where TFrom : class where TTo : class
{
	readonly Contexts<TFrom>         _contexts;
	readonly IEntityProcessor<TFrom> _processor;

	protected EntityMigratorBase(DbContext source, IModel destination)
		: this(new(source, destination), Map.Default) {}

	protected EntityMigratorBase(DbContext source, IModel destination,
	                             Func<Stop<MapInput<TFrom, TTo>>, ValueTask> map)
		: this(new(source, destination), new Map<TFrom, TTo>(map)) {}

	protected EntityMigratorBase(DbContext source, IModel destination, Action<MapInput<TFrom, TTo>> map)
		: this(new(source, destination), new Map<TFrom, TTo>(map)) {}

	protected EntityMigratorBase(DbContext source, IModel destination, Action<TFrom, TTo> map)
		: this(new(source, destination), new Map<TFrom, TTo>(map)) {}

	protected EntityMigratorBase(Contexts<TFrom> contexts, IMap map)
		: this(contexts, Processors<TFrom, TTo>.Default.Get(new(contexts, map))) {}

	protected EntityMigratorBase(Contexts<TFrom> contexts, IEntityProcessor<TFrom> processor)
		: base(new(typeof(TFrom), typeof(TTo)))
	{
		_contexts  = contexts;
		_processor = processor;
	}

	public ValueTask Get(Stop<EntityPreMigrationInput> parameter) => ValueTask.CompletedTask;

	public ValueTask Get(Stop<EntityPostMigrationInput> parameter) => ValueTask.CompletedTask;

	public async ValueTask Get(Stop<EntityMigratorInput> parameter)
	{
		var ((logger, workspaces, size), stop) = parameter;
		var (source, _, _, subject)            = _contexts;
		try
		{
			var total     = await subject.CountAsync().Off();
			var decorated = new OriginAwareWorkspaces<TFrom>(workspaces, source);
			await _processor.Off(new(new(logger, size, source, decorated, subject, total.Grade()), stop));
		}
		catch (Exception e)
		{
			logger.LogError(e, "A problem was encountered while processing the entities {From} -> {To}", typeof(TFrom),
			                typeof(TTo));
			throw;
		}
	}
}

sealed class OriginAwareWorkspaces<T> : IWorkspaces where T : class
{
	readonly IWorkspaces _previous;
	readonly DbContext   _origin;

	public OriginAwareWorkspaces(IWorkspaces previous, DbContext origin)
	{
		_previous = previous;
		_origin   = origin;
	}

	public Workspace Get()
	{
		var result = _previous.Get();
		foreach (var entry in _origin.ChangeTracker.Entries<T>())
		{
			result.Source.Applied(entry);
		}

		return result;
	}

	public DatabaseFacade Database => _previous.Database;

	public IModel Model => _previous.Model;
}
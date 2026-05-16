using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection.Stop;
using DragonSpark.Model.Selection.Conditions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Entities.Migration.Identity;

sealed class ComposeEntityMap<TFrom, TTo> : IStopAware<IReadOnlyCollection<TFrom>, IConditional<object, TTo>>
	where TFrom : class where TTo : class
{
	readonly Func<object, object> _source;
	readonly DbSet<TTo>           _destination;
	readonly Func<TTo, object>    _existing;

	public ComposeEntityMap(DbContext source, DbContext destination) : this(source, destination, Keys.Default.Get) {}

	public ComposeEntityMap(DbContext source, DbContext destination, Func<EntityEntry, object> keys)
		: this(Start.A.Selection<object, EntityEntry>(source.Entry).Select(keys).Get, destination.Set<TTo>(), keys) {}

	public ComposeEntityMap(Func<object, object> source, DbSet<TTo> destination, Func<EntityEntry, object> keys)
		: this(source, destination, Start.A.Selection<TTo, EntityEntry>(destination.Entry).Select(keys).Get) {}

	public ComposeEntityMap(Func<object, object> source, DbSet<TTo> destination, Func<TTo, object> existing)
	{
		_source      = source;
		_destination = destination;
		_existing    = existing;
	}

	public async ValueTask<IConditional<object, TTo>> Get(Stop<IReadOnlyCollection<TFrom>> parameter)
	{
		var (subject, stop) = parameter;
		var keys     = subject.Select(_source).ToArray();
		var where    = ComposeWhere<TTo>.Default.Get(new(_destination.EntityType, keys));
		var existing = await _destination.Where(where).ToArrayAsync(stop).Off();
		var result   = existing.ToDictionary(_existing, StructuralEqualityComparer.Default).ToTable();
		return result;
	}
}
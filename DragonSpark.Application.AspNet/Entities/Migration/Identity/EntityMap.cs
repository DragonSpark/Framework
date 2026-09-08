using DragonSpark.Application.AspNet.Entities.Migration.Migrators;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Stores;
using DragonSpark.Model.Sequences;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Collections.Concurrent;
using System.Linq.Expressions;

namespace DragonSpark.Application.AspNet.Entities.Migration.Identity;

sealed class EntityMap<TFrom, TTo> : IEntityMap<TFrom, TTo> where TFrom : class where TTo : class
{
	readonly IResult<Workspace>        _workspaces;
	readonly Func<EntityEntry, object> _keys;
	readonly IEqualityComparer<object> _comparer;

	public EntityMap(IResult<Workspace> workspaces)
		: this(workspaces, Keys.Default.Get, StructuralEqualityComparer.Default) {}

	public EntityMap(IResult<Workspace> workspaces, Func<EntityEntry, object> keys, IEqualityComparer<object> comparer)
	{
		_workspaces    = workspaces;
		_keys          = keys;
		_comparer = comparer;
	}

	public async ValueTask<IPopAware<object, Migrators.Instances.Entry<TTo>>> Get(
		Stop<IReadOnlyCollection<TFrom>> parameter)
	{
		var (subject, stop) = parameter;
		await using var workspace = _workspaces.Get();
		var (source, destination) = workspace;
		var to = destination.Set<TTo>();
		var where =
			new ModelWhere<TTo>(subject.Select(Start.A.Selection<object, EntityEntry>(source.Entry).Select(_keys).Get)
			                           .Result());
		var existing = await to.Where(where.Get(to.EntityType)).ToArrayAsync(stop).Off();
		var dictionary = existing.ToDictionary(Start.A.Selection<TTo, EntityEntry<TTo>>(to.Entry).Select(_keys).Get,
		                                       x => new Migrators.Instances.Entry<TTo>(x, to.Entry(x).CurrentValues));
		var store = new ConcurrentDictionary<object, Migrators.Instances.Entry<TTo>>(dictionary, _comparer);
		return new ConcurrentTable<object, Migrators.Instances.Entry<TTo>>(store);
	}
}

// TODO
public interface IWhere<T> : ISelect<IEntityType, Expression<Func<T, bool>>>;

sealed class ModelWhere<T> : ReferenceValueStore<IEntityType, Expression<Func<T, bool>>>, IWhere<T>
{
	public ModelWhere(Array<object> keys) : base(new ComposeModelWhere<T>(keys)) {}
}

sealed class ComposeModelWhere<T> : ISelect<IEntityType, Expression<Func<T, bool>>>
{
	readonly Array<object>                                            _keys;
	readonly ISelect<ComposeContainsInput, Expression<Func<T, bool>>> _where;

	public ComposeModelWhere(Array<object> keys) : this(keys, ComposeWhere<T>.Default) {}

	public ComposeModelWhere(Array<object> keys, ISelect<ComposeContainsInput, Expression<Func<T, bool>>> where)
	{
		_keys  = keys;
		_where = where;
	}

	public Expression<Func<T, bool>> Get(IEntityType parameter) => _where.Get(new(parameter, _keys));
}
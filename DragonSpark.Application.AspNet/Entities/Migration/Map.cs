using DragonSpark.Application.AspNet.Entities.Migration.Identity;
using DragonSpark.Application.AspNet.Entities.Migration.Migrators;
using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Selection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NetFabric.Hyperlinq;
using System;
using System.Buffers;
using System.Linq;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Entities.Migration;

public sealed class Map : IMap
{
	public static Map Default { get; } = new();

	Map() : this(CopyValues.Default, MapOwned.Default, EntityEntryState.Default) {}

	readonly ICommand<MapInput>                _copy;
	readonly ICommand<MapNavigationEntryInput> _owned;
	readonly ISelect<EntityEntry, EntityState> _state;

	public Map(ICommand<MapInput> copy, ICommand<MapNavigationEntryInput> owned,
	           ISelect<EntityEntry, EntityState> state)
	{
		_copy  = copy;
		_owned = owned;
		_state = state;
	}

	public ValueTask Get(Stop<MapInput> parameter)
	{
		var ((from, to), _) = parameter;
		/*
		var stop = to.Entity.GetType().Name == "MarketplaceProfile";
		if (stop)
		{
			Debugger.Break();
			// TODO V2 : DATA
		}*/
		_copy.Execute(parameter);
		to.Context.Attach(to.Entity);

		using var navigations = to.Context.Entry(to.Entity)
		                          .Navigations.AsValueEnumerable()
		                          .ToArray(ArrayPool<NavigationEntry>.Shared);
		foreach (var navigation in from.Metadata.GetNavigations().Where(x => x.TargetEntityType.IsOwned()))
		{
			try
			{
				var entry = navigations.FirstOrDefault(x => x.Metadata.Name == navigation.Name);

				if (entry is not null)
				{
					_owned.Execute(new(from.Context.Entry(from.Entity).Navigation(navigation.Name), entry));
				}
			}
			catch (Exception e)
			{
				throw;
			}
		}
		
		to.State = _state.Get(to);
		return ValueTask.CompletedTask;
	}
}

public sealed class Map<TFrom, TTo> : IMap
{
	readonly Func<Stop<MapInput<TFrom, TTo>>, ValueTask> _map;
	readonly IMap                                        _previous;

	public Map(Action<TFrom, TTo> map)
		: this(x =>
		       {
			       map(x.Subject.From.Entity, x.Subject.To.Entity);
			       return ValueTask.CompletedTask;
		       }) {}

	public Map(Action<MapInput<TFrom, TTo>> input)
		: this(x =>
		       {
			       input(x.Subject);
			       return ValueTask.CompletedTask;
		       }) {}

	public Map(Func<Stop<MapInput<TFrom, TTo>>, ValueTask> map) : this(map, Map.Default) {}

	public Map(Func<Stop<MapInput<TFrom, TTo>>, ValueTask> map, IMap previous)
	{
		_map      = map;
		_previous = previous;
	}

	public async ValueTask Get(Stop<MapInput> parameter)
	{
		await _previous.Off(parameter);
		var ((from, to), stop) = parameter;
		await _map(new(new(new(from), new(to)), stop)).Off();
	}
}
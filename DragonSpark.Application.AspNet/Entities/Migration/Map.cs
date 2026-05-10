using DragonSpark.Application.AspNet.Entities.Migration.Migrators;
using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Operations;
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

	Map() : this(CopyValues.Default, MapOwned.Default) {}

	readonly ICommand<MapInput>                _copy;
	readonly ICommand<MapNavigationEntryInput> _owned;
	
	public Map(ICommand<MapInput> copy, ICommand<MapNavigationEntryInput> owned)
	{
		_copy  = copy;
		_owned = owned;
	}

	public ValueTask Get(Stop<MapInput> parameter)
	{
		var ((from, to), _) = parameter;
		_copy.Execute(parameter);

		using var navigations = to.Context.Entry(to.Entity)
		                          .Navigations.AsValueEnumerable()
		                          .ToArray(ArrayPool<NavigationEntry>.Shared);
		foreach (var navigation in from.Metadata.GetNavigations().Where(x => x.TargetEntityType.IsOwned()))
		{
			var entry = navigations.FirstOrDefault(x => x.Metadata.Name == navigation.Name);

			if (entry is not null)
			{
				_owned.Execute(new(from.Navigation(navigation.Name), entry));
			}
		}

		return ValueTask.CompletedTask;
	}
}


public sealed class Map<TFrom, TTo> : IMap
{
	readonly Func<Stop<MapInput<TFrom, TTo>>, ValueTask> _map;
	readonly IMap                                        _previous;

	public Map(Action<TFrom, TTo> mapping) : this(mapping, Map.Default) {}

	public Map(Action<TFrom, TTo> mapping, IMap map)
		: this(x =>
		       {
			       mapping(x.Subject.From.Entity, x.Subject.To.Entity);
			       return ValueTask.CompletedTask;
		       }, 
		       map) {}

	public Map(Action<MapInput<TFrom, TTo>> input) : this(input, Map.Default) {}

	public Map(Action<MapInput<TFrom, TTo>> input, IMap map)
		: this(x =>
		       {
			       input(x.Subject);
			       return ValueTask.CompletedTask;
		       }, map) {}

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
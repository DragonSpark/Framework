using DragonSpark.Application.AspNet.Entities.Migration.Migrators;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using NetFabric.Hyperlinq;
using System.Buffers;
using System.Collections.Immutable;
using System.Linq;

namespace DragonSpark.Application.AspNet.Entities.Migration;

public sealed class CopyValues : ICommand<MapInput>
{
	public static CopyValues Default { get; } = new();

	CopyValues() : this(Names.Default, DetermineValue.Default) {}

	readonly ISelect<IEntityType, ImmutableHashSet<string>> _names;
	readonly ISelect<DetermineValueInput, object?>          _value;

	public CopyValues(ISelect<IEntityType, ImmutableHashSet<string>> names, ISelect<DetermineValueInput, object?> value)
	{
		_names = names;
		_value = value;
	}

	public void Execute(MapInput parameter)
	{
		var (from, to) = parameter;

		var names = _names.Get(to.Metadata);
		var key   = from.Metadata.FindPrimaryKey()?.Properties ?? [];
		for (byte i = 0; i < key.Count; i++)
		{
			var name = key[i].Name;
			if (names.Contains(name))
			{
				to.Property(name).CurrentValue = ExtractValueFromSource(name);
			}
		}

		switch (to.State)
		{
			case EntityState.Detached:
				to.Context.Add(to.Entity);
				break;
		}

		using var properties = from.CurrentValues.Properties.Except(key)
		                           .AsValueEnumerable()
		                           .ToArray(ArrayPool<IProperty>.Shared);
		foreach (var property in properties)
		{
			var name = property.Name;
			if (names.Contains(name))
			{
				to.Property(name).CurrentValue = ExtractValueFromSource(name);
			}
		}

		return;

		object? ExtractValueFromSource(string name)
		{
			var value  = from.CurrentValues[name];
			var result = value is not null ? _value.Get(new(name, value, to)) : null;
			return result;
		}
	}
}
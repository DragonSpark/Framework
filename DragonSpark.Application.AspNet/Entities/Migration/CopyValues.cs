using DragonSpark.Application.AspNet.Entities.Migration.Migrators;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection;
using System.Collections.Generic;

namespace DragonSpark.Application.AspNet.Entities.Migration;

sealed class CopyValues : ICommand<MapInput>
{
	public static CopyValues Default { get; } = new();

	CopyValues() : this(DetermineValue.Default) {}

	readonly ISelect<DetermineValueInput, object?> _value;

	public CopyValues(ISelect<DetermineValueInput, object?> value) => _value = value;

	public void Execute(MapInput parameter)
	{
		var (from, to) = parameter;

		var values = new Dictionary<string, object?>();

		for (byte i = 0; i < from.CurrentValues.Properties.Count; i++)
		{
			var name  = from.CurrentValues.Properties[i].Name;
			var value = from.CurrentValues[name];
			values[name] = value is not null ? _value.Get(new(name, value, to)) : null;
		}

		to.CurrentValues.SetValues(values);
	}
}
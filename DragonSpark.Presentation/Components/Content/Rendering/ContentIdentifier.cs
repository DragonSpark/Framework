using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection.Stores;
using DragonSpark.Text;
using System;
using System.Collections.Generic;

namespace DragonSpark.Presentation.Components.Content.Rendering;

sealed class ContentIdentifier : IFormatter<object>, ICommand<object>
{
	readonly ITable<Type, HashSet<int>> _counters;

	public ContentIdentifier(ContentIdentifierStore counters) => _counters = counters;

	public string Get(object parameter)
	{
		var type = parameter.GetType();
		var set  = _counters.Get(type);
		set.Add(parameter.GetHashCode());
		var result = $"{type.FullName}+{set.Count}";
		return result;
	}

	public void Execute(object parameter)
	{
		var type = parameter.GetType();
		_counters.Get(type).Clear();
	}
}
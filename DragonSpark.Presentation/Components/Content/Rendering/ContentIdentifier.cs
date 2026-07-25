using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection.Stores;
using DragonSpark.Text;
using System;
using System.Collections.Generic;

namespace DragonSpark.Presentation.Components.Content.Rendering;

sealed class ContentIdentifier : IFormatter<ContentKeyInput>, ICommand<Type>
{
	readonly ITable<Type, HashSet<int>> _counters;

	public ContentIdentifier(ContentIdentifierStore counters) => _counters = counters;

	public string Get(ContentKeyInput parameter)
	{
		var (type, pointer) = parameter;
		var set = _counters.Get(type);
		set.Add(pointer);
		return $"{type.FullName}+{set.Count}";
	}

	public void Execute(Type parameter)
	{
		_counters.Get(parameter).Clear();
	}
}
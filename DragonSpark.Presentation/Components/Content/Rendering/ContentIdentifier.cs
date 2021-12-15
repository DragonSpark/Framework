using DragonSpark.Compose;
using DragonSpark.Model.Selection.Stores;
using DragonSpark.Text;
using System;
using System.Collections.Generic;

namespace DragonSpark.Presentation.Components.Content.Rendering;

sealed class ContentIdentifier : IFormatter<object>
{
	readonly ITable<Type, HashSet<object>> _counters;

	public ContentIdentifier() : this(new Dictionary<Type, HashSet<object>>().ToTable(_ => new HashSet<object>())) {}

	public ContentIdentifier(ITable<Type, HashSet<object>> counters) => _counters = counters;

	public string Get(object parameter)
	{
		var type = parameter.GetType();
		var set  = _counters.Get(type);
		set.Add(parameter);
		var result = $"{type.AssemblyQualifiedName}+{set.Count}";
		return result;
	}
}
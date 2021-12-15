using DragonSpark.Compose;
using DragonSpark.Model.Selection.Stores;
using DragonSpark.Text;
using System;
using System.Collections.Generic;

namespace DragonSpark.Presentation.Components.Content.Rendering;

sealed class ContentIdentifier : IFormatter<Microsoft.AspNetCore.Components.ComponentBase>
{
	readonly ITable<Type, HashSet<Microsoft.AspNetCore.Components.ComponentBase>> _counters;

	public ContentIdentifier()
		: this(new Dictionary<Type, HashSet<Microsoft.AspNetCore.Components.ComponentBase>>()
			       .ToTable(_ => new HashSet<Microsoft.AspNetCore.Components.ComponentBase>())) {}

	public ContentIdentifier(ITable<Type, HashSet<Microsoft.AspNetCore.Components.ComponentBase>> counters)
		=> _counters = counters;

	public string Get(Microsoft.AspNetCore.Components.ComponentBase parameter)
	{
		var type = parameter.GetType();
		var set  = _counters.Get(type);
		set.Add(parameter);
		var result = $"{type.AssemblyQualifiedName}+{set.Count}";
		return result;
	}
}
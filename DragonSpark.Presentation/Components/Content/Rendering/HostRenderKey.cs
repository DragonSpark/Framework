using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using DragonSpark.Text;
using System;

namespace DragonSpark.Presentation.Components.Content.Rendering;

public class HostRenderKey<T> : IFormatter<T>
{
	readonly ISelect<T, string> _key;
	readonly string             _qualifier;

	protected HostRenderKey(ISelect<T, string> key, Type qualifier)
		: this(key, qualifier.AssemblyQualifiedName.Verify()) {}

	protected HostRenderKey(ISelect<T, string> key, string qualifier)
	{
		_key       = key;
		_qualifier = qualifier;
	}

	public string Get(T parameter) => $"{_key.Get(parameter)}+{_qualifier}";
}
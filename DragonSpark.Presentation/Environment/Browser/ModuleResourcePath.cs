using DragonSpark.Compose;
using DragonSpark.Text;
using System;

namespace DragonSpark.Presentation.Environment.Browser;

sealed class ModuleResourcePath : IFormatter<Type>
{
	public static ModuleResourcePath Default { get; } = new();

	ModuleResourcePath() {}

	public string Get(Type parameter)
	{
		var assembly   = parameter.Assembly.GetName().Name.Verify();
		var @namespace = parameter.Namespace.Verify().Replace(assembly, string.Empty).Replace(".", "/");
		var result     = $"./_content/{assembly}/{@namespace}/{parameter.Name}.js";
		return result;
	}
}
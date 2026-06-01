using DragonSpark.Compose;
using DragonSpark.Text;
using System;

namespace DragonSpark.Presentation.Environment.Browser;

sealed class ComposeModulePath : IFormatter<Type>
{
	public static ComposeModulePath Default { get; } = new();

	ComposeModulePath() {}
	
	public string Get(Type parameter)
	{
		var assembly     = parameter.Assembly.GetName().Name.Verify();
		var @namespace   = parameter.Namespace ?? string.Empty;
		var internalPath = @namespace.StartsWith(assembly) ? @namespace[assembly.Length..].TrimStart('.') : @namespace;
		var directory    = internalPath.Replace('.', '/');
		var name         = directory.IsAssigned() ? $"{directory}/{parameter.Name}" : parameter.Name;
		var result       = $"_content/{assembly}/{name}.razor.js";
		return result;
	}
}
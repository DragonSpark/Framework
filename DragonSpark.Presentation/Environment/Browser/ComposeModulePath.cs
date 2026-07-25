using DragonSpark.Compose;
using DragonSpark.Runtime.Environment;
using DragonSpark.Text;
using System.Reflection;

namespace DragonSpark.Presentation.Environment.Browser;

sealed class ComposeModulePath : IFormatter<Type>
{
	public static ComposeModulePath Default { get; } = new();

	ComposeModulePath() : this(PrimaryAssembly.Default) {}
	
	readonly Assembly _primary;

	public ComposeModulePath(Assembly primary) => _primary = primary;

	public string Get(Type parameter)
	{
		var assembly     = parameter.Assembly.GetName().Name.Verify();
		var @namespace   = parameter.Namespace ?? string.Empty;
		var internalPath = @namespace.StartsWith(assembly) ? @namespace[assembly.Length..].TrimStart('.') : @namespace;
		var directory    = internalPath.Replace('.', '/');
		var name         = directory.IsAssigned() ? $"{directory}/{parameter.Name}" : parameter.Name;
		var qualifier    = parameter.Assembly == _primary ? string.Empty : $"_content/{assembly}/";
		var result       = $"{qualifier}{name}.razor.js";
		return result;
	}
}
using DragonSpark.Model.Selection.Alterations;
using System.Reflection;

namespace DragonSpark.Runtime.Environment;

class ExternalAssemblyName : IAlteration<AssemblyName>
{
	readonly string _format;

	public ExternalAssemblyName(string format) => _format = format;

	public AssemblyName Get(AssemblyName parameter) => new AssemblyName(string.Format(_format, parameter.Name));
}
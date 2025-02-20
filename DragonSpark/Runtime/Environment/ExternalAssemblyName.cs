using System.Reflection;
using DragonSpark.Model.Selection.Alterations;

namespace DragonSpark.Runtime.Environment;

class ExternalAssemblyName : IAlteration<AssemblyName>
{
	readonly string _format;

    protected ExternalAssemblyName(string format) => _format = format;

	public AssemblyName Get(AssemblyName parameter) => new(string.Format(_format, parameter.Name));
}